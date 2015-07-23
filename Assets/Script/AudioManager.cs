using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{
	public static AudioManager instance;
	public static AudioClip bgm;
	
	public AudioSource bgmSource;
	public AudioSource soundSource;

	public float lowPitchRange = .9f;				
	public float highPitchRange = 1.1f;			

	void Awake () 
	{
		if (instance == null) 
		{
			instance = this;
		}
		else
		{
			Destroy(this.gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

	public static void StopPlayBgm()
	{
		if (instance != null) 
		{
			instance.bgmSource.Stop();
		}
	}

	public static void PlayBgm(AudioClip clip)
	{
		if (bgm == null || bgm.name != clip.name) 
		{
			bgm = Instantiate(clip);
			bgm.name = clip.name;

			if (instance != null) 
			{
				instance.PlayBgmClip(clip);
			}
		}

	}

	public static void Play(AudioClip clip)
	{
		instance.soundSource.Stop();
		instance.soundSource.clip = clip;
		instance.soundSource.Play();
	}

	public void PlayRandom(AudioClip[] clips)
	{
		PlayRandom(clips[Random.Range(0, clips.Length)]);
	}

	public void PlayRandom(AudioClip clip)
	{
		soundSource.Stop();
		soundSource.pitch = Random.Range(this.lowPitchRange, this.highPitchRange);;
		soundSource.clip = clip;
		soundSource.Play();
	}
	
	private void PlayBgmClip(AudioClip clip)
	{
		bgmSource.Stop();
		bgmSource.clip = clip;
		bgmSource.Play();
	}
}
