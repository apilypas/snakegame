using Microsoft.Xna.Framework.Input;
using SnakeGame.Core.Commands;

namespace SnakeGame.Core.Inputs;

public class MouseInputManager
{
    private MouseState _currentState = Mouse.GetState();
    private MouseState _previousState;
    
    private ICommand _leftClickBinding;

    public void Update()
    {
        _previousState = _currentState;
        _currentState = Mouse.GetState();
        
        if (_leftClickBinding != null
            && _currentState.LeftButton == ButtonState.Pressed
            && _previousState.LeftButton == ButtonState.Released)
        {
            _leftClickBinding.Execute();
        }
    }
    
    public bool IsLeftButtonDown => _currentState.LeftButton == ButtonState.Pressed;
    public MouseState State => _currentState;

    public void BindLeftClick(ICommand command)
    {
        _leftClickBinding = command;
    }
}