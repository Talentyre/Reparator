using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MiniGameManager))]
public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("Player")] public PlayerController PlayerController;
    [Header("A* Mesh")] public AStarMesh AStarMesh;

    [Header("GamePlay")] [SerializeField] int _totalPropCount = 10;
    public float GameDuration = 60 * 5;
	public bool TimeElapsing = true;

    // todo maybe put this in a specific UI class
    [Header("UI")] public Text PropCountText;
    public Text TimerText;
    public GameObject GameOverUI;
    
    private MiniGameManager _miniGameManager;
    private int _propCount;

    public List<Room> Rooms = new List<Room>();
    public List<GameObject> PropsPrefabs = new List<GameObject>();
    public Transform PropsParent;
    private bool _gameOver;

	[Header ("Audio")]
	public AudioClip WinMiniGameClip;
	public AudioClip LoseMiniGameClip;
	public AudioClip VictoryClip;
	public AudioClip GameOverClip;

	private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad (gameObject);
    }

    void Start()
    {
        _miniGameManager = GetComponent<MiniGameManager>();

        SetupProps();
        // todo after propCount set
        UpdatePropCountUI();
    }

    private void SetupProps()
    {
        List<GameObject> cachePrefabs = PropsPrefabs.ToList();
        List<Room> cacheRooms = Rooms.ToList();
        for (int i = 0; i < _totalPropCount; i++)
        {
            if (cachePrefabs.Count == 0)
                cachePrefabs = PropsPrefabs.ToList();
            var cachePrefab = cachePrefabs[Random.Range(0, cachePrefabs.Count)];
            cachePrefabs.Remove(cachePrefab);

            var propGo = Instantiate(cachePrefab, PropsParent);
            var prop = propGo.GetComponent<Prop>();

            bool hideOutFound = false;
            // todo porkass2000
            int maxLoop = 100;
            while (!hideOutFound)
            {
                maxLoop--;
                if (maxLoop <= 0)
                {
                    Debug.LogError("Can't find hideout oO");
                    break;
                }
                    
                // get a random room
                if (cacheRooms.Count == 0)
                    cacheRooms = Rooms.ToList();
                var cacheRoom = cacheRooms[Random.Range(0, cacheRooms.Count)];
                cacheRooms.Remove(cacheRoom);

                var availableHideouts =
                    cacheRoom.Hideouts.Where(h => h.Available && h.Type == prop.HideoutType).ToList();
                if (availableHideouts.Count == 0)
                    continue;
                hideOutFound = true;
                prop.CurrentHideout = availableHideouts[Random.Range(0, availableHideouts.Count)];
                prop.CurrentHideout.Available = false;
                prop.transform.position = prop.CurrentHideout.Position;
                
                prop.CurrentRoom = cacheRoom;
                
                Debug.Log("Prop setup");
            }
        }
    }

    private void Update()
    {
        if (_gameOver)
            return;
        
		if (TimeElapsing)
			GameDuration -= Time.deltaTime;
        if (GameDuration <= 0)
        {
            OnLose();
            return;
        }

        var span = TimeSpan.FromSeconds(GameDuration);
        TimerText.text = string.Format("{0}:{1:00}",
            (int) span.TotalMinutes,
            span.Seconds);
    }

    private void OnLose()
    {
        _gameOver = true;
        PlayerController.DeactivateControls();
        StartCoroutine(LoseCoroutine());
		AudioManager.Instance.PlaySFX (GameOverClip);
    }
    
    private IEnumerator LoseCoroutine()
    {
        TimerText.rectTransform.DOScale(1.5f, 0.25f);
        yield return new WaitForSeconds(.25f);
        
        TimerText.rectTransform.DOScale(1f, 0.25f);
        yield return new WaitForSeconds(.25f);
        
        DisplayGameOverUI(false);
    }
    
    private void OnWin()
    {
        PlayerController.DeactivateControls();
        StartCoroutine(WinCoroutine());
		AudioManager.Instance.PlaySFX (VictoryClip, 0.4f);
	}

	private IEnumerator WinCoroutine()
    {
        PropCountText.rectTransform.DOScale(1.5f, 0.25f);
        yield return new WaitForSeconds(.5f);
        
        PropCountText.rectTransform.DOScale(1f, 0.25f);
        yield return new WaitForSeconds(.5f);
        
        DisplayGameOverUI(true);
    }

    private void DisplayGameOverUI(bool win)
    {
        GameOverUI.SetActive(true);
        var canvasGroup = GameOverUI.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, 1f);
        var gameOverUi = GameOverUI.GetComponent<GameOverUI>();
        gameOverUi.WinImage.SetActive(win);
        gameOverUi.GameOverImage.SetActive(!win);
    }

    #region public methods

    public void SpawnMiniGame(Prop caughtProp)
    {
        var spawnRandomMiniGame = _miniGameManager.SpawnRandomMiniGame();
        // todo warn, ok only if miniGame is spawn at runtime
        spawnRandomMiniGame.OnLoseEvent += () => OnLoseMiniGame(caughtProp);
        spawnRandomMiniGame.OnWinEvent += () => OnWinMiniGame(caughtProp);
    }

    #endregion

    #region private methodes

    private void OnWinMiniGame(Prop caughtProp)
    {
        caughtProp.OnDead();

        _propCount++;
        UpdatePropCountUI();
        if (_propCount >= _totalPropCount)
        {
            OnWin();
        }
		else
		{
			AudioManager.Instance.PlaySFX (WinMiniGameClip);
		}
    }

    private void OnLoseMiniGame(Prop caughtProp)
    {
        caughtProp.SetTransition(Transition.MiniGameLost);
		AudioManager.Instance.PlaySFX (LoseMiniGameClip);
	}

	private void UpdatePropCountUI()
    {
        PropCountText.text = _propCount + " / " + _totalPropCount;
    }

    #endregion
}