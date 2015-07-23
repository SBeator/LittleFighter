using UnityEngine;
using System.Collections;

public class AutoHit : MonoBehaviour 
{
	public Character hitTarget;

	Character character;
	
	void Start()
	{
		this.character = this.GetComponent<Character>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (this.character.canHit) 
		{
			if (this.hitTarget == null) 
			{
				if (this.character.HitCaracters.Count > 0) 
				{
					this.character.Hit();
					this.hitTarget = this.character.HitCaracters[0];
				}
				else if(this.character.BackHitCaracters.Count > 0)
				{
					this.character.TurnBack();
					this.character.Hit();
					this.hitTarget = this.character.BackHitCaracters[0];
				}
			}
			else
			{
				if (this.character.HitCaracters.Contains(this.hitTarget)) 
				{
					this.character.Hit();
				}
			}
		}

		if (this.hitTarget != null && !this.hitTarget.Dead) 
		{
			this.character.MoveToTarget();
		}
	}
}
