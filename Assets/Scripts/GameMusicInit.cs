using UnityEngine;

public class GameMusicInit : MonoBehaviour
{
	[SerializeField] AudioClip _gameMusic = null;
	[SerializeField] AudioClip _chaseMusic = null;

	public void PlayChaseMusic ()
	{
		AudioManager.Instance.PlayMusicWithCrossFade (_chaseMusic, 0.3f);
	}

	public void PlayGameMusic ()
	{
		AudioManager.Instance.PlayMusicWithCrossFade (_gameMusic, 0.3f);   
	}

    void Start ()
    {
		PlayGameMusic ();
    }
}