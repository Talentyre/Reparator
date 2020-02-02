using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TicTacToe : MiniGameUI
{
    public Sprite CrossImage;
    public Sprite CircleImage;
    public GameObject ButtonsPanel;
    
    private static readonly List<TicTacToeSetup> _setups = new List<TicTacToeSetup>
    {
        new TicTacToeSetup()
        {
            Board = new[,]
            {
                {TicTacToeSymbol.Circle,TicTacToeSymbol.None, TicTacToeSymbol.Cross},
                {TicTacToeSymbol.None,TicTacToeSymbol.Cross, TicTacToeSymbol.Cross},
                {TicTacToeSymbol.Circle,TicTacToeSymbol.None, TicTacToeSymbol.Circle},
            },
            CorrectPosition = new Vector2(0,1)
        }
        ,
        new TicTacToeSetup()
        {
        Board = new[,]
        {
            {TicTacToeSymbol.Cross,TicTacToeSymbol.Circle, TicTacToeSymbol.Cross},
            {TicTacToeSymbol.Circle,TicTacToeSymbol.None, TicTacToeSymbol.Circle},
            {TicTacToeSymbol.Cross,TicTacToeSymbol.None, TicTacToeSymbol.None},
        },
        CorrectPosition = new Vector2(1,1)
        },
        new TicTacToeSetup()
        {
        Board = new[,]
        {
            {TicTacToeSymbol.None,TicTacToeSymbol.Cross, TicTacToeSymbol.Circle},
            {TicTacToeSymbol.Cross,TicTacToeSymbol.Circle, TicTacToeSymbol.Circle},
            {TicTacToeSymbol.Cross,TicTacToeSymbol.None, TicTacToeSymbol.None},
        },
        CorrectPosition = new Vector2(0,0)
        },
        new TicTacToeSetup()
        {
        Board = new[,]
        {
            {TicTacToeSymbol.Circle,TicTacToeSymbol.Cross, TicTacToeSymbol.Cross},
            {TicTacToeSymbol.Circle,TicTacToeSymbol.Cross, TicTacToeSymbol.None},
            {TicTacToeSymbol.None,TicTacToeSymbol.Circle, TicTacToeSymbol.None},
        },
        CorrectPosition = new Vector2(0,2)
        },
        new TicTacToeSetup()
        {
            Board = new[,]
            {
                {TicTacToeSymbol.None,TicTacToeSymbol.Cross, TicTacToeSymbol.Circle},
                {TicTacToeSymbol.Circle,TicTacToeSymbol.Circle, TicTacToeSymbol.None},
                {TicTacToeSymbol.Cross,TicTacToeSymbol.None, TicTacToeSymbol.Cross},
            },
            CorrectPosition = new Vector2(1,2)
        },
        new TicTacToeSetup()
        {
        Board = new[,]
        {
            {TicTacToeSymbol.Cross,TicTacToeSymbol.Cross, TicTacToeSymbol.None},
            {TicTacToeSymbol.None,TicTacToeSymbol.Circle, TicTacToeSymbol.None},
            {TicTacToeSymbol.Circle,TicTacToeSymbol.Cross, TicTacToeSymbol.Circle},
        },
        CorrectPosition = new Vector2(2,0)
        },
    };

    private TicTacToeSetup _pickedSetup;
    private TicTacToeButton[] _buttons;
    private TicTacToeButton _selectedButton = null;
    private TweenerCore<Color, Color, ColorOptions> _currentTween;
    private float _axisCooldown;


    void Start()
    {
        base.Start();
        
        _buttons = ButtonsPanel.GetComponentsInChildren<TicTacToeButton>();
        _pickedSetup = _setups[Random.Range(0, _setups.Count)];
        int i = 0;
        List<TicTacToeButton> emptyButtons = new List<TicTacToeButton>();
        foreach (var ticTacToeSymbol in _pickedSetup.Board)
        {
            // todo la flemme (constantes)
            var position = new Vector2(i%3, i / 3);
            
            var ticTacToeButton = _buttons[i];
            ticTacToeButton.SymbolImage.sprite = ticTacToeSymbol == TicTacToeSymbol.Circle ? CircleImage :
                ticTacToeSymbol == TicTacToeSymbol.Cross ? CrossImage : null;
            ticTacToeButton.Position = position;
            
            var button = ticTacToeButton.GetComponent<Button>();
            if (ticTacToeSymbol == TicTacToeSymbol.None)
            {
                emptyButtons.Add(ticTacToeButton);
                
                var symbolImageColor = ticTacToeButton.SymbolImage.color;
                symbolImageColor.a = 0f;
                ticTacToeButton.SymbolImage.color = symbolImageColor;
                
                button.onClick.AddListener(() => OnButtonClicked(ticTacToeButton));
            }
            else
            {
                button.interactable = false;
            }
            
            i++;
        }

        var validButtons = emptyButtons.Where(b => b.Position != _pickedSetup.CorrectPosition).ToList();
        var tacToeButton = validButtons[Random.Range(0, validButtons.Count())];
        FindObjectOfType<EventSystem>().SetSelectedGameObject(tacToeButton.gameObject);
        OnButtonClicked(tacToeButton);
    }

    private void OnButtonClicked(TicTacToeButton ticTacToeButton)
    {
        if (_currentTween != null)
            _currentTween.Pause();

        if (_selectedButton != null)
        {
            _selectedButton.SymbolImage.sprite = null;
            var col = _selectedButton.SymbolImage.color;
            col.a = 0f;
            _selectedButton.SymbolImage.color = col;
            
            _selectedButton.GetComponent<Button>().interactable = true;   
        }
        
        _selectedButton = ticTacToeButton;
        _selectedButton.SymbolImage.sprite = CrossImage;
        _selectedButton.GetComponent<Button>().interactable = false;

        var symbolImageColor = _selectedButton.SymbolImage.color;
        symbolImageColor.a = 0f;
        _selectedButton.SymbolImage.color = symbolImageColor;
        _currentTween = _selectedButton.SymbolImage.DOFade(1f, .5f).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        
        UpdateControls();
    }

    private void UpdateControls()
    {
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");

        _axisCooldown -= Time.deltaTime;
        if (Math.Abs(vertical) > 0.001f && _axisCooldown <= 0)
        {
            _axisCooldown = 0.5f;
            int offsetY = vertical > 0 ? -1 : 1;
            if (!CheckMovement(offsetY, 0) && !CheckMovement(offsetY*2 ,0))
            {
                if (!CheckMovement(offsetY, 1) && !CheckMovement(offsetY*2, 1))
                {
                    if (!CheckMovement(offsetY, 2) && !CheckMovement(offsetY*2, 2))
                    {
                        if (!CheckMovement(offsetY, -1) && !CheckMovement(offsetY*2, -1))
                        {
                            if (!CheckMovement(offsetY, -2) && !CheckMovement(offsetY*2, -2))
                            {
                                Debug.LogWarning("No case found for vertical movement !!!");
                            }
                        }
                    }
                }
            }
        }
        
        if (Math.Abs(horizontal) > 0.001f && _axisCooldown <= 0)
        {
            _axisCooldown = 0.5f;
            int offsetX = horizontal > 0 ? 1 : -1;
            if (!CheckMovement(0, offsetX) && ! CheckMovement(0 ,offsetX*2))
            {
                if (!CheckMovement(1, offsetX) && !CheckMovement(1, offsetX*2))
                {
                    if (!CheckMovement(2, offsetX) && !CheckMovement(2, offsetX*2))
                    {
                        if (!CheckMovement(-1, offsetX) && !CheckMovement(-1, offsetX*2))
                        {
                            if (!CheckMovement(-2, offsetX) && !CheckMovement(-2, offsetX*2))
                            {
                                Debug.LogWarning("No case found for horizontal movement !!!");
                            }
                        }
                    }
                }
            }
        }
        
        

        if (Input.GetButton("Submit"))
        {
            if (_selectedButton.Position == _pickedSetup.CorrectPosition)
            {
                OnWin();
            }
            else
            {
                OnLost();
            }
        }
    }

    private bool CheckMovement(int offsetY, int offsetX)
    {
        if (_selectedButton.Position.y + offsetY >= 0 && _selectedButton.Position.y + offsetY < 3 &&
            _selectedButton.Position.x + offsetX >= 0 && _selectedButton.Position.x + offsetX < 3 && 
            _pickedSetup.Board[(int) (_selectedButton.Position.y + offsetY), (int) _selectedButton.Position.x + offsetX] ==
            TicTacToeSymbol.None)
        {
            OnButtonClicked(
                _buttons[(int) (_selectedButton.Position.x + offsetX + (_selectedButton.Position.y + offsetY) * 3)]);
            return true;
        }

        return false;
    }
}
