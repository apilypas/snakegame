using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Core.Renderers;

public abstract class RendererBase
{
    public abstract void Render(SpriteBatch spriteBatch, GameTime gameTime);
}