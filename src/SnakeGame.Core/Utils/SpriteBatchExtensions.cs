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
        Color color,
        int patchSizeX = 16,
        int patchSizeY = 16)
    {
        var patchSize = new Point(patchSizeX, patchSizeY);
        
        var destinationPatchSizeX = (int)Math.Ceiling(Math.Min(patchSizeX, size.Width / 3));
        var destinationPatchSizeY = (int)Math.Ceiling(Math.Min(patchSizeY, size.Height / 3));
        
        // Top left corner
        spriteBatch.Draw(
            texture,
            new Rectangle(
                (int)position.X,
                (int)position.Y,
                destinationPatchSizeX,
                destinationPatchSizeY),
            new Rectangle(
                textureRectangle.Location, 
                patchSize),
            color);
        
        // Top right corner
        spriteBatch.Draw(
            texture,
            new Rectangle(
                (int) (position.X + size.Width - destinationPatchSizeX), 
                (int)position.Y,
                destinationPatchSizeX, 
                destinationPatchSizeY),
            new Rectangle(
                textureRectangle.Location + new Point(patchSize.X * 2, 0), 
                patchSize),
            color);
        
        // Bottom left corner
        spriteBatch.Draw(
            texture,
            new Rectangle(
                (int)position.X, 
                (int) (position.Y + size.Height - destinationPatchSizeY),
                destinationPatchSizeX, 
                destinationPatchSizeY),
            new Rectangle(
                textureRectangle.Location + new Point(0, patchSize.Y * 2), 
                patchSize),
            color);
        
        // Bottom right corner
        spriteBatch.Draw(
            texture,
            new Rectangle(
                (int) (position.X + size.Width - destinationPatchSizeX), 
                (int) (position.Y + size.Height - destinationPatchSizeY), 
                destinationPatchSizeX, 
                destinationPatchSizeY),
            new Rectangle(
                textureRectangle.Location + new Point(patchSize.X * 2, patchSize.Y * 2), 
                patchSize),
            color);
        
        // Top border
        spriteBatch.Draw(
            texture,
            new Rectangle(
                (int) (position.X + destinationPatchSizeX), 
                (int)position.Y,
                (int) size.Width - destinationPatchSizeX * 2,
                destinationPatchSizeY),
            new Rectangle(
                textureRectangle.Location + new Point(patchSize.X, 0),
                patchSize),
            color);
        
        // Bottom border
        spriteBatch.Draw(
            texture,
            new Rectangle(
                (int) (position.X + destinationPatchSizeX),
                (int) (position.Y + size.Height - destinationPatchSizeY),
                (int) size.Width - destinationPatchSizeX * 2,
                destinationPatchSizeY),
            new Rectangle(
                textureRectangle.Location + new Point(patchSize.X, patchSize.Y * 2),
                patchSize),
            color);
        
        // Left border
        spriteBatch.Draw(
            texture,
            new Rectangle(
                (int) position.X,
                (int) (position.Y + destinationPatchSizeY),
                destinationPatchSizeX,
                (int) size.Height - destinationPatchSizeY * 2),
            new Rectangle(
                textureRectangle.Location + new Point(0, patchSize.Y),
                patchSize),
            color);
        
        // Right border
        spriteBatch.Draw(
            texture,
            new Rectangle(
                (int) (position.X + size.Width - destinationPatchSizeX),
                (int) (position.Y + destinationPatchSizeY),
                destinationPatchSizeX,
                (int) size.Height - destinationPatchSizeY * 2),
            new Rectangle(
                textureRectangle.Location + new Point(patchSize.X * 2, patchSize.Y),
                patchSize),
            color);
        
        // Fill
        spriteBatch.Draw(
            texture,
            new Rectangle(
                (int) (position.X + destinationPatchSizeX),
                (int) (position.Y + destinationPatchSizeY),
                (int) size.Width - destinationPatchSizeX * 2,
                (int) size.Height - destinationPatchSizeY * 2),
            new Rectangle(
                textureRectangle.Location + new Point(patchSize.X, patchSize.Y),
                patchSize),
            color);
    }
}