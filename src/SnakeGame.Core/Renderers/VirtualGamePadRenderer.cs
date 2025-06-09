using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
using SnakeGame.Core.Core.Systems;
using SnakeGame.Core.Entities;

namespace SnakeGame.Core.Renderers;

public class VirtualGamePadRenderer(VirtualGamePadManager gamePadManager, VirtualGamePad virtualGamePad) : RendererBase
{
    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        if (gamePadManager.IsConnected && gamePadManager.IsVisible)
        {
            RenderButton(spriteBatch, gamePadManager.LeftButton, virtualGamePad.LeftArrowSprite);
            RenderButton(spriteBatch, gamePadManager.RightButton, virtualGamePad.RightArrowSprite);
            RenderButton(spriteBatch, gamePadManager.UpButton, virtualGamePad.UpArrowSprite);
            RenderButton(spriteBatch, gamePadManager.DownButton, virtualGamePad.DownArrowSprite);
            RenderButton(spriteBatch, gamePadManager.StartButton, virtualGamePad.PauseSprite);
            RenderBigButton(spriteBatch, gamePadManager.ActionButton, virtualGamePad.ActionSprite);
        }
    }

    private void RenderBigButton(SpriteBatch spriteBatch, VirtualGamePadManager.VirtualGamePadButton button, Sprite signSprite)
    {
        if (button.IsPressed)
            virtualGamePad.PressedBigButtonSprite.Draw(spriteBatch, button.Position, 0f, Vector2.One);
        else
            virtualGamePad.BigButtonSprite.Draw(spriteBatch, button.Position, 0f, Vector2.One);
        
        signSprite.Draw(spriteBatch, button.Position + new Vector2(32f, 32f), 0f, Vector2.One);
    }

    private void RenderButton(SpriteBatch spriteBatch, VirtualGamePadManager.VirtualGamePadButton button, Sprite signSprite)
    {
        if (button.IsPressed)
            virtualGamePad.PressedButtonSprite.Draw(spriteBatch, button.Position, 0f, Vector2.One);
        else
            virtualGamePad.ButtonSprite.Draw(spriteBatch, button.Position, 0f, Vector2.One);
        
        signSprite.Draw(spriteBatch, button.Position + new Vector2(16f, 16f), 0f, Vector2.One);
    }
}