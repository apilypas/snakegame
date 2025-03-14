using Microsoft.Xna.Framework.Input;

namespace SnakeGame.Core.Inputs;

public class KeyboardInputHandler
{
    private KeyboardState _previousState = Keyboard.GetState();
    private KeyboardState _currentState = Keyboard.GetState();
    
    public void Update()
    {
        _previousState = _currentState;
        _currentState = Keyboard.GetState();
    }
    
    public bool GetIsKeyDown(Keys key)
    {
        return _currentState.IsKeyDown(key);
    }

    public bool GetIsKeyPressed(Keys key)
    {
        return _currentState.IsKeyDown(key) && _previousState.IsKeyUp(key);
    }

    public bool GetIsKeyReleased(Keys key)
    {
        return _currentState.IsKeyUp(key) && _previousState.IsKeyDown(key);
    }
}