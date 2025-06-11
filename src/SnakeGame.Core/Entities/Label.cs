using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.Entities;

public class Label : Entity
{
    public SpriteFont Font { get; set; }
    public string Text { get; set; }
    public Color Color { get; set; } = Color.White;

    public override void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
    {
        spriteBatch.DrawStringWithShadow(
            Font,
            Text,
            position,
            Color,
            Rotation,
            Vector2.Zero);
    }
}