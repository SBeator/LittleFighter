using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

public class MenuInWelcomePage : MonoBehaviour 
{
	public AudioClip bgmAudio;
	public AudioClip clickAudio;

	public void StartFight()
	{
		AudioManager.StopPlayBgm();
		AudioManager.Play(clickAudio);
		Application.LoadLevel ("MainGame");
	}

	public void Quit()
	{
		AudioManager.Play(clickAudio);
		Application.Quit();
	}

	void Start()
	{
		AudioManager.PlayBgm(this.bgmAudio);

		var config = new GameConfig
		{
			players = new PlayersConfig
			{
				players = new List<PlayerConfig>
				{
					new PlayerConfig
					{
						name = "David",
					},
					new PlayerConfig
					{
						name = "Steven"
					}
				}
			},
			stageLevelConfig = new StageLevelConfig
			{
				stages = new List<StageConfig>
				{
					new StageConfig
					{
						levels = new List<LevelConfig>
						{
							new LevelConfig
							{
								enemyWaves = new List<EnemyWaveConfig>
								{
									new EnemyWaveConfig
									{
										enemyCountMap = new List<EnemyCountConfig>
										{
											new EnemyCountConfig
											{
												name = "Bandit", 
												count = 1
											}
										}
									}
								}
							}

						}
					}
				}
			}
		};

		Stream fStream = new FileStream(@"Configs\xmlText.xml", FileMode.Open, FileAccess.ReadWrite);

		XmlSerializer xmlFormat = new XmlSerializer(typeof(GameConfig));
		//xmlFormat.Serialize(fStream, config);
		var configtest = xmlFormat.Deserialize(fStream);
		configtest.GetType();

	}
}
