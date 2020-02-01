using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

[RequireComponent(typeof(MiniGameManager))]
public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("Player")] public PlayerController PlayerController;
	[Header ("A* Mesh")] public AStarMesh AStarMesh;

    // todo maybe put this in a specific UI class
    [Header("UI")] public Text PropCountText;
    
    private MiniGameManager _miniGameManager;
    private int _propCount;
    private int _totalPropCount = 10;

	private void Awake ()
	{
		if (Instance != null)
		{
			DestroyImmediate (gameObject);
			return;
		}

		Instance = this;
		//DontDestroyOnLoad (gameObject);
	}

	void Start()
    {
        _miniGameManager = GetComponent<MiniGameManager>();
        
        // todo after propCount set
        UpdatePropCountUI();
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
    }

    private void OnLoseMiniGame(Prop caughtProp)
    {
        caughtProp.SetTransition(Transition.MiniGameLost);
    }

    private void UpdatePropCountUI()
    {
        PropCountText.text = _propCount + " / " + _totalPropCount;
    }

    #endregion
    
}
