using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum FaceTo
{
	None,
	Right,
	Left
}

public abstract class Character : MonoBehaviour 
{
	public float playgroundHalfWidth = 8.5f;
	public static float playerFootBodyDistance = 0.8f;
	public float hightLightDelay = 0.3f;
	public float playerSize = 1f;
	public int maxHealth = 10;
	public int damage = 1;

	public float speed = 3f;
	public float hitDelay = 0.8f;
	public float hitTime = 0.4f;

	public AudioClip[] hitClips;
	public AudioClip[] missHitClips;

	public PlayerInfo playerInfo;

	public bool Dead
	{
		get
		{
			return this.dead;
		}
	}

	public IList<Character> HitCaracters
	{
		get
		{
			return this.hitManager.HitCharacters;
		}
	}
	
	public IList<Character> BackHitCaracters
	{
		get
		{
			return this.backHitManager.HitCharacters;
		}
	}

	public Character Target
	{
		get
		{
			return this.autoHit != null ? this.autoHit.hitTarget : null;
		}

		set
		{
			if (this.autoHit != null) 
			{
				this.autoHit.hitTarget = value;
			}
		}
	}
	
	public string Name
	{
		get
		{
			return this.name.Replace("(Clone)", "");
		}
	}

	[HideInInspector] public bool canHit = true;
	[HideInInspector] public bool moving = false;
	[HideInInspector] public int health;

	bool hitting = false;

	float lastHitTime = 0f;
	Animator anim;
	Vector3 movingToPosition;
	Vector3 rightScale;
	Vector3 leftScale;
	HitManager hitManager;
	HitManager backHitManager;
	bool dead = false;

	bool isHightLighted;
	float hightLightStartTime;
	float hightLightRefreshTime;
	SpriteRenderer spriteRanderer;

	AutoHit autoHit;
	FaceTo forceFaceTo;
	FaceTo faceTo;
	Vector3 targetRightPositionOffset = new Vector3(1, 0, 0);
	bool forceMoving;
	Vector3 forceMovePosition = Vector3.zero;
	
	protected virtual IList<string> HitMap 
	{
		get
		{
			return new List<string>
			{
				"Hit1",
				"Hit2"
			};
		}
	}

	void Awake()
	{
		this.movingToPosition = this.transform.position;
	}

	protected virtual void Start()
	{
		this.forceFaceTo = FaceTo.None;
		this.faceTo = FaceTo.None;
		this.autoHit = this.GetComponent<AutoHit>();
		this.spriteRanderer = this.GetComponent<SpriteRenderer>();
		this.anim = this.GetComponent<Animator>();
		this.rightScale = this.transform.localScale;
		this.leftScale = new Vector3(-this.rightScale.x, this.rightScale.y, this.rightScale.z);
		this.health = maxHealth;

		var hitManagers = this.GetComponentsInChildren<HitManager>();
		foreach (var item in hitManagers) 
		{
			if (item.tag == "HitColider") 
			{
				this.hitManager = item;
				continue;
			}

			if (item.tag == "BackHitColider") 
			{
				this.backHitManager = item;
				continue;
			}
		}

		AddItself();
	}

	protected virtual void Update()
	{
		if (this.dead) 
		{
			return;
		}

		var hittedTime = Time.time - this.lastHitTime;

		if (this.hitting) 
		{
			if (hittedTime > this.hitTime) 
			{
				this.hitting = false;
			}
			else if (!this.forceMoving)
			{
				this.anim.SetBool ("Moving", false);
				this.moving = false;	
			}
		}

		if (!this.canHit && hittedTime > this.hitDelay) 
		{
			this.canHit = true;
		}

		if (this.forceMoving || !this.hitting)
		{
			HandleMove ();
		}

		if (this.isHightLighted) 
		{
			if (Time.time - this.hightLightRefreshTime < this.hightLightDelay) 
			{
				var highLightedTime = (Time.time - this.hightLightStartTime) % this.hightLightDelay;
				var grey = Mathf.Abs(-0.6f / this.hightLightDelay * highLightedTime + 1);
				this.spriteRanderer.color = new Color(grey, grey, grey);
			}
			else 
			{
				this.spriteRanderer.color = new Color(1, 1, 1);
				this.isHightLighted = false;
			}
		}

		this.transform.rotation = Quaternion.identity;
	}


	public void AddPlayerInfo(PlayerInfo playerInfo)
	{
		this.playerInfo = playerInfo;
		this.playerInfo.PlayerImage = this.GetComponent<SpriteRenderer>().sprite;
		this.playerInfo.PlayerName = this.name.Replace("(Clone)", "");
	}

	public static Vector2 GetFootFromBodyPosition(Vector2 position)
	{
		return new Vector2(position.x, position.y - playerFootBodyDistance);
	}
	
	public static Vector2 GetBodyFromFootPosition(Vector2 position)
	{
		return new Vector2(position.x, position.y + playerFootBodyDistance);
	}

	public Vector2 GetFootPosition()
	{
		return GetFootFromBodyPosition(this.transform.position);
	}

	public bool Pointed(Vector2 pointer)
	{
		return Vector2.Distance(pointer, this.transform.position) < this.playerSize;
	}

	public void HightLight()
	{
		if (!this.isHightLighted) 
		{
			this.hightLightStartTime = Time.time;
		}

		this.isHightLighted = true;
		this.hightLightRefreshTime = Time.time;
	}

	public void Hit()
	{
		if (this.dead || !this.canHit || this.forceMoving)
		{
			return;
		}
		
		this.anim.SetTrigger(HitMap[Random.Range(0, HitMap.Count)]);

		Invoke("Attack", this.hitTime / 2);
		
		this.lastHitTime = Time.time;
		this.hitting = true;
		this.canHit = false;
	}

	public void TurnBack()
	{
		if (this.faceTo == FaceTo.Right) 
		{
			this.transform.localScale = this.leftScale;
			this.faceTo = FaceTo.Left;
		}
		else
		{
			this.transform.localScale = this.rightScale;
			this.faceTo = FaceTo.Right;
		}
	}
	
	public void Move(float dX, float dY)
	{
		this.MoveTo(this.transform.position.x + dX, this.transform.position.y + dY);
	}

	public void MoveTo(float x, float y)
	{
		if (this.dead) 
		{
			return;
		}
		this.SetMovePosition(new Vector3(x, y, 0));
	}

	public void ForceMoveTo(float x, float y)
	{
		this.forceMoving = true;
		this.forceMovePosition.Set(x, y, 0);

		this.MoveTo(x, y);
	}

	public void ForceTarget(Character target)
	{
		this.Target = target;
		this.forceMoving = false;
	}

	public void MoveToTarget()
	{
		if (this.dead || this.Target == null) 
		{
			this.forceFaceTo = FaceTo.None;
			return;
		}

		var targetPosition = this.Target.transform.position;
		var thisPosition = this.transform.position;

		if (thisPosition.x < targetPosition.x) 
		{
			this.forceFaceTo = FaceTo.Right;
			this.SetMovePosition(targetPosition - targetRightPositionOffset);
			
		}
		else 
		{
			this.forceFaceTo = FaceTo.Left;
			this.SetMovePosition(targetPosition + targetRightPositionOffset);
		}
	}

	public void MoveToRight()
	{
		var thisPosition = this.transform.position;
		this.SetMovePosition(thisPosition + new Vector3(playgroundHalfWidth * 2, 0, 0));
		this.forceFaceTo = FaceTo.Right;
	}

	public void StopMove()
	{
		this.movingToPosition = this.transform.position;
	}

	public bool LossHealth(int damage)
	{
		this.health -= damage;
		if (this.playerInfo != null) 
		{
			this.playerInfo.BearingValue += damage;	
		}

		if (health <= 0) 
		{
			this.Die();
			return true;
		}

		return false;
	}

	public virtual void Die()
	{
		var colliders = this.GetComponents<Collider2D>();

		foreach (var collider in colliders) 
		{
			collider.enabled = false;
		}

		this.anim.SetTrigger("Die");
		this.dead = true;
		RemoveItself();
		
		Destroy (this.gameObject, 3f);
	}

	public virtual bool CanTarget(Character target)
	{
		return true;
	}
	
	protected virtual void Attack()
	{
		if (this.Dead) 
		{
			return;
		}

		foreach (var character in this.HitCaracters) 
		{
			var dead = character.LossHealth(this.damage);
			if (this.playerInfo != null) 
			{
				this.playerInfo.AttackValue += this.damage;
			}

			if (dead) 
			{
				if (this.playerInfo != null) 
				{
					this.playerInfo.KillValue ++;
				}
			}
		}

		if (this.HitCaracters.Count > 0) 
		{
			AudioManager.instance.PlayRandom (this.hitClips);
		}
		else
		{
			AudioManager.instance.PlayRandom (this.missHitClips);
		}
	}

	protected abstract void RemoveItself();
	protected abstract void AddItself();
	
	void HandleMove ()
	{
		var movingVector = this.movingToPosition - this.transform.position;
		if (movingVector.magnitude > float.Epsilon) 
		{
			var newPostion = Vector3.MoveTowards (this.transform.position, this.movingToPosition, this.speed * Time.deltaTime);
			this.transform.position = newPostion;
			var forceFace = !(this.forceMoving || this.forceFaceTo == FaceTo.None);
			if ((!forceFace && movingVector.x > 0 && this.faceTo != FaceTo.Right) ||
				(!forceFace && movingVector.x < 0 && this.faceTo != FaceTo.Left) ||
				(forceFace && this.forceFaceTo != this.faceTo))
			{
				this.TurnBack();
			}

			
			this.anim.SetBool ("Moving", true);
			this.moving = true;
		}
		else 
		{
			this.anim.SetBool ("Moving", false);
		}
		
		if (movingVector.magnitude < this.playerSize) 
		{
			this.moving = false;
		}

		if (this.forceMoving == true && (this.forceMovePosition - this.transform.position).magnitude < float.Epsilon) 
		{
			this.forceMoving = false;
		}
	}

	void SetMovePosition(Vector3 position)
	{
		if (this.forceMoving) 
		{
			this.movingToPosition = this.forceMovePosition;
		}
		else 
		{
			this.movingToPosition = position;	
		}
	}
}
