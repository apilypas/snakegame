using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class ModalsRenderer(ModalState gameState) : RendererBase
{
    private Sprite _backgroundSprite;
    private Texture2D _texture;
    private SpriteFont _font;

    public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _font = content.Load<SpriteFont>("font1");
        _texture = content.Load<Texture2D>("snake");
        
        _backgroundSprite = new Sprite(new Texture2DRegion(
            _texture,
            new Rectangle(20, 40, 20, 20)
            ));
    }

    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        RenderModals(spriteBatch);
    }

    private void RenderModals(SpriteBatch spriteBatch)
    {
        if (gameState.Type == ModalState.ModalStateType.Paused)
            DrawPausedModal(spriteBatch);
    }

    private void DrawPausedModal(SpriteBatch spriteBatch)
    {
        for (var i = 0; i < 10; i++)
        {
            for (var j = 0; j < 20; j++)
            {
                _backgroundSprite.Draw(spriteBatch, new Vector2(100 + j * 20, 100 + i * 20), 0f, Vector2.One);
            }
        }
        
        const string text = "Game is paused";
        spriteBatch.DrawString(
            _font,
            text,
            new Vector2(200, 150),
            Colors.DefaultTextColor,
            0f,
            _font.MeasureString(text) / 2f,
            1f,
            SpriteEffects.None,
            0f);
    }

    public override void Update(GameTime gameTime)
    {
    }
}