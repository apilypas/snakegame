using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Core.Commands;

namespace SnakeGame.Core.Inputs;

public class InputBindingManager(KeyboardInputManager keyboard)
{
    private readonly Dictionary<Keys, ICommand> _keyDownBindings = new();
    private readonly Dictionary<Keys, ICommand> _keyPressedBindings = new();
    private readonly Dictionary<Keys, ICommand> _keyReleasedBindings = new();
    
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

    public void Update()
    {
        HandleKeyDownBindings();
        HandleKeyPressedBindings();
        HandleKeyReleasedBindings();
    }

    private void HandleKeyReleasedBindings()
    {
        foreach (var key in _keyReleasedBindings.Keys)
        {
            if (keyboard.IsKeyReleased(key))
            {
                _keyReleasedBindings[key].Execute();
            }
        }
    }

    private void HandleKeyPressedBindings()
    {
        foreach (var key in _keyPressedBindings.Keys)
        {
            if (keyboard.IsKeyPressed(key))
            {
                _keyPressedBindings[key].Execute();
            }
        }
    }

    private void HandleKeyDownBindings()
    {
        foreach (var key in _keyDownBindings.Keys)
        {
            if (keyboard.IsKeyDown(key))
            {
                _keyDownBindings[key].Execute();
            }
        }
    }
}