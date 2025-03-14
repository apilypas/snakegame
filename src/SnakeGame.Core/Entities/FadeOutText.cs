namespace SnakeGame.Core.Entities;

public class FadeOutText : EntityBase
{
    public string Text { get; set; } = string.Empty;
    public float TimeToLive { get; set; } = 1f;
}