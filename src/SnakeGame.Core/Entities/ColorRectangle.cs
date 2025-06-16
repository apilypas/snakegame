using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace SnakeGame.Core.Entities;

public class ColorRectangle : Control
{
    public Color FillColor { get; set; } = Color.White;
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.FillRectangle(
            GlobalPosition,
            Size,
            FillColor,
            1f);
    }
}