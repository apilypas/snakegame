using Microsoft.Xna.Framework;

namespace SnakeGame.DesktopGL.Core.Entities;

public class SnakeSegment : EntityBase
{
    public SnakeDirection Direction { get; set; }
    public bool IsCorner { get; set; }
    public bool IsClockwise { get; set; }

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
