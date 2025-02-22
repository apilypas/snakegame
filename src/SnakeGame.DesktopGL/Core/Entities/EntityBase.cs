using Microsoft.Xna.Framework;

namespace SnakeGame.DesktopGL.Core.Entities;

public abstract class EntityBase
{
    public virtual Vector2 Location { get; set; } = Vector2.Zero;
    public virtual float Rotation { get; set; } = 0f;
}