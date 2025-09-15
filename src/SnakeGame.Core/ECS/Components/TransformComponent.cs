using Microsoft.Xna.Framework;

namespace SnakeGame.Core.ECS.Components;

public class TransformComponent
{
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
    public Vector2 Scale { get; set; } = Vector2.One;
}