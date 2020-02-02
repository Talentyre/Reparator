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
    public Image TimerGauge;
    public Animator MouthAnimator;
    
    public event Action OnWinEvent;
    public event Action OnLoseEvent;
    
    private float _currentTimer;
    private bool _init;
    private bool _warningOn;
    private bool _started;

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
            return;
        }
        
        if (!_started)
            OnStart();

        
        
        if (_currentTimer <= 0)
        {
            OnLost();
            return;
        }
        
        _currentTimer -= Time.deltaTime;
        if (!_warningOn && _currentTimer < Timer / 2.5)
        {
            _warningOn = true;
        }

        var transformLocalPosition = TimerGauge.transform.localPosition;
        transformLocalPosition.x = -TimerGauge.rectTransform.rect.size.x * (1 - _currentTimer / Timer);
        TimerGauge.transform.localPosition = transformLocalPosition;
    }

    protected virtual void OnStart()
    {
        _started = true;
        MouthAnimator.SetBool("start", true);
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
