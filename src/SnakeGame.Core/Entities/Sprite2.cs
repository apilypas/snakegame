using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Core.Entities;

public class Sprite2 : Entity
{
    public Texture2D Texture { get; set; }
    public Color Color { get; set; } = Color.White;
    public Rectangle SourceRectangle { get; set; }
    public Vector2 Origin { get; set; }
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;

    public override void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
    {
        spriteBatch.Draw(
            Texture,
            new Rectangle((int)position.X, (int)position.Y, SourceRectangle.Width, SourceRectangle.Height),
            SourceRectangle,
            Color,
            Rotation,
            Origin,
            Effects,
            1f);
    }
}