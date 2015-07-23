using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

	public GameManager gameManager;
	public AudioManager audioManager;
	public MovePointer movePointer;
	
	void Awake()
	{
		if (GameManager.instance == null)
		{
			Instantiate(this.gameManager);
		}

		if (AudioManager.instance == null)
		{
			Instantiate(this.audioManager);
		}

		if (MovePointer.instance == null)
		{
			Instantiate(this.movePointer);
		}
	}
}
