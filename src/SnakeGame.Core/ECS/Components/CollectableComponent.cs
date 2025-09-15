using System.Collections.Generic;
using MonoGame.Extended.Tweening;
using SnakeGame.Core.Enums;

namespace SnakeGame.Core.ECS.Components;

public class CollectableComponent
{
    public CollectableType CollectableType { get; set; }
    public int? CollectedByEntityId { get; set; }
    public HashSet<Tween> PulseTweens { get; } = [];
}