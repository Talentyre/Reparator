using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

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
            {TicTacToeSymbol.Circle,TicTacToeSymbol.None, TicTacToeSymbol.Cross},
            {TicTacToeSymbol.Circle,TicTacToeSymbol.Cross, TicTacToeSymbol.Cross},
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
            
            if (ticTacToeSymbol == TicTacToeSymbol.None)
                emptyButtons.Add(ticTacToeButton);
            
            var button = ticTacToeButton.GetComponent<Button>();
            if (ticTacToeSymbol == TicTacToeSymbol.None)
            {
                button.onClick.AddListener(() => OnButtonClicked(ticTacToeButton));
            }
            else
            {
                button.interactable = false;
            }
            
            i++;
        }

        var validButtons = emptyButtons.Where(b => b.Position != _pickedSetup.CorrectPosition).ToList();
        OnButtonClicked(validButtons[Random.Range(0, validButtons.Count())]);
    }

    private void OnButtonClicked(TicTacToeButton ticTacToeButton)
    {
        if (_currentTween != null)
            _currentTween.Pause();

        if (_selectedButton != null)
        {
            _selectedButton.SymbolImage.sprite = null;
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
}
