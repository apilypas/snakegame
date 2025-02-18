using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class UserInterfaceRenderer : RendererBase
{
    private Texture2D _texture;
    private SpriteFont _font;

    private Rectangle _gridPattern1Source;
    private Rectangle _gridPattern2Source;

    private Random _random;

    private GameWorld _gameWorld;

    private bool[][] _gridPattern = null;

    public UserInterfaceRenderer(GameWorld gameWorld)
    {
        _random = new Random();
        _gridPattern1Source = new Rectangle(Constants.SegmentSize, Constants.SegmentSize, Constants.SegmentSize, Constants.SegmentSize);
        _gridPattern2Source = new Rectangle(Constants.SegmentSize * 2, Constants.SegmentSize, Constants.SegmentSize, Constants.SegmentSize);
        _gameWorld = gameWorld;
    }

    public override void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("snake");
        _font = content.Load<SpriteFont>("font1");
    }

    public override void Render(SpriteBatch spriteBatch, float deltaTime)
    {
        RenderGrid(spriteBatch);
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

    private void RenderGrid(SpriteBatch spriteBatch)
    {
        if (_gridPattern == null)
        {
            _gridPattern = new bool[Constants.WallWidth][];

            for (var i = 0; i < Constants.WallWidth; i++)
            {
                _gridPattern[i] = new bool[Constants.WallHeight];
                for (var j = 0; j < Constants.WallHeight; j++)
                {
                    _gridPattern[i][j] = _random.Next() % 10 != 0;
                }
            }
        }

        for (var i = 0; i < Constants.WallHeight; i++)
        {
            var y = i * Constants.SegmentSize;
        
            for (var j = 0; j < Constants.WallWidth; j++)
            {
                var x = j * Constants.SegmentSize;

                if (_gridPattern[j][i])
                {
                    spriteBatch.Draw(
                        _texture,
                        new Vector2(x, y),
                        _gridPattern1Source,
                        Color.White);
                }
                else
                {
                    spriteBatch.Draw(
                        _texture,
                        new Vector2(x, y),
                        _gridPattern2Source,
                        Color.White);
                }
            }
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
