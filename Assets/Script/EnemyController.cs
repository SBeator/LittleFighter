using UnityEngine;
using System.Collections;

public class EnemyController : Character 
{
	Character playerCharacter;
	
	protected override void Start()
	{
		this.SetTargetToPlayer ();

		base.Start();
	}

	protected override void Update ()
	{
		if (this.Target == null || this.Target.Dead) 
		{
			this.SetTargetToPlayer ();
		}

		base.Update();
	}

	void SetTargetToPlayer ()
	{
		var players = GameObject.FindGameObjectsWithTag ("Player");
		if (players != null) 
		{
			foreach (var player in players) 
			{
				var playerCharacter = player.GetComponent<Character> ();
				if (playerCharacter != null && !playerCharacter.Dead)
				{
					this.playerCharacter = playerCharacter;
					this.Target = this.playerCharacter;
					break;
				}
			}
		}
	}

	public override bool CanTarget(Character target)
	{
		if (target.tag == "Player") 
		{
			return true;
		}
		
		return false;
	}

	protected override void AddItself()
	{
		GameManager.instance.AddEnemy(this);
	}

	protected override void RemoveItself()
	{
		GameManager.instance.RemoveEnemy(this);
	}
}
