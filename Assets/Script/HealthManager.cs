using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour 
{
	Character character;
	SpriteRenderer[] healthBar;
	SpriteRenderer healthValue;

	void Start () 
	{
		this.character = this.GetComponentInParent<Character>();	
		if (this.character == null) 
		{
			Destroy(this.gameObject);
		}
		else
		{
			this.healthBar = this.GetComponentsInChildren<SpriteRenderer>();
			foreach (var item in healthBar) 
			{
				if (item.tag == "HealthValue") 
				{
					this.healthValue = item;
					break;
				}
			}
		}
	}

	void Update()
	{
		UpdateHealthBar();
	}

	void UpdateHealthBar()
	{
		var health = this.character.health;
		if (health < 0) 
		{
			this.character.health = 0;
			health = 0;
		}

		var maxHealth = this.character.maxHealth;
		if (health >= maxHealth) 
		{
			this.ShowHealthBar(false);
		}
		else
		{
			this.ShowHealthBar(true);
			this.SetHealthValue(health, maxHealth);
		}
	}

	void SetHealthValue(int health, int maxHealth)
	{
		var rate = ((float)health) / ((float)maxHealth);

		this.healthValue.transform.localScale = new Vector3(rate, this.healthValue.transform.localScale.y, this.healthValue.transform.localScale.z);
		// this.healthValue.transform.position = new Vector3(this.transform.position.x + (rate - 1f) / 2f, this.transform.position.y, this.transform.position.z);

		Color healthColor;
		if (rate > 0.5) 
		{
			healthColor = new Color((1f - rate) * 2, 1, 0);
		}
		else
		{
			healthColor = new Color(1, rate * 2, 0);
		}

		this.healthValue.color = healthColor;

	}

	void ShowHealthBar(bool show)
	{
		foreach (var item in this.healthBar) 
		{
			item.enabled = show;
		}
	}
}
