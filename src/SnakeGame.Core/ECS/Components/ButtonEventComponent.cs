namespace SnakeGame.Core.ECS.Components;

public enum ButtonEvents
{
    Resume,
    Close,
    Exit,
    ShowScoreBoard,
    StartNew,
    ShowCredits,
    ShowStartScreen
}

public class ButtonEventComponent
{
    public int DialogEntityId { get; set; }
    public ButtonEvents Event { get; set; }
}