using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Core.Utils;

public static class SpriteBatchExtensions
{
    public static void DrawStringWithShadow(
        this SpriteBatch spriteBatch,
        SpriteFont spriteFont,
        string text,
        Vector2 position,
        Color color,
        float rotation = 0f,
        Vector2 origin = new(),
        float scale = 1f,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0f)
    {
        // Shadow text
        spriteBatch.DrawString(
            spriteFont,
            text,
            position + Vector2.One,
            Color.Black,
            rotation,
            origin,
            scale,
            effects,
            layerDepth);
                
        // Visible text
        spriteBatch.DrawString(
            spriteFont,
            text,
            position,
            color,
            rotation,
            origin,
            scale,
            effects,
            layerDepth);
    }
}