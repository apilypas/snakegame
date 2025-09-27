using SnakeGame.Core.Enums;

namespace SnakeGame.Core.ECS.Components;

public class CollectableSpawnerComponent
{
    public CollectableType CollectableType { get; set; }
    public int Count { get; set; }
}