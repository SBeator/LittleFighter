using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HitManager : MonoBehaviour 
{
	public IList<Character> HitCharacters
	{
		get
		{
			return this.hitCharacters;
		}
	}

	Character character;

	IList<Character> hitCharacters;

	void Awake()
	{
		this.hitCharacters = new List<Character>();
	}

	void Start()
	{
		this.character = this.GetComponentInParent<Character>();
	}

	void Update()
	{
		int index = 0;
		while (index < this.hitCharacters.Count) 
		{
			var hitCharacter = this.hitCharacters[index];

			if (hitCharacter == null || hitCharacter.Dead) 
			{
				this.hitCharacters.RemoveAt(index);
			}
			else 
			{
				index++;	
			}
		}
	}


	void OnTriggerEnter2D (Collider2D other) 
	{
		var otherCharacter = other.gameObject.GetComponent<Character>();
		if (otherCharacter != null && this.character.CanTarget(otherCharacter)) 
		{
			this.hitCharacters.Add(otherCharacter);
		}
	}
	
	void OnTriggerExit2D (Collider2D other) 
	{
		this.hitCharacters.Remove(other.gameObject.GetComponent<Character>());
	}
}
