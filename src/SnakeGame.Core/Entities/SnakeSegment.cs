using Microsoft.Xna.Framework;

namespace SnakeGame.Core.Entities;

public class SnakeSegment : Entity
{
    public SnakeDirection Direction { get; init; }
    public bool IsCorner { get; init; }
    public bool IsClockwise { get; init; }
    public Color Color { get; set; }

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
