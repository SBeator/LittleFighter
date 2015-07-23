using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugMessage : MonoBehaviour 
{
	static DebugMessage instance;

	Text text;

	void Awake()
	{
		if (instance == null) 
		{
			instance = this;
		}
	}

	void Start()
	{
		this.text = this.GetComponent<Text>();
	}

	public static void log(string log)
	{
		instance.text.text = log;
	}
}
