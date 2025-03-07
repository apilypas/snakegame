using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class ScoreBoardRenderer(ScoreBoard scoreBoard) : RendererBase
{
    private SpriteFont _font;
    private Vector2 _offset;

    public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _font = content.Load<SpriteFont>("MainFont");
        _offset = RendererUtils.GetPlayFieldOffset(graphicsDevice);
    }

    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        var displayTextPosition = new Vector2(GameWorld.GetRectangle().Width + 20f, 20f) + _offset;
        
        DrawLine(spriteBatch, scoreBoard.DisplayText, displayTextPosition);
    }

    public override void Update(GameTime gameTime)
    {
    }

    private void DrawLine(SpriteBatch spriteBatch, string text, Vector2 position)
    {
        spriteBatch.DrawString(
            _font,
            text,
            position,
            Colors.DefaultTextColor,
            0f,
            Vector2.Zero,
            1f,
            SpriteEffects.None,
            0f);
    }
}
