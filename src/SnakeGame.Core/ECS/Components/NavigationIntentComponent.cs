namespace SnakeGame.Core.ECS.Components;

public enum NavigationEvent
{
    None,
    FocusNext,
    FocusPrevious,
    Select
}

public class NavigationIntentComponent
{
    public NavigationEvent Event { get; set; }
}