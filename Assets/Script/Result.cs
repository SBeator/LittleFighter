using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Result : MonoBehaviour 
{
	public static Result instance;

	public float canClickDelay = 2f;
	public AudioClip passAudio;
	public AudioClip failureAudio;
	public AudioClip clickAudio;

	bool canClick = false;
	ResultRow[] resultRows;
	
	void Awake()
	{
		if (instance != null) 
		{
			Destroy(instance);
		}
		
		instance = this;
	}

	void Start()
	{
		this.resultRows = this.GetComponentsInChildren<ResultRow>(true);
		this.gameObject.SetActive(false);
	}

	public static void ShowResult(IList<PlayerInfo> playerInfos, bool pass)
	{
		AudioManager.PlayBgm(pass ? instance.passAudio : instance.failureAudio);

		for (int i = 0; i < playerInfos.Count && i < instance.resultRows.Length; i++) 
		{
			instance.resultRows[i].ShowPlayerInfo(playerInfos[i]);
		}

		instance.Invoke("makeThisCanClick", instance.canClickDelay);

		instance.gameObject.SetActive(true);
	}

	public void ResetGame()
	{
		if (this.canClick) 
		{
			GameManager.instance.BackToMenu();
			AudioManager.Play(this.clickAudio);
		}
	}

	void makeThisCanClick()
	{
		this.canClick = true;
	}
}
