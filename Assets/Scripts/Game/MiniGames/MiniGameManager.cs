﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public List<MiniGameUI> MiniGames = new List<MiniGameUI>();
    private List<MiniGameUI> _usedMiniGames = new List<MiniGameUI>();

	public AudioClip MiniGameSpawnClip;
	public AudioClip MiniGameMusic;

    public MiniGameUI SpawnRandomMiniGame()
    {
        if (_usedMiniGames.Count >= MiniGames.Count)
            _usedMiniGames.Clear();
        var availableMiniGames = MiniGames.Where(m => !_usedMiniGames.Contains(m)).ToList();
        var pickedMiniGame = availableMiniGames[Random.Range(0, availableMiniGames.Count())];
        _usedMiniGames.Add(pickedMiniGame);

		GameController.Instance.TimeElapsing = false;
		AudioManager.Instance.PlaySFX (MiniGameSpawnClip);
		AudioManager.Instance.PlayMusicWithCrossFade (MiniGameMusic, 0.7f);
	
		return Instantiate (pickedMiniGame);
    }
}
