namespace SnakeGame.Core.ECS.Components;

public enum SoundEffectTypes
{
    Pickup,
    Hit,
    GameEnd,
    Timer,
    SpeedUp,
    Click,
    Turn,
    EnemyHit,
    PowerUp,
    AddTime
}

public class SoundEffectComponent
{
    public SoundEffectTypes Type { get; set; }
}