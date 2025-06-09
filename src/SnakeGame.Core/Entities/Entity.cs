using Microsoft.Xna.Framework;

namespace SnakeGame.Core.Entities;

public abstract class Entity
{
    public int Id { get; set; }
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
}