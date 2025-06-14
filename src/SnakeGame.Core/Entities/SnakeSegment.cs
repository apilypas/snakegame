using Microsoft.Xna.Framework;
using SnakeGame.Core.Enums;

namespace SnakeGame.Core.Entities;

public class SnakeSegment
{
    public SnakeDirection Direction { get; init; }
    public bool IsCorner { get; init; }
    public bool IsClockwise { get; init; }
    public Color Color { get; set; }
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }

    public Rectangle GetRectangle()
    {
        return new Rectangle((int)Position.X, (int)Position.Y, Constants.SegmentSize, Constants.SegmentSize);
    }

    public SnakeSegment Clone()
    {
        return new SnakeSegment
        {
            Position = Position,
            Rotation = Rotation,
            Direction = Direction,
            IsCorner = IsCorner,
            IsClockwise = IsClockwise
        };
    }
}
