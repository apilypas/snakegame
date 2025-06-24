using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SnakeGame.Core.Inputs;

public class GamePadInputHandler(PlayerIndex playerIndex = PlayerIndex.One)
{
    private GamePadState _previousState = GamePad.GetState(playerIndex);
    private GamePadState _currentState = GamePad.GetState(playerIndex);
    
    public bool IsConnected => GetIsConnected();
    
    public void Update()
    {
        _previousState = _currentState;
        _currentState = GamePad.GetState(playerIndex);
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

    private bool GetIsConnected()
    {
        return _currentState.IsConnected;
    }
}