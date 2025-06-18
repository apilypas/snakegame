using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Entities;

public class Control : Entity
{
    public SizeF Size { get; set; }
    public InputManager Inputs { get; set; }
    public bool IsHovered { get; set; }

    public override void Update(GameTime gameTime)
    {
        IsHovered = Inputs != null && GetGlobalRectangle().Contains(Inputs.Mouse.Position);
    }

    private Rectangle GetGlobalRectangle()
    {
        return new Rectangle((int)GlobalPosition.X, (int)GlobalPosition.Y, (int)Size.Width, (int)Size.Height);
    }
}