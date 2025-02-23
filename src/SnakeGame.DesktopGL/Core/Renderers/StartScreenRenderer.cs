using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.DesktopGL.Core.Renderers;
using SnakeGame.DesktopGL.Core.Sprites;

public class StartScreenRenderer : RendererBase
{
    private TextSprite _textSprite;
    
    public StartScreenRenderer()
    {
    }

    public override void LoadContent(ContentManager content)
    {
        _textSprite = TextSprite.Create("Click anywhere to start")
            .Load(content, "font1");
    }

    public override void Render(SpriteBatch spriteBatch, float deltaTime)
    {
        Draw(spriteBatch, GetCenter(spriteBatch.GraphicsDevice), _textSprite);
    }

    private Vector2 GetCenter(GraphicsDevice graphicsDevice)
    {
        return new Vector2(graphicsDevice.Viewport.Width / 2f, graphicsDevice.Viewport.Height / 2f);
    }
}