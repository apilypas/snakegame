using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SnakeGame.Core.Data;
using SnakeGame.Core.Enums;

namespace SnakeGame.Core.ECS.Components;

public class SnakeComponent
{
    public bool IsInitialized { get; set; }
    
    public bool IsAlive { get; set; }
    public float Speed { get; set; }
    public List<SnakeSegment> Segments { get; } = [];
    public SnakeSegment Head { get; set; } // Used for partial head
    public SnakeSegment Tail { get; set; } // User for partial tail
    public SnakeDirection Direction { get; set; } // Current direction
    public SnakeDirection FollowingDirection { get; set; } = SnakeDirection.Up; // Direction at next turn
    public SnakeDirection? NewDirection { get; set; } // Direction where player wants to go
    public int SegmentsToGrow { get; set; }
    public Color Color { get; set; } = Color.White;
    public float DeathAnimationTimer { get; set; }
    public Vector2 DefaultLocation { get; set; }
    public int DefaultLength { get; set; }
    public SnakeDirection DefaultDirection { get; set; }
}