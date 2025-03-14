using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SnakeGame.Core.Inputs;

public class GamePadInputHandler(PlayerIndex playerIndex = PlayerIndex.One)
{
    public interface IVirtualGamePad
    {
        GamePadState GetState(GamePadState state);
        bool IsConnected { get; }
    }
    
    private GamePadState _previousState = GamePad.GetState(playerIndex);
    private GamePadState _currentState = GamePad.GetState(playerIndex);
    private IVirtualGamePad _virtualGamePad;
    
    public bool IsConnected => GetIsConnected();
    
    public void Update()
    {
        _previousState = _currentState;
        _currentState = GamePad.GetState(playerIndex);
        
        if (_virtualGamePad != null)
            _currentState = _virtualGamePad.GetState(_currentState);
    }
    
    public bool GetIsButtonPressed(Buttons button)
    {
        return _currentState.IsButtonDown(button) && _previousState.IsButtonUp(button);
    }

    public bool GetIsButtonReleased(Buttons button)
    {
        return _currentState.IsButtonUp(button) && _previousState.IsButtonDown(button);
    }

    public bool GetIsButtonDown(Buttons button)
    {
        return _currentState.IsButtonDown(button);
    }

    public void AttachVirtualGamePad(IVirtualGamePad virtualGamePad)
    {
        _virtualGamePad = virtualGamePad;
    }

    private bool GetIsConnected()
    {
        return _currentState.IsConnected || _virtualGamePad is { IsConnected: true };
    }
}