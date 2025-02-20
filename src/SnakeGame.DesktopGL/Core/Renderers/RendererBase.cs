using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.DesktopGL.Core.Renderers;

public abstract class RendererBase
{
    public abstract void LoadContent(ContentManager content);
    public abstract void Render(SpriteBatch spriteBatch, float deltaTime);
}