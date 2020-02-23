using System;
using UnityEngine;

public class HidingPropsCounter : MonoBehaviour
{
	public static event Action PropSeen;
	public static event Action AllPropsHidden;

	public int _hidingPropsCount;

	public AudioClip baseGameMusic;
	public AudioClip chaseMusic;

	void PropSawByPlayer ()
	{
		_hidingPropsCount++;
		if (_hidingPropsCount == 1)
			AudioManager.Instance.PlayMusicWithCrossFade (chaseMusic, 0.3f);
	}

	void HideoutReached ()
	{
		_hidingPropsCount--;
		if (_hidingPropsCount == 0)
			AudioManager.Instance.PlayMusicWithCrossFade (baseGameMusic, 0.3f);
	}

	void Awake ()
	{
		Prop.Revealed += PropSawByPlayer;
		MiniGameUI.GameWon += HideoutReached;
		StateHiding.HideoutReached += HideoutReached;
	}
}