using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Core.Renderers;

public abstract class RendererBase
{
    public abstract void LoadContent(GraphicsDevice graphicsDevice, ContentManager content);
    public abstract void Render(SpriteBatch spriteBatch, GameTime gameTime);
    public abstract void Update(GameTime gameTime);
}