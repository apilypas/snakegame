using Microsoft.Xna.Framework;

namespace SnakeGame.Core.Entities;

public abstract class CharacterState
{
    public abstract void Update(GameTime gameTime);
}
