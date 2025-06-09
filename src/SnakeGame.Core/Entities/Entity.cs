using Microsoft.Xna.Framework;

namespace SnakeGame.Core.Entities;

public abstract class Entity
{
    public int Id { get; set; } = 0;
    public Vector2 Location { get; set; } = Vector2.Zero;
    public float Rotation { get; set; } = 0f;
}