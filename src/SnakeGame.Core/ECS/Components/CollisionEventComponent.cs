namespace SnakeGame.Core.ECS.Components;

public class CollisionEventComponent
{
    public int EntityId { get; set; }
    public int CollidesWithEntityId { get; set; }
}