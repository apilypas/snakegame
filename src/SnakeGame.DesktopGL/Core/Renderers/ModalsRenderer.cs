using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.DesktopGL.Core.Renderers;
using SnakeGame.DesktopGL.Core.Sprites;

public class ModalsRenderer : RendererBase
{
    private TextSprite _textSprite;
    private TextureSprite _bacgroundSprite;

    private ModalState _gameState;

    public ModalsRenderer(ModalState gameState)
    {
        _gameState = gameState;
    }

    public override void LoadContent(ContentManager content)
    {
        _textSprite = TextSprite.Create().Load(content, "font1");
        _textSprite.Text = "Game is paused";

        _bacgroundSprite = TextureSprite.Create(new Rectangle(20, 40, 20, 20)).Load(content, "snake");
    }

    public override void Render(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, float deltaTime)
    {
        RenderModals(spriteBatch);
    }

    public void RenderModals(SpriteBatch spriteBatch)
    {
        if (_gameState.Type == ModalState.ModalStateType.Paused)
            DrawPausedModal(spriteBatch);
    }

    private void DrawPausedModal(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                Draw(spriteBatch, new Vector2(100 + j * 20, 100 + i * 20), _bacgroundSprite);
            }
        }
        
        Draw(spriteBatch, new Vector2(200, 150), _textSprite);
    }
}