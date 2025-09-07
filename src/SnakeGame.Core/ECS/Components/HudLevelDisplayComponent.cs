namespace SnakeGame.Core.ECS.Components;

public class HudLevelDisplayComponent
{
    public string Level { get; set; } = string.Empty;
    public float Progress { get; set; }
}