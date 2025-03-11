using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using SnakeGame.Core.Commands;
using SnakeGame.Core.Screens;

namespace SnakeGame.Core.Events;

public class KeyboardInputManager
{
    private KeyboardState _previousState;
    private KeyboardState _currentState;
    
    private bool _isBindingsEnabled = true;
    
    private readonly Dictionary<Keys, ICommand> _keyDownBindings = new();
    private readonly Dictionary<Keys, ICommand> _keyPressedBindings = new();
    private readonly Dictionary<Keys, ICommand> _keyReleasedBindings = new();
    
    public KeyboardState KeyboardState => _currentState;

    public void Update()
    {
        _previousState = _currentState;
        _currentState = Keyboard.GetState();
        
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

public class MouseInputManager(ScreenBase screen)
{
    private MouseState _state;
    
    private ICommand _leftClickBinding;

    public void Update()
    {
        _state = Mouse.GetState();
        
        if (_leftClickBinding != null && _state.LeftButton == ButtonState.Pressed)
        {
            _leftClickBinding.Execute();
        }
    }
    
    public bool IsLeftButtonPressed => _state.LeftButton == ButtonState.Pressed;
    public MouseState State => _state;

    public void BindLeftClick(ICommand command)
    {
        _leftClickBinding = command;
    }
}

public class TouchInputManager(ScreenBase screen)
{
    private TouchCollection _state;
    
    private ICommand _touchedBinding;

    public void Update()
    {
        _state = TouchPanel.GetState();

        if (_touchedBinding != null && IsTouched)
        {
            _touchedBinding.Execute();
        }
    }
    
    public void BindTouched(ICommand command)
    {
        _touchedBinding = command;
    }

    public bool IsConnected => _state.IsConnected;
    public bool IsTouched => _state.Any(x => x.State is TouchLocationState.Pressed or TouchLocationState.Moved);
    
    public TouchCollection State => _state;
}

public class InputManager(ScreenBase screen)
{
    public KeyboardInputManager Keyboard { get; } = new();
    public MouseInputManager Mouse { get; } = new(screen);
    public TouchInputManager Touch { get; } = new(screen);

    public void Update()
    {
        Keyboard.Update();
        Mouse.Update();
        Touch.Update();
    }
}