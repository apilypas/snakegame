using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class UserInterfaceRenderer : RendererBase
{
    private Texture2D _texture;
    private SpriteFont _font;

    private GameWorld _gameWorld;

    public UserInterfaceRenderer(GameWorld gameWorld)
    {
        _gameWorld = gameWorld;
    }

    public override void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("snake");
        _font = content.Load<SpriteFont>("font1");
    }

    public override void Render(SpriteBatch spriteBatch, float deltaTime)
    {
        RenderScores(spriteBatch);
    }

    public void RenderModals(SpriteBatch spriteBatch)
    {
        if (_gameWorld.IsPaused)
        {
            spriteBatch.Draw(
                _texture,
                new Rectangle(100, 100, 300, 150),
                new Rectangle(Constants.SegmentSize * 3, Constants.SegmentSize, 20, 20),
                Color.White
            );

            var text = "Game is paused";
            spriteBatch.DrawString(
                _font,
                text,
                new Vector2(200, 150),
                Color.White,
                0,
                _font.MeasureString(text) / 2,
                1f,
                SpriteEffects.None,
                0f);
        }
    }

    private void RenderScores(SpriteBatch spriteBatch)
    {
        var scoreText = $"Score: {_gameWorld.Score}";
        spriteBatch.DrawString(
            _font,
            scoreText,
            new Vector2(Constants.WallWidth * Constants.SegmentSize + 60, 20),
            Color.White,
            0,
            _font.MeasureString(scoreText) / 2,
            1f,
            SpriteEffects.None,
            0f);
    }
}
