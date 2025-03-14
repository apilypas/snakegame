using Microsoft.Xna.Framework;

namespace SnakeGame.Core.Inputs;

public class InputManager
{
    public InputBindingManager Bindings { get; }
    public KeyboardInputManager Keyboard { get; }
    public MouseInputManager Mouse { get; }
    public TouchInputManager Touch { get; }
    public GamePadManager GamePad { get; }
    
    public InputManager()
    {
        Keyboard = new KeyboardInputManager();
        Mouse = new MouseInputManager();
        Touch = new TouchInputManager();
        GamePad = new GamePadManager();
        Bindings = new InputBindingManager(Keyboard, Mouse);
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