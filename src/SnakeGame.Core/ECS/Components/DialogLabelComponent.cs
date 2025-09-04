using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Core.ECS.Components;

public class DialogLabelComponent
{
    public SpriteFont Font { get; set; }
    public string Text { get; set; }
    public Color Color { get; set; } = Color.White;
}