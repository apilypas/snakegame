using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SnakeGame.Core.Inputs;

public class MouseInputHandler
{
    private MouseState _currentState = Mouse.GetState();
    private MouseState _previousState = Mouse.GetState();
    
    public bool IsLeftButtonDown => _currentState.LeftButton == ButtonState.Pressed;
    public bool IsLeftButtonPressed => GetIsLeftButtonPressed();
    public bool IsLeftButtonReleased => GetIsLeftButtonReleased();
    public Vector2 Position => GetPosition();

    public void Update()
    {
        _previousState = _currentState;
        _currentState = Mouse.GetState();
    }

    private Vector2 GetPosition()
    {
        return new Vector2(_currentState.X, _currentState.Y);
    }

    private bool GetIsLeftButtonPressed()
    {
        return _previousState.LeftButton == ButtonState.Released 
               && _currentState.LeftButton == ButtonState.Pressed;
    }

    private bool GetIsLeftButtonReleased()
    {
        return _previousState.LeftButton == ButtonState.Pressed
               && _currentState.LeftButton == ButtonState.Released;
    }
}