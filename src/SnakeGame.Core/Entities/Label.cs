using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.Entities;

public class Label : Entity
{
    public SpriteFont Font { get; set; }
    public string Text { get; set; }
    public Color Color { get; set; }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawStringWithShadow(
            Font,
            Text,
            GlobalPosition,
            Color,
            Rotation,
            Vector2.Zero);
    }
}