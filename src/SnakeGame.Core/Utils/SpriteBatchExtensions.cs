using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

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
        // Automatically calculate shadow properties
        var size = spriteFont.MeasureString("A");
        var shadowOffset = new Vector2(MathF.Round(size.Y / 24f), MathF.Round(size.Y / 24f));
        
        // Shadow text
        spriteBatch.DrawString(
            spriteFont,
            text,
            position + shadowOffset,
            Colors.DefaultTextShadowColor,
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
    
    // Draws rectangle from nice patch
    public static void DrawFromNinePatch(
        this SpriteBatch spriteBatch,
        Vector2 position,
        SizeF size,
        Texture2D texture,
        Rectangle textureRectangle,
        Color color)
    {
        var patchSize = new Point(16, 16);
        
        // Top left corner
        spriteBatch.Draw(
            texture,
            new Rectangle((int)position.X, (int)position.Y, 16, 16),
            new Rectangle(textureRectangle.Location, patchSize),
            color);
        
        // Top right corner
        spriteBatch.Draw(
            texture,
            new Rectangle((int) (position.X + size.Width - 16), (int)position.Y, 16, 16),
            new Rectangle(textureRectangle.Location + new Point(32, 0), patchSize),
            color);
        
        // Bottom left corner
        spriteBatch.Draw(
            texture,
            new Rectangle((int)position.X, (int) (position.Y + size.Height - 16), 16, 16),
            new Rectangle(textureRectangle.Location + new Point(0, 32), patchSize),
            color);
        
        // Bottom right corner
        spriteBatch.Draw(
            texture,
            new Rectangle((int) (position.X + size.Width - 16), (int) (position.Y + size.Height - 16), 16, 16),
            new Rectangle(textureRectangle.Location + new Point(32, 32), patchSize),
            color);
        
        // Top border
        spriteBatch.Draw(
            texture,
            new Rectangle((int) (position.X + 16), (int)position.Y, (int) Math.Ceiling(size.Width - 32), 16),
            new Rectangle(textureRectangle.Location + new Point(16, 0), patchSize),
            color);
        
        // Bottom border
        spriteBatch.Draw(
            texture,
            new Rectangle((int) (position.X + 16), (int) (position.Y + size.Height - 16), (int) Math.Ceiling(size.Width - 32), 16),
            new Rectangle(textureRectangle.Location + new Point(16, 32), patchSize),
            color);
        
        // Left border
        spriteBatch.Draw(
            texture,
            new Rectangle((int) position.X, (int) (position.Y + 16), 16, (int) Math.Ceiling(size.Height - 32)),
            new Rectangle(textureRectangle.Location + new Point(0, 16), patchSize),
            color);
        
        // Right border
        spriteBatch.Draw(
            texture,
            new Rectangle((int) (position.X + size.Width - 16), (int) (position.Y + 16), 16, (int) Math.Ceiling(size.Height - 32)),
            new Rectangle(textureRectangle.Location + new Point(32, 16), patchSize),
            color);
        
        // Fill
        spriteBatch.Draw(
            texture,
            new Rectangle((int) (position.X + 16), (int) (position.Y + 16), (int) Math.Ceiling(size.Width - 32), (int) Math.Ceiling(size.Height - 32)),
            new Rectangle(textureRectangle.Location + new Point(16, 16), patchSize),
            color);
    }
}