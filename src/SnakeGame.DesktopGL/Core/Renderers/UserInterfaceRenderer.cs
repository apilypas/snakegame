using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.DesktopGL.Core.Sprites;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class UserInterfaceRenderer : RendererBase
{
    private Texture2D _texture;
    private SpriteFont _font;
    private TextSprite _scoreSprite;

    private GameWorld _gameWorld;

    public UserInterfaceRenderer(GameWorld gameWorld)
    {
        _gameWorld = gameWorld;
    }

    public override void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("snake");
        _font = content.Load<SpriteFont>("font1");

        _scoreSprite = TextSprite.Create().Load(content, "font1");
    }

    public override void Render(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, float deltaTime)
    {
        Offset = PlayFieldRenderer.GetPlayFieldOffset(graphicsDevice);

        RenderScores(spriteBatch);
        RenderModals(spriteBatch);
    }

    public void RenderModals(SpriteBatch spriteBatch)
    {
        // TODO: this should be implemented elsewhere
        if (_gameWorld.IsPaused)
        {
            spriteBatch.Draw(
                _texture,
                new Rectangle(100, 100, 300, 150),
                new Rectangle(20, 40, 20, 20),
                Color.White
            );

            var text = "Game is paused";
            spriteBatch.DrawString(
                _font,
                text,
                new Vector2(200, 150),
                Colors.DefaultTextColor,
                0,
                _font.MeasureString(text) / 2,
                1f,
                SpriteEffects.None,
                0f);
        }
    }

    private void RenderScores(SpriteBatch spriteBatch)
    {
        Draw(spriteBatch,
            new Vector2(_gameWorld.GetRectangle().Width + 60f, 20f),
            _scoreSprite.WithText($"Score: {_gameWorld.Score}"));
    }
}
