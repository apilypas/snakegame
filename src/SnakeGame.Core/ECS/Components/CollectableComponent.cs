using SnakeGame.Core.Enums;

namespace SnakeGame.Core.ECS.Components;

public class CollectableComponent
{
    public CollectableType CollectableType { get; set; }
    public int? CollectedByEntityId { get; set; }
}