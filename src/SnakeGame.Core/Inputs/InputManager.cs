namespace SnakeGame.Core.Inputs;

public class InputManager
{
    public InputBindingManager Bindings { get; }
    public KeyboardInputHandler Keyboard { get; } = new();
    public MouseInputHandler Mouse { get; } = new();
    public TouchInputHandler Touch { get; } = new();
    public GamePadInputHandler GamePad { get; } = new();

    public InputManager()
    {
        Bindings = new(Keyboard, Mouse);
    }

    public void Update()
    {
        Keyboard.Update();
        Mouse.Update();
        Touch.Update();
        GamePad.Update();
        Bindings.Update();
    }
}