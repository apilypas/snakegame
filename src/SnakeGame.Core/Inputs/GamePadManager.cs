using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Core.Commands;

namespace SnakeGame.Core.Inputs;

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