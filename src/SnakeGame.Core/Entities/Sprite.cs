using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Core.Entities;

public class Sprite : Entity
{
    public Texture2D Texture { get; set; }
    public Color Color { get; set; } = Color.White;
    public Rectangle SourceRectangle { get; set; }
    public Vector2 Origin { get; set; }
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            new Rectangle((int)GlobalPosition.X, (int)GlobalPosition.Y, SourceRectangle.Width, SourceRectangle.Height),
            SourceRectangle,
            Color,
            Rotation,
            Origin,
            Effects,
            1f);
    }
}