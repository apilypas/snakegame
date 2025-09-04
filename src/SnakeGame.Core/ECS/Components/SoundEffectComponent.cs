namespace SnakeGame.Core.ECS.Components;

public enum SoundEffectTypes
{
    ScoreChanged,
    PlayerDied,
    GameEnded,
    TimerChanged
}

public class SoundEffectComponent
{
    public SoundEffectTypes Type { get; set; }
}