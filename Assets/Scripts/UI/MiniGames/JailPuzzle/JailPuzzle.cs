using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class JailPuzzle : MiniGameUI
{
    private static readonly List<JailPuzzleSetup> _setups = new List<JailPuzzleSetup>()
    {
        new JailPuzzleSetup()
        {
            Board = new[,]
            {
                {JailPuzzlePiece.PipeLb, JailPuzzlePiece.Empty, JailPuzzlePiece.PipeRb},
                {JailPuzzlePiece.PipeRt, JailPuzzlePiece.PipeLr, JailPuzzlePiece.PipeLt},
                {JailPuzzlePiece.Empty, JailPuzzlePiece.Empty, JailPuzzlePiece.Empty},
            }
        },
        new JailPuzzleSetup()
        {
            Board = new[,]
            {
                {JailPuzzlePiece.PipeLb, JailPuzzlePiece.Empty, JailPuzzlePiece.Empty},
                {JailPuzzlePiece.PipeRt, JailPuzzlePiece.PipeLb, JailPuzzlePiece.Empty},
                {JailPuzzlePiece.Empty, JailPuzzlePiece.PipeRt, JailPuzzlePiece.PipeLr},
            }
        },
        new JailPuzzleSetup()
        {
            Board = new[,]
            {
                {JailPuzzlePiece.Empty, JailPuzzlePiece.Empty, JailPuzzlePiece.Empty},
                {JailPuzzlePiece.Empty, JailPuzzlePiece.PipeRb, JailPuzzlePiece.PipeLr},
                {JailPuzzlePiece.PipeLr, JailPuzzlePiece.PipeLt, JailPuzzlePiece.Empty},
            }
        },
    };

    public GameObject ButtonsPanel;
    private JailPuzzleButton[] _buttons;
    private JailPuzzleSetup _pickedSetup;

    public Sprite SpriteLb;
    public Sprite SpriteLr;
    public Sprite SpriteLt;
    public Sprite SpriteRb;
    public Sprite SpriteRt;
    public Sprite SpriteTb;
    public List<Sprite> SpriteVoid = new List<Sprite>();
    private EventSystem _eventSystem;
    private float _axisCooldown;
    private JailPuzzleButton _selectedButton;
    private List<Sprite> SpriteVoidCache;

	public AudioClip ButtonRotateClip;

	public override void Start()
    {
        base.Start();
        SpriteVoidCache = SpriteVoid.ToList();
        _buttons = ButtonsPanel.GetComponentsInChildren<JailPuzzleButton>();
        _pickedSetup = _setups[Random.Range(0, _setups.Count)];

        _eventSystem = FindObjectOfType<EventSystem>();

        int i = 0;
        bool firstGoSetup = false;
        foreach (var jailPuzzlePiece in _pickedSetup.Board)
        {
            var jailPuzzleButton = _buttons[i];
            var position = new Vector2(i % 3, i / 3);
            jailPuzzleButton.Position = position;

            var button = jailPuzzleButton.GetComponent<Button>();
            button.image.sprite = GetSprite(jailPuzzlePiece);

            jailPuzzleButton.JailPuzzlePiece = jailPuzzlePiece;

            if (jailPuzzlePiece != JailPuzzlePiece.Empty)
            {
                if (!firstGoSetup)
                {
                    SetSelectedButton(jailPuzzleButton, button);
                    firstGoSetup = true;
                }


                button.onClick.AddListener(() => OnClickButton(jailPuzzleButton));
                jailPuzzleButton.CurrentRotation = Random.Range(1, 4);
                SetButtonRotation(jailPuzzleButton);
            }
            else
            {
                button.interactable = false;
            }

            i++;
        }
    }

    private void SetSelectedButton(JailPuzzleButton jailPuzzleButton, Button button)
    {
        _selectedButton = jailPuzzleButton;
        _eventSystem.SetSelectedGameObject(button.gameObject);
    }

    public override void Update()
    {
        base.Update();
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");

        _axisCooldown -= Time.deltaTime;
        if (Math.Abs(vertical) > 0.001f && _axisCooldown <= 0)
        {
            _axisCooldown = 0.5f;
            int offsetY = vertical > 0 ? -1 : 1;
            if (!CheckMovement(offsetY, 0) && !CheckMovement(offsetY * 2, 0))
            {
                if (!CheckMovement(offsetY, 1) && !CheckMovement(offsetY * 2, 1))
                {
                    if (!CheckMovement(offsetY, 2) && !CheckMovement(offsetY * 2, 2))
                    {
                        if (!CheckMovement(offsetY, -1) && !CheckMovement(offsetY * 2, -1))
                        {
                            if (!CheckMovement(offsetY, -2) && !CheckMovement(offsetY * 2, -2))
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
            if (!CheckMovement(0, offsetX) && !CheckMovement(0, offsetX * 2))
            {
                if (!CheckMovement(1, offsetX) && !CheckMovement(1, offsetX * 2))
                {
                    if (!CheckMovement(2, offsetX) && !CheckMovement(2, offsetX * 2))
                    {
                        if (!CheckMovement(-1, offsetX) && !CheckMovement(-1, offsetX * 2))
                        {
                            if (!CheckMovement(-2, offsetX) && !CheckMovement(-2, offsetX * 2))
                            {
                                Debug.LogWarning("No case found for horizontal movement !!!");
                            }
                        }
                    }
                }
            }
        }
    }

    private bool CheckMovement(int offsetY, int offsetX)
    {
        if (_selectedButton.Position.y + offsetY >= 0 && _selectedButton.Position.y + offsetY < 3 &&
            _selectedButton.Position.x + offsetX >= 0 && _selectedButton.Position.x + offsetX < 3 &&
            _pickedSetup.Board[(int) (_selectedButton.Position.y + offsetY),
                (int) _selectedButton.Position.x + offsetX] !=
            JailPuzzlePiece.Empty)
        {
            var jailPuzzleButton =
                _buttons[(int) (_selectedButton.Position.x + offsetX + (_selectedButton.Position.y + offsetY) * 3)];
            SetSelectedButton(jailPuzzleButton, jailPuzzleButton.GetComponent<Button>());
            return true;
        }

        return false;
    }

    private void OnClickButton(JailPuzzleButton jailPuzzleButton)
    {
        jailPuzzleButton.CurrentRotation++;
        if (jailPuzzleButton.CurrentRotation > 3)
            jailPuzzleButton.CurrentRotation = 0;

        SetButtonRotation(jailPuzzleButton);
		AudioManager.Instance.PlaySFX (ButtonRotateClip);
        CheckWin();
    }

    private void CheckWin()
    {
        foreach (var jailPuzzleButton in _buttons)
        {
            var currentRotation = jailPuzzleButton.CurrentRotation;
            if (!(jailPuzzleButton.JailPuzzlePiece == JailPuzzlePiece.PipeLr &&
                  (currentRotation == 0 || currentRotation == 2)) &&
                currentRotation > 0)
                return;
        }

        OnWin();
    }

    private void SetButtonRotation(JailPuzzleButton button)
    {
        button.transform.rotation = Quaternion.Euler(0, 0, 90 * button.CurrentRotation);
    }

    private Sprite GetSprite(JailPuzzlePiece jailPuzzlePiece)
    {
        switch (jailPuzzlePiece)
        {
            case JailPuzzlePiece.PipeLb:
            {
                return SpriteLb;
            }
            case JailPuzzlePiece.PipeLr:
            {
                return SpriteLr;
            }
            case JailPuzzlePiece.PipeLt:
            {
                return SpriteLt;
            }
            case JailPuzzlePiece.PipeRb:
            {
                return SpriteRb;
            }
            case JailPuzzlePiece.PipeRt:
            {
                return SpriteRt;
            }
            case JailPuzzlePiece.PipeTb:
            {
                return SpriteTb;
            }
        }

        if (SpriteVoidCache.Count == 0)
            SpriteVoidCache = SpriteVoid.ToList();
        var rand = Random.Range(0, SpriteVoidCache.Count);
        var sprite = SpriteVoidCache[rand];
        SpriteVoidCache.RemoveAt(rand);
        return sprite;
    }
}