using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Enums;
using SnakeGame.Core.Renderers;
using SnakeGame.Core.StateMachines;
using SnakeGame.Core.Systems;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.Entities;

public class Snake : Entity
{
    private readonly static Random Random = new();
    private readonly static Color[] InvincibleColors = [Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.White]; 
    
    private int _segmentsToGrow;

    private SnakeDirection _direction;
    private SnakeDirection _nextDirection;

    private CharacterState _state;

    private float _deathAnimationTimer;
    private bool _isFaster;
    private float _speedTimer;
    
    private readonly Vector2 _defaultLocation;
    private readonly int _defaultLength;
    private readonly SnakeDirection _defaultDirection;
    
    private SnakeRenderer _renderer;
    private readonly AssetManager _assets;

    public List<SnakeSegment> Segments { get; } = [];
    public SnakeSegment Head { get; private set; } // Used for partial head
    public SnakeSegment Tail { get; private set; } // User for partial tail
    
    public SnakeState State { get; private set; } = SnakeState.Alive;
    public Color Color { get; set; } = Color.White;
    public bool IsInvincible { get; set; }

    protected Snake(AssetManager assets, Vector2 location, int length, SnakeDirection direction)
    {
        _assets = assets;
        _defaultLocation = location;
        _defaultLength = length;
        _defaultDirection = direction;
    }

    public virtual void Initialize()
    {
        _renderer = new SnakeRenderer(this, _assets);
        Reset(_defaultLocation, _defaultLength, _defaultDirection);
        UpdateSegmentColors();
    }

    public void UpdateDirection(SnakeDirection direction)
    {
        var head = Segments[0];

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

    public override void Update(GameTime gameTime)
    {
        if (State != SnakeState.Alive)
            return;
        
        if (_state != null)
            _state.Update(gameTime);
        
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        var movementSize = deltaTime * GetSpeed(deltaTime);

        var head = Segments[0];
        var tail = Segments[^1];

        Head.Position = _direction.FindNextPoint(Head.Position, movementSize);

        if (_segmentsToGrow <= 0)
            Tail.Position = tail.Direction.FindNextPoint(Tail.Position, movementSize);

        // Check if partial head is still connected to body, otherwise - create new head
        if (Head.GetRectangle().Intersects(head.GetRectangle()))
            return;
    
        var newLocation = _direction.FindNextPoint(head.Position, Constants.SegmentSize);

        var newHead = new SnakeSegment
        {
            Position = newLocation,
            Direction = _nextDirection,
            Rotation = _nextDirection.ToAngle(),
            IsCorner = _nextDirection != head.Direction,
            IsClockwise = head.Direction.IsClockwise(_nextDirection)
        };
        
        Segments.Insert(0, newHead);
        
        Head = newHead.Clone();

        if (_segmentsToGrow > 0)
        {
            // Just don't remove the segment when one is needed
            _segmentsToGrow--;
        }
        else
        {
            // If head was added then tail must be removed to simulate snake movement
            Segments.Remove(tail);
            Tail = Segments[^1].Clone();
        }
        
        // Fixate direction that should be followed
        _direction = _nextDirection;

        UpdateSegmentColors();
    }

    public void Grow()
    {
        _segmentsToGrow++;
    }

    public bool CollidesWithSelf()
    {
        var headRectangle = Head.GetRectangle();

        for (var i = 1; i < Segments.Count; i++)
        {
            if (Segments[i].GetRectangle().Intersects(headRectangle))
                return true;
        }

        return false;
    }

    public bool CollidesWith(Rectangle rectangle)
    {
        if (Head.GetRectangle().Intersects(rectangle))
            return true;

        if (Tail.GetRectangle().Intersects(rectangle))
            return true;

        foreach (var segment in Segments)
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

        if (Segments.Count > 0)
        {
            _deathAnimationTimer += deltaTime;
            
            if (_deathAnimationTimer >= reduceByMs)
            {
                Segments.RemoveAt(0);
                reduced = true;

                if (Segments.Count > 0)
                {
                    Head = Segments[0].Clone();
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

    public void Faster()
    {
        _isFaster = true;
    }

    public void Slower()
    {
        _isFaster = false;
    }

    public void SpeedBoost(float seconds)
    {
        _speedTimer = seconds;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _renderer.Render(spriteBatch);
    }

    protected void ChangeState(CharacterState newState)
    {
        _state = newState;
    }
    
    private void Reset(Vector2 location, int length, SnakeDirection direction)
    {
        Segments.Clear();

        for (var i = 0; i < length; i++)
        {
            var segment = new SnakeSegment
            {
                Position = location,
                Direction = direction,
                Rotation = direction.ToAngle()
            };

            Segments.Add(segment);

            location = direction.GetOpposite().FindNextPoint(location, Constants.SegmentSize);
        }

        _direction = direction;
        _nextDirection = direction;

        Head = Segments[0].Clone();
        Tail = Segments[^1].Clone();
        
        State = SnakeState.Alive;
        _speedTimer = 0f;
        _isFaster = false;
        IsInvincible = false;
    }
    
    private float GetSpeed(float deltaTime)
    {
        var speed = (_isFaster || _speedTimer > 0f) ? Constants.IncreasedSnakeSpeed : Constants.DefaultSnakeSpeed;

        if (_speedTimer > 0f)
            _speedTimer -= deltaTime;
        
        return speed;
    }
    
    private void UpdateSegmentColors()
    {
        Head.Color = GetColor(Color, 0, IsInvincible);
        Tail.Color = GetColor(Color, Segments.Count - 1, IsInvincible);
        
        for (var i = 0; i < Segments.Count; i++)
        {
            Segments[i].Color = GetColor(Color, i, IsInvincible);
        }
    }
    
    private static Color GetColor(Color color, int index, bool isInvincible)
    {
        return isInvincible 
            ? GetInvincibleColor()
            : GetNormalStateColor(color, index);
    }

    private static Color GetNormalStateColor(Color color, int index)
    {
        var r = MathHelper.Clamp(color.R + 5*index, 0, 255);
        var g = MathHelper.Clamp(color.G + 5*index, 0, 255);
        var b = MathHelper.Clamp(color.B + 5*index, 0, 255);

        return new Color(r, g, b);
    }

    private static Color GetInvincibleColor()
    {
        var i = Random.Next(0, InvincibleColors.Length);
        return InvincibleColors[i];
    }
}
