namespace SnakeGame.Core.ECS.Components;

public class LevelBonusComponent
{
    public enum LevelBonusType
    {
        AddTime,
        AddInvincibility,
        DestroyEnemies
    }
    
    public LevelBonusType Type { get; set; }
}