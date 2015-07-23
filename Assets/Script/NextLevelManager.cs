using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NextLevelManager : MonoBehaviour {

	public static NextLevelManager instance;

	public AudioClip flickAudio;
	public float flickTime = 1f;

	Text text;
	bool stopFlick; 

	bool canClick = false;

	public static void ShowNextLevelMessage()
	{
		instance.StartCoroutine(instance.FlickText());
	}

	public void GoToNextLevel()
	{
		if (this.canClick) 
		{
			this.canClick = false;
			GameManager.instance.NextLevel();
			this.stopFlick = true;
		}
	}

	void Awake()
	{
		if (instance != null) 
		{
			Destroy(instance);
		}

		instance = this;
		this.canClick = false;
	}

	void Start () 
	{
		this.text = this.GetComponentInChildren<Text>();
		this.text.enabled = false;
		this.stopFlick = false;
	}

	IEnumerator FlickText()
	{
		yield return new WaitForSeconds(this.flickTime * 2);
		var showText = true;
		while(true)
		{
			this.text.enabled = showText;

			if (showText && this.flickAudio != null) 
			{
				AudioManager.Play(this.flickAudio);
			}

			showText = !showText;

			yield return new WaitForSeconds(this.flickTime / 2);

			if (this.stopFlick) 
			{
				break;
			}

			this.canClick = true;
		}

		this.text.enabled = false;
	}
}
