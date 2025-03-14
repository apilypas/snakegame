using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Core.Commands;

namespace SnakeGame.Core.Inputs;

public class InputBindingManager(
    KeyboardInputHandler keyboard,
    MouseInputHandler mouse,
    TouchInputHandler touch,
    GamePadInputHandler gamePad)
{
    private readonly Dictionary<Keys, ICommand> _keyDownBindings = new();
    private readonly Dictionary<Keys, ICommand> _keyPressedBindings = new();
    private readonly Dictionary<Keys, ICommand> _keyReleasedBindings = new();
    
    private readonly Dictionary<Buttons, ICommand> _buttonPressedBindings = new();
    private readonly Dictionary<Buttons, ICommand> _buttonReleasedBindings = new();
    private readonly Dictionary<Buttons, ICommand> _buttonDownBindings = new();
    
    private ICommand _mouseLeftClickBinding;
    private ICommand _touchedBinding;
    
    public void BindKeyboardKeyDown(Keys key, ICommand command)
    {
        _keyDownBindings.Add(key, command);
    }

    public void BindKeyboardKeyPressed(Keys key, ICommand command)
    {
        _keyPressedBindings.Add(key, command);
    }

    public void BindKeyboardKeyReleased(Keys key, ICommand command)
    {
        _keyReleasedBindings.Add(key, command);
    }

    public void BindMouseLeftClick(ICommand command)
    {
        _mouseLeftClickBinding = command;
    }
    
    public void BindTouchScreenTouched(ICommand command)
    {
        _touchedBinding = command;
    }
    
    public void BindGamePadButtonPressed(Buttons button, ICommand command)
    {
        _buttonPressedBindings.Add(button, command);
    }
    
    public void BindGamePadButtonReleased(Buttons button, ICommand command)
    {
        _buttonReleasedBindings.Add(button, command);
    }

    public void BindGamePadButtonDown(Buttons button, ICommand command)
    {
        _buttonDownBindings.Add(button, command);
    }

    public void Update()
    {
        HandleKeyDownBindings();
        HandleKeyPressedBindings();
        HandleKeyReleasedBindings();
        HandleMouseLeftClickBindings();
        HandleTouchBindings();
        HandleButtonPressedBindings();
        HandleButtonReleasedBindings();
        HandleButtonDownBindings();
    }

    private void HandleButtonDownBindings()
    {
        if (!gamePad.IsConnected)
        {
            return;
        }
        
        foreach (var button in _buttonDownBindings.Keys)
        {
            if (gamePad.GetIsButtonDown(button))
            {
                _buttonDownBindings[button].Execute();
            }
        }
    }

    private void HandleButtonReleasedBindings()
    {
        if (!gamePad.IsConnected)
        {
            return;
        }
        
        foreach (var button in _buttonReleasedBindings.Keys)
        {
            if (gamePad.GetIsButtonReleased(button))
            {
                _buttonReleasedBindings[button].Execute();
            }
        }
    }

    private void HandleButtonPressedBindings()
    {
        if (!gamePad.IsConnected)
        {
            return;
        }
        
        foreach (var button in _buttonPressedBindings.Keys)
        {
            if (gamePad.GetIsButtonPressed(button))
            {
                _buttonPressedBindings[button].Execute();
            }
        }
    }

    private void HandleTouchBindings()
    {
        if (!touch.IsConnected)
        {
            return;
        }
        
        if (_touchedBinding != null && touch.IsTouchedAnywhere)
        {
            _touchedBinding.Execute();
        }
    }

    private void HandleMouseLeftClickBindings()
    {
        if (mouse.IsLeftButtonReleased)
        {
            _mouseLeftClickBinding?.Execute();
        }
    }

    private void HandleKeyReleasedBindings()
    {
        foreach (var key in _keyReleasedBindings.Keys)
        {
            if (keyboard.GetIsKeyReleased(key))
            {
                _keyReleasedBindings[key].Execute();
            }
        }
    }

    private void HandleKeyPressedBindings()
    {
        foreach (var key in _keyPressedBindings.Keys)
        {
            if (keyboard.GetIsKeyPressed(key))
            {
                _keyPressedBindings[key].Execute();
            }
        }
    }

    private void HandleKeyDownBindings()
    {
        foreach (var key in _keyDownBindings.Keys)
        {
            if (keyboard.GetIsKeyDown(key))
            {
                _keyDownBindings[key].Execute();
            }
        }
    }
}