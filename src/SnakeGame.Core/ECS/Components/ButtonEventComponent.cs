namespace SnakeGame.Core.ECS.Components;

public enum ButtonEvents
{
    Resume,
    Exit,
    ShowScoreBoard,
    StartNew,
    ShowCredits
}

public class ButtonEventComponent
{
    public ButtonEvents Event { get; set; }
}