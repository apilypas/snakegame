using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class StartScreenRenderer : RendererBase
{
    private SpriteFont _font;
    
    public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _font = content.Load<SpriteFont>("MainFont");
    }

    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        const string text = "Click anywhere to start";

        spriteBatch.DrawString(
            _font,
            text,
            GetCenter(spriteBatch.GraphicsDevice),
            Colors.DefaultTextColor,
            0f,
            _font.MeasureString(text) / 2f,
            1f,
            SpriteEffects.None,
            0f);
    }

    private static Vector2 GetCenter(GraphicsDevice graphicsDevice)
    {
        return new Vector2(graphicsDevice.Viewport.Width / 2f, graphicsDevice.Viewport.Height / 2f);
    }

    public override void Update(GameTime gameTime)
    {
    }
}