using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.DesktopGL.Core.Entities;
using SnakeGame.DesktopGL.Core.Sprites;

namespace SnakeGame.DesktopGL.Core.Renderers;

public abstract class RendererBase
{
    public Vector2 Offset { get; set; }

    public abstract void LoadContent(ContentManager content);
    public abstract void Render(SpriteBatch spriteBatch, float deltaTime);

    protected void Draw(SpriteBatch spriteBatch, EntityBase entity, TextureSprite sprite)
    {
        var location = entity.Location + Offset;
        var rotation = entity.Rotation + sprite.Rotation;

        spriteBatch.Draw(
            sprite.Texture,
            location + sprite.Origin,
            sprite.SourceRectangle,
            Color.White,
            rotation,
            sprite.Origin,
            1f,
            sprite.Effects,
            0f);
    }

    protected void Draw(SpriteBatch spriteBatch, Vector2 location, TextureSprite sprite)
    {
        spriteBatch.Draw(
            sprite.Texture,
            location + Offset + sprite.Origin,
            sprite.SourceRectangle,
            Color.White,
            sprite.Rotation,
            sprite.Origin,
            1f,
            sprite.Effects,
            0f);
    }
}