using Microsoft.Xna.Framework;

namespace SnakeGame.DesktopGL.Core.Entities;

public abstract class EntityBase
{
    public int Id { get; set; } = 0;
    public Vector2 Location { get; set; } = Vector2.Zero;
    public float Rotation { get; set; } = 0f;
}