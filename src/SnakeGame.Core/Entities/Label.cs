using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.Entities;

public class Label : Control
{
    private Vector2 _drawAtPosition;
    
    public enum HorizontalLabelAlignment
    {
        Left,
        Center,
        Right
    }

    public enum VerticalLabelAlignment
    {
        Top,
        Center,
        Bottom
    }
    
    public SpriteFont Font { get; set; }
    public string Text { get; set; }
    public Color Color { get; set; } = Color.White;
    public HorizontalLabelAlignment HorizontalAlignment { get; set; } = HorizontalLabelAlignment.Left;
    public VerticalLabelAlignment VerticalAlignment { get; set; } = VerticalLabelAlignment.Top;

    private Vector2 GetStringDrawPosition()
    {
        var textSize = Font.MeasureString(Text);
        var x = 0f; // Left
        var y = 0f; // Top

        if (HorizontalAlignment == HorizontalLabelAlignment.Center)
            x = (Size.Width - textSize.X) / 2f;
        if (HorizontalAlignment == HorizontalLabelAlignment.Right)
            x = Size.Width - textSize.X;
        
        if (VerticalAlignment == VerticalLabelAlignment.Center)
            y = (Size.Height - textSize.Y) / 2f;
        if (VerticalAlignment == VerticalLabelAlignment.Bottom)
            y = Size.Height - textSize.Y;
        
        return new Vector2(x, y);
    }

    protected override void OnSizeChanged()
    {
        _drawAtPosition = GetStringDrawPosition();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawStringWithShadow(
            Font,
            Text,
            GlobalPosition + _drawAtPosition,
            Color,
            Rotation,
            Vector2.Zero);
    }
}