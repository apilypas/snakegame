using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SnakeGame.DesktopGL.Core.Entities;

public class Snake
{
    private List<SnakeSegment> _segments;
    private SnakeSegment _head;
    private SnakeSegment _tail;
    private SnakeState _state = SnakeState.Alive;
    private Vector2 _initialLocation = Vector2.Zero;

    private int _segmentsToGrow = 0;

    private SnakeDirection _direction;
    private SnakeDirection _nextDirection;

    private float _deathAnimationTimer = 0f;
    private bool _hasSpeed = false;
    private float _speedTimer = 0f;

    public IList<SnakeSegment> Segments => _segments;
    public SnakeSegment Head => _head;
    public SnakeSegment Tail => _tail;
    public SnakeState State => _state;

    private Snake() { }

    protected Snake(Vector2 initialLocation)
    {
        _initialLocation = initialLocation;
    }

    public void Initialize()
    {
        Reset();
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

        _head.Location = MoveByDirection(_head.Location, _direction, movementSize);

        if (_segmentsToGrow <= 0)
            _tail.Location = MoveByDirection(_tail.Location, tail.Direction, movementSize);

        if (_head.GetRectangle().Intersects(head.GetRectangle()))
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
        _head = newHead.Clone();

        if (_segmentsToGrow > 0)
        {
            // Just don't remove the segment when one is needed
            _segmentsToGrow--;
        }
        else
        {
            _segments.Remove(tail);
            _tail = _segments[^1].Clone();
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

    public bool Intersects(Rectangle rectangle)
    {
        if (_head.GetRectangle().Intersects(rectangle))
            return true;

        if (_tail.GetRectangle().Intersects(rectangle))
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
        _state = SnakeState.Dead;
    }

    public bool Reduce(float deltaTime)
    {
        const float reduceByMs = .03f;
        bool reduced = false;

        if (_segments.Count > 0)
        {
            _deathAnimationTimer += deltaTime;
            
            if (_deathAnimationTimer >= reduceByMs)
            {
                _segments.RemoveAt(0);
                reduced = true;

                if (_segments.Count > 0)
                {
                    _head = _segments[0].Clone();
                    _deathAnimationTimer -= reduceByMs;
                }
                else
                {
                    _head = null;
                    _tail = null;
                }
            }
        }

        return reduced;
    }

    public void Reset(int length = Constants.InitialSnakeSize)
    {
        _segments = [];

        var position = _initialLocation;

        for (var i = 0; i < length; i++)
        {
            var segment = new SnakeSegment
            {
                Location = position,
                Direction = SnakeDirection.Right,
                Rotation = 0f
            };

            _segments.Add(segment);

            position.X -= Constants.SegmentSize;
        }

        _direction = SnakeDirection.Right;
        _nextDirection = SnakeDirection.Right;

        _head = _segments[0].Clone();
        _tail = _segments[^1].Clone();

        _state = SnakeState.Alive;
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
