using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class ModalsRenderer(ModalState gameState) : RendererBase
{
    private SpriteFont _font;

    public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _font = content.Load<SpriteFont>("MainFont");
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
        DrawModal(spriteBatch, "Game is paused");   
    }

    public override void Update(GameTime gameTime)
    {
    }

    private void DrawModal(SpriteBatch spriteBatch, string text)
    {
        var textSize = _font.MeasureString(text);
        
        var screenWidth = spriteBatch.GraphicsDevice.Viewport.Width;
        var screenHeight = spriteBatch.GraphicsDevice.Viewport.Height;

        var modalWidth = textSize.X + 80f;
        var modalHeight = textSize.Y + 80f;

        var modalX = (screenWidth - modalWidth) / 2f;
        var modalY = (screenHeight - modalHeight) / 2f;
        
        spriteBatch.FillRectangle(modalX, modalY, modalWidth, modalHeight, Colors.ModalBackgroundColor);
        
        spriteBatch.DrawString(
            _font,
            text,
            new Vector2(modalX + 40f, modalY + 40f),
            Colors.DefaultTextColor,
            0f,
            Vector2.Zero, 
            1f,
            SpriteEffects.None,
            0f);
    }
}