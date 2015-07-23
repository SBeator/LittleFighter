using UnityEngine;
using System.Collections;

public class MovePointer : MonoBehaviour 
{
	public static MovePointer instance;

	LineRenderer line;
	GameObject target;
	
	void Awake()
	{
		if (instance == null) 
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(this.gameObject);
		}
		
		DontDestroyOnLoad(this.gameObject);
	}

	void Start()
	{
		this.line = this.GetComponentInChildren<LineRenderer>();
		this.target = GameObject.FindWithTag("PointTarget");
		Init();
	}

	void OnLevelWasLoaded(int index)
	{
		Init();
	}

	void Init()
	{
		Clear();
	}

	public static void Clear()
	{
		instance.SetVisible(false);
	}

	public static void DrawLine(Character from, Vector2 targetPosition)
	{
		instance.SetVisible(true);

		instance.target.transform.position = targetPosition;

		instance.line.SetPosition(0, from.GetFootPosition());
		instance.line.SetPosition(1, targetPosition);
	}

	public static void DrawLine(Character from, Character target)
	{
		instance.target.SetActive(false);
		instance.line.enabled = true;
		
		instance.line.SetPosition(0, from.GetFootPosition());
		instance.line.SetPosition(1, target.GetFootPosition());
	}

	private void SetVisible(bool visible)
	{
		if (this.target != null && this.line != null) 
		{
			this.target.SetActive(visible);
			this.line.enabled = visible;
		}
	}
}
