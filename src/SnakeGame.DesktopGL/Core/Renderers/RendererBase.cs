using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.DesktopGL.Core.Entities;
using SnakeGame.DesktopGL.Core.Sprites;

namespace SnakeGame.DesktopGL.Core.Renderers;

public abstract class RendererBase
{
    protected Vector2 Offset { get; set; }

    public abstract void LoadContent(ContentManager content);
    public abstract void Render(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, float deltaTime);

    protected void Draw(SpriteBatch spriteBatch, EntityBase entity, TextureSprite sprite)
    {
        var location = entity.Location + Offset;
        var rotation = entity.Rotation + sprite.Rotation;

        spriteBatch.Draw(
            sprite.Texture,
            location + sprite.Origin,
            sprite.SourceRectangle,
            sprite.Color,
            rotation,
            sprite.Origin,
            sprite.Scale,
            sprite.Effects,
            sprite.LayerDepth);
    }

    protected void Draw(SpriteBatch spriteBatch, Vector2 location, TextureSprite sprite)
    {
        spriteBatch.Draw(
            sprite.Texture,
            location + Offset + sprite.Origin,
            sprite.SourceRectangle,
            sprite.Color,
            sprite.Rotation,
            sprite.Origin,
            sprite.Scale,
            sprite.Effects,
            sprite.LayerDepth);
    }

    protected void Draw(SpriteBatch spriteBatch, Vector2 location, TextSprite sprite)
    {
        spriteBatch.DrawString(
            sprite.Font,
            sprite.Text,
            location + Offset,
            sprite.Color,
            sprite.Rotation,
            sprite.Font.MeasureString(sprite.Text) / 2f,
            sprite.Scale,
            SpriteEffects.None,
            sprite.LayerDepth);
    }
}