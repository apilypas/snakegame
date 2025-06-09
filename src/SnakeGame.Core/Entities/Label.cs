using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Core.Entities;

public class Label : Entity
{
    public SpriteFont Font { get; set; }
    public string Text { get; set; }
}