using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using SnakeGame.DesktopGL.Core.Commands;

namespace SnakeGame.DesktopGL.Core.Events;

public class InputManager
{
    private KeyboardState _previousState;
    private KeyboardState _currentState;
    private MouseState _currentMouseState;
    
    private readonly Dictionary<Keys, ICommand> _keyDownBindings = new();
    private readonly Dictionary<Keys, ICommand> _keyPressedBindings = new();
    private readonly Dictionary<Keys, ICommand> _keyReleasedBindings = new();
    private ICommand _leftClickCommand;

    public void Update()
    {
        _previousState = _currentState;
        _currentState = Keyboard.GetState();
        _currentMouseState = Mouse.GetState();

        foreach (var key in _keyDownBindings.Keys)
        {
            if (IsKeyDown(key))
                _keyDownBindings[key].Execute();
        }

        foreach (var key in _keyPressedBindings.Keys)
        {
            if (IsKeyPressed(key))
                _keyPressedBindings[key].Execute();
        }

        foreach (var key in _keyReleasedBindings.Keys)
        {
            if (IsKeyReleased(key))
                _keyReleasedBindings[key].Execute();
        }

        if (_leftClickCommand != null && _currentMouseState.LeftButton == ButtonState.Pressed)
        {
            _leftClickCommand.Execute();
        }
    }

    public void BindKeyDown(Keys key, ICommand command)
    {
        _keyDownBindings.Add(key, command);
    }

    public void BindKeyPressed(Keys key, ICommand command)
    {
        _keyPressedBindings.Add(key, command);
    }

    public void BindKeyReleased(Keys key, ICommand command)
    {
        _keyReleasedBindings.Add(key, command);
    }

    public void BindLeftClick(ICommand command)
    {
        _leftClickCommand = command;
    }

    private bool IsKeyDown(Keys key)
    {
        return _currentState.IsKeyDown(key);
    }

    private bool IsKeyPressed(Keys key)
    {
        return _currentState.IsKeyDown(key) && _previousState.IsKeyUp(key);
    }

    private bool IsKeyReleased(Keys key)
    {
        return _currentState.IsKeyUp(key) && !_previousState.IsKeyDown(key);
    }
}