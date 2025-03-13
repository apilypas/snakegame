using Microsoft.Xna.Framework.Input;

namespace SnakeGame.Core.Inputs;

public class KeyboardInputManager
{
    private KeyboardState _previousState = Keyboard.GetState();
    private KeyboardState _currentState = Keyboard.GetState();
    
    public void Update()
    {
        _previousState = _currentState;
        _currentState = Keyboard.GetState();
    }
    
    public bool IsKeyDown(Keys key)
    {
        return _currentState.IsKeyDown(key);
    }

    public bool IsKeyPressed(Keys key)
    {
        return _currentState.IsKeyDown(key) && _previousState.IsKeyUp(key);
    }

    public bool IsKeyReleased(Keys key)
    {
        return _currentState.IsKeyUp(key) && _previousState.IsKeyDown(key);
    }
}