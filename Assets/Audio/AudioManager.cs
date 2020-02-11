using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance;

	AudioSource[] _audioSources;
	AudioSource _sfxSource;
	int _currentPlayingIndex;

	void Awake ()
	{
		if (Instance != null)
		{
			DestroyImmediate (gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad (gameObject);

		_audioSources = new AudioSource[2];
		_audioSources[0] = gameObject.AddComponent<AudioSource> ();
		_audioSources[1] = gameObject.AddComponent<AudioSource> ();
		_sfxSource = gameObject.AddComponent<AudioSource> ();

		foreach (var source in _audioSources)
			source.loop = true;
	}

	public void PlayMusic (AudioClip musicClip)
	{
		var activeSource = _audioSources[_currentPlayingIndex];

		activeSource.clip = musicClip;
		activeSource.volume = 1;
		activeSource.Play ();
	}

	public void PlayMusicWithFade (AudioClip newMusicClip, float transitionTime = 1)
	{
		var activeSource = _audioSources[_currentPlayingIndex];
		StartCoroutine (FadeMusic (activeSource, newMusicClip, transitionTime));
	}
	IEnumerator FadeMusic (AudioSource activeSource, AudioClip newClip, float transitionTime)
	{
		if (!activeSource.isPlaying)
			activeSource.Play ();

		for (float t = 0; t < transitionTime; t += Time.deltaTime)
		{
			activeSource.volume = 1 - t / transitionTime;
			yield return null;
		}

		activeSource.Stop ();
		activeSource.clip = newClip;
		activeSource.Play ();

		for (float t = 0; t < transitionTime; t += Time.deltaTime)
		{
			activeSource.volume = t / transitionTime;
			yield return null;
		}
	}

	public void PlayMusicWithCrossFade (AudioClip newMusicClip, float transitionTime = 1)
	{
		var activeSource = _audioSources[_currentPlayingIndex];
		var newSource = _audioSources[1 - _currentPlayingIndex];
		_currentPlayingIndex = 1 - _currentPlayingIndex;

		newSource.clip = newMusicClip;
		newSource.Play ();
		StartCoroutine (CrossFadeMusic (activeSource, newSource, transitionTime));
	}
	IEnumerator CrossFadeMusic (AudioSource previousSource, AudioSource newSource, float transitionTime)
	{
		for (float t = 0; t <= transitionTime; t += Time.deltaTime)
		{
			previousSource.volume = 1 - t / transitionTime;
			newSource.volume = t / transitionTime;
			yield return null;
		}

		previousSource.Stop ();
	}

	public void PlaySFX (AudioClip sfxClip)
	{
		_sfxSource.PlayOneShot (sfxClip);
	}
	public void PlaySFX (AudioClip sfxClip, float volume, float pitch = 1)
	{
		_sfxSource.pitch = pitch;
		_sfxSource.PlayOneShot (sfxClip, volume);
	}

	public void SetMusicVolume (float volume)
	{
		foreach (var source in _audioSources)
			source.volume = volume;
	}
	public void SetSFXVolume (float volume)
	{
		_sfxSource.volume = volume;
	}
}