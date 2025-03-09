using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SnakeGame.Core.Entities;

public class Snake : EntityBase
{
    private List<SnakeSegment> _segments;
    
    private int _segmentsToGrow;

    private SnakeDirection _direction;
    private SnakeDirection _nextDirection;

    private float _deathAnimationTimer = 0f;
    private bool _hasSpeed = false;
    private float _speedTimer = 0f;

    public IList<SnakeSegment> Segments => _segments;
    public SnakeSegment Head { get; private set; }
    public SnakeSegment Tail { get; private set; }
    public SnakeState State { get; private set; } = SnakeState.Alive;
    
    private readonly Vector2 _defaultLocation;
    private readonly int _defaultLength;
    private readonly SnakeDirection _defaultDirection;

    protected Snake(Vector2 location, int length, SnakeDirection direction)
    {
        _defaultLocation = location;
        _defaultLength = length;
        _defaultDirection = direction;
    }

    public void Initialize()
    {
        Reset(_defaultLocation, _defaultLength, _defaultDirection);
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

    public virtual void Update(float deltaTime)
    {
        var speed = (_hasSpeed || _speedTimer > 0f) ? Constants.IncreasedSnakeSpeed : Constants.DefaultSnakeSpeed;

        if (_speedTimer > 0f)
            _speedTimer -= deltaTime;

        var movementSize = deltaTime * speed;

        var head = _segments[0];
        var tail = _segments[^1];

        Head.Location = MoveByDirection(Head.Location, _direction, movementSize);

        if (_segmentsToGrow <= 0)
            Tail.Location = MoveByDirection(Tail.Location, tail.Direction, movementSize);

        if (Head.GetRectangle().Intersects(head.GetRectangle()))
            return;
    
        var newLocation = MoveByDirection(head.Location, _direction, Constants.SegmentSize);

        var newHead = new SnakeSegment
        {
            Location = newLocation,
            Direction = _nextDirection,
            Rotation = GetRotation(_nextDirection),
            IsCorner = _nextDirection != head.Direction,
            IsClockwise = head.Direction.IsClockwise(_nextDirection)
        };

        _direction = _nextDirection;

        _segments.Insert(0, newHead);
        Head = newHead.Clone();

        if (_segmentsToGrow > 0)
        {
            // Just don't remove the segment when one is needed
            _segmentsToGrow--;
        }
        else
        {
            _segments.Remove(tail);
            Tail = _segments[^1].Clone();
        }
    }

    public void Grow()
    {
        _segmentsToGrow++;
    }

    public bool IntersectsWithHead()
    {
        var headRectangle = Head.GetRectangle();

        for (var i = 1; i < _segments.Count; i++)
        {
            if (_segments[i].GetRectangle().Intersects(headRectangle))
                return true;
        }

        return false;
    }

    public bool Intersects(Rectangle rectangle)
    {
        if (Head.GetRectangle().Intersects(rectangle))
            return true;

        if (Tail.GetRectangle().Intersects(rectangle))
            return true;

        foreach (var segment in _segments)
        {
            if (segment.GetRectangle().Intersects(rectangle))
                return true;
        }

        return false;
    }

    public void Die()
    {
        State = SnakeState.Dead;
    }

    public bool Reduce(float deltaTime)
    {
        const float reduceByMs = .03f;
        var reduced = false;

        if (_segments.Count > 0)
        {
            _deathAnimationTimer += deltaTime;
            
            if (_deathAnimationTimer >= reduceByMs)
            {
                _segments.RemoveAt(0);
                reduced = true;

                if (_segments.Count > 0)
                {
                    Head = _segments[0].Clone();
                    _deathAnimationTimer -= reduceByMs;
                }
                else
                {
                    Head = null;
                    Tail = null;
                }
            }
        }

        return reduced;
    }

    public void Reset(Vector2 location, int length, SnakeDirection direction)
    {
        _segments = [];

        for (var i = 0; i < length; i++)
        {
            var segment = new SnakeSegment
            {
                Location = location,
                Direction = direction,
                Rotation = GetRotation(direction)
            };

            _segments.Add(segment);

            location = MoveByDirection(location, direction.GetOpposite(), Constants.SegmentSize);
        }

        _direction = direction;
        _nextDirection = direction;

        Head = _segments[0].Clone();
        Tail = _segments[^1].Clone();
        
        State = SnakeState.Alive;
        _speedTimer = 0f;
        _hasSpeed = false;
    }

    public void SpeedUp()
    {
        _hasSpeed = true;
    }

    public void SpeedDown()
    {
        _hasSpeed = false;
    }

    public void ResetSpeedUpTimer()
    {
        _speedTimer = Constants.SpeedUpRate;
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

    private float GetRotation(SnakeDirection direction)
    {
        if (direction == SnakeDirection.Right)
            return 0f;

        if (direction == SnakeDirection.Left)
            return MathF.PI;

        if (direction == SnakeDirection.Down)
            return MathF.PI / 2f;

        if (direction == SnakeDirection.Up)
            return -MathF.PI / 2f;

        return 0f;
    }
}
