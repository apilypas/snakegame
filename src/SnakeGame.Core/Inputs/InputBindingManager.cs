using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Core.Commands;

namespace SnakeGame.Core.Inputs;

public class InputBindingManager(KeyboardInputHandler keyboard, MouseInputHandler mouse)
{
    private readonly Dictionary<Keys, ICommand> _keyDownBindings = new();
    private readonly Dictionary<Keys, ICommand> _keyPressedBindings = new();
    private readonly Dictionary<Keys, ICommand> _keyReleasedBindings = new();
    private ICommand _mouseLeftClickBinding;
    
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

    public void BindMouseLeftClick(ICommand command)
    {
        _mouseLeftClickBinding = command;
    }

    public void Update()
    {
        HandleKeyDownBindings();
        HandleKeyPressedBindings();
        HandleKeyReleasedBindings();
        HandleMouseLeftClickBinding();
    }

    private void HandleMouseLeftClickBinding()
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