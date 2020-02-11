using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Simon : MiniGameUI
{
    public float ButtonDuration = 1f;
    
    public List<SimonButton> SimonButtons = new List<SimonButton>();
    
    private List<SimonButton> ButtonSequence = new List<SimonButton>();

    private bool _playerTurn;
    private float _timer;
    private int _turnCount = 1;
    private int _chosenButtonCount = 0;

	public AudioClip ButtonClicClip;
	public AudioClip ButtonLightClip;

	void Start()
    {
        base.Start();

        foreach (var simonButton in SimonButtons)
        {
            simonButton.GetComponent<Button>().onClick.AddListener(() => OnButtonClicked(simonButton));
            var topImageColor = simonButton.TopImage.color;
            topImageColor.a = 0f;
            simonButton.TopImage.color = topImageColor;
        }

        for (int i = 0; i < 3; i++)
        {
            ButtonSequence.Add(SimonButtons[Random.Range(0, SimonButtons.Count)]);
        }
    }

    private void SetSelectedButton()
    {
        FindObjectOfType<EventSystem>().SetSelectedGameObject(SimonButtons[0].gameObject);
    }

    protected override void OnStart()
    {
        base.OnStart();
        ComputerTurn();
    }

    private void ComputerTurn()
    {
        _chosenButtonCount = 0;
        _playerTurn = false;
        ActivateButtons(false);
        
        StartCoroutine(ComputerTurnCoroutine());
    }

    private void OnButtonClicked(SimonButton simonButton)
    {
        if (!_playerTurn)
            return;

		AudioManager.Instance.PlaySFX (ButtonClicClip);

        if (simonButton != ButtonSequence[_chosenButtonCount])
        {
            OnLost();
        }
        else
        {
            _chosenButtonCount++;
            
            if (_chosenButtonCount >= _turnCount)
            {
                _turnCount++;
                if (_turnCount > 3)
                {
                    OnWin();
                }
                else
                {
                    ComputerTurn();   
                }
            }
        }
    }

    private IEnumerator ComputerTurnCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        
        for (int i = 0; i < _turnCount; i++)
        {
            var pickedButton = ButtonSequence[i];

            pickedButton.TopImage.DOFade(1f, .35f);
			AudioManager.Instance.PlaySFX (ButtonLightClip);    

            yield return new WaitForSeconds(.5f);
            
            pickedButton.TopImage.DOFade(0f, .35f);
            
            yield return new WaitForSeconds(0.5f);
        }

        PlayerTurn();
    }

    private void PlayerTurn()
    {
        ActivateButtons(true);
        SetSelectedButton();
        _playerTurn = true;
    }

    void Update()
    {
        base.Update();
    }

    private void ActivateButtons(bool activate)
    {
        foreach (var simonButton in SimonButtons)
        {
            simonButton.GetComponent<Button>().interactable = activate;
        }
    }
}
