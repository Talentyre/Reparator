using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    
    public override void Start()
    {
        base.Start();
        _buttons = ButtonsPanel.GetComponentsInChildren<JailPuzzleButton>();
        _pickedSetup = _setups[Random.Range(0, _setups.Count)];

        int i = 0;
        foreach (var jailPuzzlePiece in _pickedSetup.Board)
        {
            var jailPuzzleButton = _buttons[i];
            var button = jailPuzzleButton.GetComponent<Button>();
            button.image.sprite = GetSprite(jailPuzzlePiece);

            jailPuzzleButton.JailPuzzlePiece = jailPuzzlePiece;
            
            if (jailPuzzlePiece != JailPuzzlePiece.Empty)
            {
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

    private void OnClickButton(JailPuzzleButton jailPuzzleButton)
    {
        jailPuzzleButton.CurrentRotation++;
        if (jailPuzzleButton.CurrentRotation > 3)
            jailPuzzleButton.CurrentRotation = 0;
        
        SetButtonRotation(jailPuzzleButton);
        CheckWin();
    }

    private void CheckWin()
    {
        foreach (var jailPuzzleButton in _buttons)
        {
            var currentRotation = jailPuzzleButton.CurrentRotation;
            Debug.Log("piece = "+ jailPuzzleButton.JailPuzzlePiece+"rot = "+currentRotation);
            if (!(jailPuzzleButton.JailPuzzlePiece == JailPuzzlePiece.PipeLr && (currentRotation == 0 || currentRotation == 2)) &&
                currentRotation > 0)
                return;
        }
        OnWin();
    }

    private void SetButtonRotation(JailPuzzleButton button)
    {
        button.transform.rotation = Quaternion.Euler(0, 0, 90* button.CurrentRotation);
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

        var rand = Random.Range(0, SpriteVoid.Count);
        var sprite = SpriteVoid[rand];
        SpriteVoid.RemoveAt(rand);
        return sprite;
    }
}
