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
        RenderScores(spriteBatch);
    }
    
    private void RenderScores(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(
            _font,
            scoreBoard.ScoreText,
            new Vector2(GameWorld.GetRectangle().Width + 60f, 20f) + _offset,
            Colors.DefaultTextColor,
            0f,
            _font.MeasureString(scoreBoard.ScoreText) / 2f,
            1f,
            SpriteEffects.None,
            0f);
    }

    public override void Update(GameTime gameTime)
    {
    }
}
