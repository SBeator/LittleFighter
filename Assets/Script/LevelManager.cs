using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour 
{
	public GameObject[] possiblePlayers;
	public GameObject[] possibleEnemies;

	public float playgroundHalfWidth = 8.5f;
	public float playgroundHalfHeight = 3.5f;
	public float firstEnemyDelay = 3f;

	bool created = false;

	int enemyToBeCreatedNumber;

	public void InitializeLevel(int level)
	{
		this.enemyToBeCreatedNumber = 0;
		this.CreatePlayers(level);
		this.CreateEnemies(level);
		this.created = true;
	}

	public bool IsEnemyDone()
	{
		return this.enemyToBeCreatedNumber <= 0;
	}

	void Update()
	{
		if (this.created && this.enemyToBeCreatedNumber <= 0) 
		{
			GameManager.instance.gameCanOver = true;

			this.created = false;
		}
	}

	void CreatePlayers(int level)
	{
		for (int i = 0; i < this.possiblePlayers.Length; i++) 
		{
			var player = CreateCharacter(this.possiblePlayers[i], new Vector2(-playgroundHalfWidth - 2 + i * 2, 0));
			player.MoveTo(i * 2, 0);
		}
	}

	void CreateEnemies(int level)
	{
		for (int i = 0; i < level; i++) 
		{
			Invoke("CreateRandomEnemie", this.firstEnemyDelay);
			this.enemyToBeCreatedNumber++;
		}
	}

	void CreateRandomEnemie()
	{
		CreateRandomCharacters(possibleEnemies, this.RandomOutPosition());
		this.enemyToBeCreatedNumber--;
	}

	Character CreateRandomCharacters(GameObject[] gameObject, Vector3 position)
	{
		return this.CreateCharacter(gameObject[Random.Range(0, gameObject.Length - 1)], position);
	}

	Character CreateCharacter(GameObject gameObject, Vector3 position)
	{
		var clone = Instantiate(gameObject, position, Quaternion.identity) as GameObject;
		return clone.GetComponent<Character>();
	}

	Vector2 RandomInPosition()
	{
		float x = Random.Range(-playgroundHalfWidth, playgroundHalfWidth);
		float y = Random.Range(-playgroundHalfHeight, playgroundHalfHeight);
		
		return new Vector2(x, y);
	}

	Vector2 RandomOutPosition()
	{
		float x = Random.Range(0, 2) > 0 ? -playgroundHalfWidth : playgroundHalfWidth;
		float y = Random.Range(-playgroundHalfHeight, playgroundHalfHeight);

		return new Vector2(x, y);
	}



}
