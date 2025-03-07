using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using SnakeGame.DesktopGL.Core.Commands;

namespace SnakeGame.DesktopGL.Core.Events;

public class InputManager
{
    private KeyboardState _previousState;
    private KeyboardState _currentState;
    
    private MouseState _previousMouseState;
    private MouseState _currentMouseState;

    private bool _isBindingsEnabled = true;
    
    private readonly Dictionary<Keys, ICommand> _keyDownBindings = new();
    private readonly Dictionary<Keys, ICommand> _keyPressedBindings = new();
    private readonly Dictionary<Keys, ICommand> _keyReleasedBindings = new();
    private ICommand _leftClickBinding;
    
    public MouseState MouseState => _currentMouseState;
    public KeyboardState KeyboardState => _currentState;

    public void Update()
    {
        _previousState = _currentState;
        _currentState = Keyboard.GetState();

        _previousMouseState = _currentMouseState;
        _currentMouseState = Mouse.GetState();

        if (_isBindingsEnabled)
        {
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

            if (_leftClickBinding != null && _currentMouseState.LeftButton == ButtonState.Pressed)
            {
                _leftClickBinding.Execute();
            }
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
        _leftClickBinding = command;
    }

    public void EnableBindings()
    {
        _isBindingsEnabled = true;
    }

    public void DisableBindings()
    {
        _isBindingsEnabled = false;
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
        return _currentState.IsKeyUp(key) && _previousState.IsKeyDown(key);
    }
}