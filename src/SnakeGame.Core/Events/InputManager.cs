using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using SnakeGame.Core.Commands;

namespace SnakeGame.Core.Events;

public class KeyboardInputManager
{
    private KeyboardState _previousState;
    private KeyboardState _currentState = Keyboard.GetState();
    
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

public class TouchInputManager
{
    private TouchCollection _touches;
    
    private ICommand _touchedBinding;

    public void Update()
    {
        _touches = TouchPanel.GetState();

        if (_touchedBinding != null && IsTouched)
        {
            _touchedBinding.Execute();
        }
    }
    
    public void BindTouched(ICommand command)
    {
        _touchedBinding = command;
    }

    public bool IsConnected => _touches.IsConnected;
    public bool IsTouched => _touches.Any(x => x.State is TouchLocationState.Pressed or TouchLocationState.Moved);
    
    public TouchCollection Touches => _touches;
}

public interface IVirtualGamePad
{
    GamePadState GetState(GamePadState state);
}

public class GamePadManager(PlayerIndex playerIndex = PlayerIndex.One)
{
    private readonly Dictionary<Buttons, ICommand> _buttonPressedBindings = new();
    private readonly Dictionary<Buttons, ICommand> _buttonReleasedBindings = new();
    private readonly Dictionary<Buttons, ICommand> _buttonDownBindings = new();

    private GamePadState _previousState;
    private GamePadState _currentState;

    public IVirtualGamePad VirtualGamePad { get; set; }
    
    public void Update()
    {
        _previousState = _currentState;
        _currentState = GamePad.GetState(playerIndex);
        
        if (VirtualGamePad != null)
            _currentState = VirtualGamePad.GetState(_currentState);
        
        foreach (var button in _buttonPressedBindings.Keys)
        {
            if (IsButtonPressed(button))
            {
                _buttonPressedBindings[button].Execute();
            }
        }
        
        foreach (var button in _buttonReleasedBindings.Keys)
        {
            if (IsButtonReleased(button))
            {
                _buttonReleasedBindings[button].Execute();
            }
        }
        
        foreach (var button in _buttonDownBindings.Keys)
        {
            if (IsButtonDown(button))
            {
                _buttonDownBindings[button].Execute();
            }
        }
    }

    public void BindButtonPressed(Buttons button, ICommand command)
    {
        _buttonPressedBindings.Add(button, command);
    }
    
    public void BindButtonReleased(Buttons button, ICommand command)
    {
        _buttonReleasedBindings.Add(button, command);
    }

    public void BindButtonDown(Buttons button, ICommand command)
    {
        _buttonDownBindings.Add(button, command);
    }
    
    private bool IsButtonPressed(Buttons button)
    {
        return _currentState.IsButtonDown(button) && _previousState.IsButtonUp(button);
    }

    private bool IsButtonReleased(Buttons button)
    {
        return _currentState.IsButtonUp(button) && _previousState.IsButtonDown(button);
    }

    private bool IsButtonDown(Buttons button)
    {
        return _currentState.IsButtonDown(button);
    }
}

public class InputManager
{
    public KeyboardInputManager Keyboard { get; } = new();
    public MouseInputManager Mouse { get; } = new();
    public TouchInputManager Touch { get; } = new();
    public GamePadManager GamePad { get; } = new();

    public void Update()
    {
        Keyboard.Update();
        Mouse.Update();
        Touch.Update();
        GamePad.Update();
    }
}