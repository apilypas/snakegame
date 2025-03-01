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
        _textSprite = TextSprite.Create().Load(content, "font1");
        _textSprite.Text = "Click anywhere to start";
    }

    public override void Render(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, float deltaTime)
    {
        Draw(spriteBatch, GetCenter(graphicsDevice), _textSprite);
    }

    private Vector2 GetCenter(GraphicsDevice graphicsDevice)
    {
        return new Vector2(graphicsDevice.Viewport.Width / 2f, graphicsDevice.Viewport.Height / 2f);
    }
}