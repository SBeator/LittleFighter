using UnityEngine;
using System.Collections;

public class PlayerController : Character
{
	// Update is called once per frame
	protected override void Update ()
	{
		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		
//		if (Input.GetKeyDown(KeyCode.Space)) 
//		{
//			this.Hit();
//		}
//		
//		var horizantial = Input.GetAxis("Horizontal");
//		var vertical = Input.GetAxis("Vertical");
//		this.Move(horizantial, vertical);
		
		#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
		
		//		var horizantial = Input.GetAxis("Horizontal");
		//		var vertical = Input.GetAxis("Vertical");
		//		this.character.Move(horizantial, vertical);
		
		#endif
		base.Update();
	}

	public override bool CanTarget(Character target)
	{
		if (target.tag == "Enemy") 
		{
			return true;
		}

		return false;
	}

	protected override void AddItself()
	{
		GameManager.instance.AddPlayer(this);
	}

	protected override void RemoveItself()
	{
		GameManager.instance.RemovePlayer(this);
	}
}
