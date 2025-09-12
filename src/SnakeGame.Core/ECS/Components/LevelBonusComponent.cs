namespace SnakeGame.Core.ECS.Components;

public class LevelBonusComponent
{
    public enum LevelBonusType
    {
        AddTime,
        AddInvincibility,
        DestroyEnemies,
        AddDiamondSpawnRate,
        AddScoreMultiplicator
    }
    
    public LevelBonusType Type { get; set; }
}