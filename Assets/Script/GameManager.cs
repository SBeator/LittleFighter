using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour 
{
	public float playgroundHalfWidth = 8.5f;
	public static GameManager instance;

	public float nextLevelDelay = 5f;
	public float playBgmDelay = 2f;
	public float startGameDelay = 4f;
	public AudioClip bgm;
	public int level;
	public int stage = 1;
	public bool gameCanOver;

	IList<PlayerInfo> playerInfos;
	
	IList<Character> players;
	IList<Character> enemies;

	IList<Character> characters;

	Character selectedCharacter;
	Character targetCharacter;
	bool touched;
	bool touchMoved;
	LevelManager levelManager;
	float startTime;
	int initializedLevel;

	bool gameIsOver;
	bool needRestart;

	void Awake()
	{
		if (instance == null) 
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		
		DontDestroyOnLoad(this.gameObject);
		
		this.players = new List<Character>();
		this.enemies = new List<Character>();
		this.playerInfos = new List<PlayerInfo>();
		this.level = 1;
		this.levelManager = this.GetComponent<LevelManager>();

		InitGame();
	}
	
	void InitGame()
	{
		if (Application.loadedLevelName == "MainGame" && this.initializedLevel != this.level) 
		{
			touched = false;
			touchMoved = false;
			this.gameIsOver = false;
			this.needRestart = false;
			this.gameCanOver = false;
			this.initializedLevel = this.level;

			Invoke("PlayBgm", this.playBgmDelay);
			Invoke("StartGame", this.startGameDelay);
		}
	}

	void PlayBgm()
	{
		AudioManager.PlayBgm(this.bgm);
	}

	void StartGame()
	{
		foreach (var player in this.players) 
		{
			Destroy (player);
		}
		
		foreach (var enemy in this.enemies) 
		{
			Destroy (enemy);
		}

		if (level == 1) 
		{
			this.playerInfos.Clear();
		}

		this.players.Clear();
		this.enemies.Clear();
		this.characters.Clear();
		
		this.levelManager.InitializeLevel(level);
	}

	void Start()
	{
		this.startTime = Time.time;
	}

	void Update()
	{
		if (!this.gameIsOver && this.gameCanOver) 
		{
			if (this.players.Count == 0) 
			{
				this.GameOver();
			}
			
			if (this.enemies.Count == 0 && this.levelManager.IsEnemyDone()) 
			{
				this.GameWin ();
			}
		}

		if (this.gameIsOver) 
		{
			if (this.needRestart) 
			{
				bool allPlayerOut = true;
				
				foreach (var player in this.players) 
				{
					if (player.GetFootPosition().x < this.playgroundHalfWidth) 
					{
						allPlayerOut = false;
						break;
					}
				}
				
				if (allPlayerOut) 
				{
					this.Restart();
				}
			}

			return;
		}

		this.characters = this.players.Concat(this.enemies).Where(x => x != null).OrderByDescending(c => c.transform.position.y).ToList();

		int order = 0;
		foreach (var orderItem in this.characters) 
		{
			orderItem.GetComponent<SpriteRenderer>().sortingOrder = order;
			order++;
		}
		
		if (Input.GetMouseButtonDown(0)) 
		{
			this.CheckIfTouched (Input.mousePosition);
		}
		else if(Input.GetMouseButtonUp(0))
		{
			this.CheckIfNeedMove (Input.mousePosition);
		}
		else
		{
			this.DrawLine (Input.mousePosition);
		}
		
		if (Input.touches.Length == 1) 
		{
			var touch = Input.GetTouch(0);
			switch (touch.phase) 
			{
			case TouchPhase.Began:
				this.CheckIfTouched (touch.position);
				break;
			case TouchPhase.Ended:
				this.CheckIfNeedMove (touch.position);
				break;
			default:
				break;
			}
		}

		if (this.touched) 
		{
			if (this.selectedCharacter != null) 
			{
				this.selectedCharacter.HightLight();
			}
			
			if (this.targetCharacter != null) 
			{
				this.targetCharacter.HightLight();
			}
		}
	}
	
	public void AddPlayer(Character character)
	{
		this.players.Add(character);

		var playerInfo = this.playerInfos.FirstOrDefault(x => x.PlayerName == character.Name);

		if (playerInfo == null) 
		{
			playerInfo = new PlayerInfo();
			this.playerInfos.Add(playerInfo);
		}

		character.AddPlayerInfo(playerInfo);
	}

	public void RemovePlayer(Character character)
	{
		this.players.Remove(character);
	}

	public void AddEnemy(Character character)
	{
		this.enemies.Add(character);
	}
	
	public void RemoveEnemy(Character character)
	{
		this.enemies.Remove(character);
	}

	public void NextLevel()
	{
		this.level++;

		foreach (var player in this.players) 
		{
			player.MoveToRight();
		}

		this.needRestart = true;
	}

	public void BackToMenu()
	{
		this.level = 1;
		this.initializedLevel = 0;
		Application.LoadLevel ("Welcome");
	}

	void OnLevelWasLoaded(int index)
	{
		InitGame();
	}

	void GameOver()
	{
		MovePointer.Clear();
		this.gameIsOver = true;

		Result.ShowResult(this.playerInfos, false);
	}

	void GameWin()
	{
		MovePointer.Clear();
		this.gameIsOver = true;
		NextLevelManager.ShowNextLevelMessage();
	}

	private void Restart()
	{
		Application.LoadLevel (Application.loadedLevel);
	}

	void CheckIfTouched(Vector2 touchPosition)
	{
		var touchPoint = Camera.main.ScreenToWorldPoint (new Vector3(touchPosition.x, touchPosition.y)) ;

		foreach (var character in this.players) 
		{
			if (character.Pointed(touchPoint)) 
			{
				this.selectedCharacter = character;
				this.touched = true;
				break;
			}
		}
	}
	
	void DrawLine(Vector2 touchPosition)
	{
		if (this.touched) 
		{
			var touchPoint = Camera.main.ScreenToWorldPoint (new Vector3(touchPosition.x, touchPosition.y)) ;

			if (this.selectedCharacter.Pointed (touchPoint)) 
			{
				touchMoved = false;
				MovePointer.Clear();
			} 
			else 
			{
				var touchedOther = false;
				this.targetCharacter = null;
				foreach (var character in characters) 
				{
					if (this.selectedCharacter.CanTarget(character) && character.Pointed(touchPoint)) 
					{
						this.targetCharacter = character;
						touchedOther = true;
						break;
					}
				}

				touchMoved = true;

				if (touchedOther) 
				{
					MovePointer.DrawLine (this.selectedCharacter, this.targetCharacter);
				}
				else
				{
					MovePointer.DrawLine (this.selectedCharacter, touchPoint);
				}
			}
		}
	}
	
	void CheckIfNeedMove(Vector2 touchPosition)
	{
		if (this.touched && touchMoved) 
		{

			if (this.targetCharacter != null) 
			{
				this.selectedCharacter.ForceTarget(this.targetCharacter);
			}
			else
			{
				this.selectedCharacter.Target = null;
				var touchPoint = Camera.main.ScreenToWorldPoint (new Vector3(touchPosition.x, touchPosition.y)) ;
				this.selectedCharacter.ForceMoveTo(touchPoint.x, touchPoint.y);
			}

			MovePointer.Clear();
		}
		
		this.touched = false;
		this.touchMoved = false;
	}
}
