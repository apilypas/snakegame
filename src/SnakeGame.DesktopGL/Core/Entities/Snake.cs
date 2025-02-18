using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SnakeGame.DesktopGL.Core.Entities;

public class SnakeSegment
{
    public Vector2 Location { get; set; }
    public SnakeDirection Direction { get; set; }

    public Rectangle GetRectangle()
    {
        return new Rectangle((int)Location.X, (int)Location.Y, Constants.SegmentSize, Constants.SegmentSize);
    }

    public float GetRotation()
    {
        if (Direction == SnakeDirection.Right)
            return 0f;

        if (Direction == SnakeDirection.Left)
            return MathF.PI;

        if (Direction == SnakeDirection.Down)
            return MathF.PI / 2f;

        if (Direction == SnakeDirection.Up)
            return -MathF.PI / 2f;

        return 0f;
    }
}

public enum SnakeDirection
{
    Up,
    Down,
    Left,
    Right
}

public class Snake
{
    private List<SnakeSegment> _segments;
    private SnakeSegment _head;
    private SnakeSegment _tail;

    private int _segmentsToGrow = 0;

    private SnakeDirection _direction;
    private SnakeDirection _nextDirection;

    public IList<SnakeSegment> Segments => _segments;
    public SnakeSegment Head => _head;
    public SnakeSegment Tail => _tail;

    public void Initialize()
    {
        ResetSnake(5);
    }

    public void ChangeDirection(SnakeDirection direction)
    {
        var head = _segments[0];

        if (head.Direction == direction)
            return;

        if (head.Direction == SnakeDirection.Right && direction == SnakeDirection.Left)
            return;
    
        if (head.Direction == SnakeDirection.Left && direction == SnakeDirection.Right)
            return;
    
        if (head.Direction == SnakeDirection.Up && direction == SnakeDirection.Down)
            return;
    
        if (head.Direction == SnakeDirection.Down && direction == SnakeDirection.Up)
            return;
    
        _nextDirection = direction;
    }

    public void Move(float deltaTime)
    {
        var movementSize = deltaTime * 100f;

        var head = _segments[0];
        var tail = _segments[^1];

        _head.Location = MoveByDirection(_head.Location, _direction, movementSize);

        if (_segmentsToGrow <= 0)
            _tail.Location = MoveByDirection(_tail.Location, tail.Direction, movementSize);

        if (_head.GetRectangle().Intersects(head.GetRectangle()))
            return;
    
        var newLocation = MoveByDirection(head.Location, _direction, Constants.SegmentSize);

        var newHead = new SnakeSegment
        {
            Location = newLocation,
            Direction = _nextDirection
        };

        _direction = _nextDirection;

        _segments.Insert(0, newHead);

        _head = new SnakeSegment
        {
            Direction = newHead.Direction,
            Location = newHead.Location
        };

        if (_segmentsToGrow > 0)
        {
            // Just don't remove the segment when one is needed
            _segmentsToGrow--;
        }
        else
        {
            _segments.Remove(tail);
        
            _tail = new SnakeSegment
            {
                Direction = _segments[^1].Direction,
                Location = _segments[^1].Location
            };
        }
    }

    public void Grow()
    {
        _segmentsToGrow++;
    }

    public bool IntersectsWithHead()
    {
        var headRectangle = _head.GetRectangle();

        for (var i = 1; i < _segments.Count; i++)
        {
            if (_segments[i].GetRectangle().Intersects(headRectangle))
                return true;
        }

        return false;
    }

    public bool IsOutOfBounds()
    {
        var location = _head.Location;

        if (location.X < 0f)
            return true;

        if (location.Y < 0f)
            return true;
    
        if (location.X + Constants.SegmentSize > Constants.WallWidth * Constants.SegmentSize)
            return true;
    
        if (location.Y + Constants.SegmentSize > Constants.WallHeight * Constants.SegmentSize)
            return true;

        return false;
    }

    private void ResetSnake(int length)
    {
        _segments = [];

        var position = new Vector2(
            Constants.WallWidth / 2f * Constants.SegmentSize,
            Constants.WallHeight / 2f * Constants.SegmentSize
        );

        for (var i = 0; i < length; i++)
        {
            var segment = new SnakeSegment
            {
                Location = position,
                Direction = SnakeDirection.Right
            };

            _segments.Add(segment);

            position.X -= Constants.SegmentSize;
        }

        _direction = SnakeDirection.Right;
        _nextDirection = SnakeDirection.Right;

        _head = new SnakeSegment
        {
            Direction = _segments[0].Direction,
            Location = _segments[0].Location
        };

        _tail = new SnakeSegment
        {
            Direction = _segments[^1].Direction,
            Location = _segments[^1].Location
        };
    }

    private Vector2 MoveByDirection(Vector2 location, SnakeDirection direction, float size)
    {
        if (direction == SnakeDirection.Right)
        {
            location += new Vector2(size, 0f);
        }

        if (direction == SnakeDirection.Left)
        {
            location -= new Vector2(size, 0f);
        }

        if (direction == SnakeDirection.Down)
        {
            location += new Vector2(0f, size);
        }

        if (direction == SnakeDirection.Up)
        {
            location -= new Vector2(0f, size);
        }

        return location;
    }
}
