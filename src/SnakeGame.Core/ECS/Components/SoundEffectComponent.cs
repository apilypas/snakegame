namespace SnakeGame.Core.ECS.Components;

public enum SoundEffectTypes
{
    Pickup,
    PlayerDied,
    GameEnded,
    TimerChanged,
    SpeedUp
}

public class SoundEffectComponent
{
    public SoundEffectTypes Type { get; set; }
}