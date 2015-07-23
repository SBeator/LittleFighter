using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StageMessage : MonoBehaviour 
{
	public static StageMessage instance;

	Text text;

	void Awake()
	{
		if (instance != null) 
		{
			Destroy(instance);
		}

		instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		this.text = this.GetComponentInChildren<Text>();
		this.text.text = "Stage " + GameManager.instance.stage + "-" + GameManager.instance.level;
	}
}
