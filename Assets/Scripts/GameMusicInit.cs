using UnityEngine;

public class GameMusicInit : MonoBehaviour
{
	[SerializeField] AudioClip _gameMusic = null;

	public void PlayGameMusic ()
	{
		AudioManager.Instance.PlayMusicWithCrossFade (_gameMusic, 0.7f);   
	}

    void Start()
    {
		PlayGameMusic ();
    }
}