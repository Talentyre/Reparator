using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameUI : MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    [Header("Timer")]
    public float StartTimer = 1f;
    public float Timer = 3f;
    public Slider TimerSlider;
    public Image WarningImage;

    public event Action OnWinEvent;
    public event Action OnLoseEvent;
    
    private float _currentTimer;
    private bool _init;
    private bool _warningOn;

    // Start is called before the first frame update
    public virtual void Start()
    {
        CanvasGroup.alpha = 0f;
        _currentTimer = Timer;
        
        CanvasGroup.DOFade(1f, .5f).OnComplete(() => _init = true);
    }
    
    public virtual void Update()
    {
        UpdateTimer();
    }
    
    private void UpdateTimer()
    {
        if (!_init)
            return;

        StartTimer -= Time.deltaTime;
        if (StartTimer > 0)
        {
            OnStart();
            return;
        }

        
        
        if (_currentTimer <= 0)
        {
            OnLost();
            return;
        }
        
        _currentTimer -= Time.deltaTime;
        if (!_warningOn && _currentTimer < Timer / 2.5)
        {
            _warningOn = true;
            WarningImage.transform.DOShakePosition(0.25f, 5f,30).SetLoops(-1);
        }
        TimerSlider.value = _currentTimer / Timer;
    }

    protected virtual void OnStart()
    {
        
    }

    protected void OnWin()
    {
        Debug.Log("Gagné");
        Close();
        if (OnWinEvent != null)
            OnWinEvent();
    }

    protected void OnLost()
    {
        Debug.Log("perdu");
        Close();
        if (OnLoseEvent != null)
            OnWinEvent();
    }

    private void Close()
    {
        CanvasGroup.DOFade(0f, .5f).OnComplete(() => Destroy(gameObject));
    }
}
