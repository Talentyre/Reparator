using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public List<MiniGameUI> MiniGames = new List<MiniGameUI>();
    private List<MiniGameUI> _usedMiniGames = new List<MiniGameUI>();
    

    public MiniGameUI SpawnRandomMiniGame()
    {
        if (_usedMiniGames.Count >= MiniGames.Count)
            _usedMiniGames.Clear();
        var availableMiniGames = MiniGames.Where(m => !_usedMiniGames.Contains(m)).ToList();
        var pickedMiniGame = availableMiniGames[Random.Range(0, availableMiniGames.Count())];
        _usedMiniGames.Add(pickedMiniGame);
        
        return Instantiate(pickedMiniGame);
    }
}
