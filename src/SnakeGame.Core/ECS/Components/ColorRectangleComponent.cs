using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace SnakeGame.Core.ECS.Components;

public class ColorRectangleComponent
{
    public Color FillColor { get; set; } = Color.White;
    public SizeF Size { get; set; }
}