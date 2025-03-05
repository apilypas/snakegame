using Microsoft.Xna.Framework;

namespace SnakeGame.DesktopGL.Core.Entities;

public class SnakeSegment : EntityBase
{
    public SnakeDirection Direction { get; init; }
    public bool IsCorner { get; init; }
    public bool IsClockwise { get; init; }

    public Rectangle GetRectangle()
    {
        return new Rectangle((int)Location.X, (int)Location.Y, Constants.SegmentSize, Constants.SegmentSize);
    }

    public SnakeSegment Clone()
    {
        return new SnakeSegment
        {
            Location = Location,
            Rotation = Rotation,
            Direction = Direction,
            IsCorner = IsCorner,
            IsClockwise = IsClockwise
        };
    }
}
