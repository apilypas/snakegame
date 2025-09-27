namespace SnakeGame.Core.ECS.Components;

public enum ButtonEvents
{
    Resume,
    Close,
    Exit,
    ShowScoreBoard,
    StartNew,
    ShowCredits,
    ShowStartScreen,
    AddTime,
    AddInvincibility,
    DestroyEnemies,
    AddDiamondSpawnRate,
    AddScoreMultiplicator,
    AddDiamonds
}

public class ButtonEventComponent
{
    public int DialogEntityId { get; set; }
    public ButtonEvents Event { get; set; }
}