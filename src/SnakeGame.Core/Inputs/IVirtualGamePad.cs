using Microsoft.Xna.Framework.Input;

namespace SnakeGame.Core.Inputs;

public interface IVirtualGamePad
{
    GamePadState GetState(GamePadState state);
}