using Microsoft.Xna.Framework;

namespace SnakeGame.Core.StateMachines;

public abstract class CharacterState
{
    public abstract void Update(GameTime gameTime);
}
