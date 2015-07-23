using System.Collections.Generic;

public class GameConfig
{
	public PlayersConfig players;
	public StageLevelConfig stageLevelConfig;
}

public class PlayersConfig 
{
	public List<PlayerConfig> players;
}

public class PlayerConfig 
{
	public string name;
}

public class StageLevelConfig
{
	public List<StageConfig> stages;
}

public class StageConfig 
{
	public List<LevelConfig> levels;
}

public class LevelConfig 
{
	public List<EnemyWaveConfig> enemyWaves;
}

public class EnemyWaveConfig 
{
	public List<EnemyCountConfig> enemyCountMap;
}

public class EnemyCountConfig
{
	public string name;
	public int count;
}

public class CharactorConfig
{
	public string name;
}