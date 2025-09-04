using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Enums;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.ECS.Systems;

public class SnakeSystem : EntityUpdateSystem
{
    private readonly GameState _gameState;
    private ComponentMapper<SnakeComponent> _snakeMapper;

    public SnakeSystem(GameState gameState)
        : base(Aspect.All(typeof(SnakeComponent)))
    {
        _gameState = gameState;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _snakeMapper = mapperService.GetMapper<SnakeComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        if (_gameState.IsPaused)
            return;
        
        foreach (var entityId in ActiveEntities)
        {
            var snake = _snakeMapper.Get(entityId);

            if (!snake.IsInitialized)
            {
                Reset(snake, snake.DefaultLocation, snake.DefaultLength, snake.DefaultDirection);
                UpdateSegmentColors(snake);
                snake.IsAlive = true;
                snake.IsInitialized = true;
            }

            if (snake.IsAlive)
            {
                UpdateDirection(snake);

                snake.State?.Update(gameTime);

                var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                var movementSize = deltaTime * GetSpeed(snake, deltaTime);

                var head = snake.Segments[0];
                var tail = snake.Segments[^1];

                snake.Head.Position =
                    snake.Direction.FindNextPoint(snake.Head.Position, movementSize);

                if (snake.SegmentsToGrow <= 0)
                    snake.Tail.Position =
                        tail.Direction.FindNextPoint(snake.Tail.Position, movementSize);

                // Check if partial head is still connected to body, otherwise - create new head
                if (snake.Head.GetRectangle().Intersects(head.GetRectangle()))
                    continue;

                var newLocation = snake.Direction.FindNextPoint(head.Position, Constants.SegmentSize);

                var newHead = new SnakeSegment
                {
                    Position = newLocation,
                    Direction = snake.FollowingDirection,
                    Rotation = snake.FollowingDirection.ToAngle(),
                    IsCorner = snake.FollowingDirection != head.Direction,
                    IsClockwise = head.Direction.IsClockwise(snake.FollowingDirection)
                };

                snake.Segments.Insert(0, newHead);

                snake.Head = newHead.Clone();

                if (snake.SegmentsToGrow > 0)
                {
                    // Just don't remove the segment when one is needed
                    snake.SegmentsToGrow--;
                }
                else
                {
                    // If head was added then tail must be removed to simulate snake movement
                    snake.Segments.Remove(tail);
                    snake.Tail = snake.Segments[^1].Clone();
                }

                // Fixate direction that should be followed
                snake.Direction = snake.FollowingDirection;

                UpdateSegmentColors(snake);
            }
        }
    }

    private void Reset(SnakeComponent snake, Vector2 location, int length, SnakeDirection direction)
    {
        snake.Segments.Clear();

        for (var i = 0; i < length; i++)
        {
            var segment = new SnakeSegment
            {
                Position = location,
                Direction = direction,
                Rotation = direction.ToAngle()
            };

            snake.Segments.Add(segment);

            location = direction.GetOpposite().FindNextPoint(location, Constants.SegmentSize);
        }

        snake.Direction = direction;
        snake.FollowingDirection = direction;

        snake.Head = snake.Segments[0].Clone();
        snake.Tail = snake.Segments[^1].Clone();
        
        snake.IsAlive = true;
        snake.SpeedTimer = 0f;
        snake.IsFaster = false;
        snake.IsInvincible = false;
    }
    
    private void UpdateSegmentColors(SnakeComponent snake)
    {
        snake.Head.Color = GetColor(snake.Color, 0, snake.IsInvincible);
        snake.Tail.Color = GetColor(snake.Color, snake.Segments.Count - 1, snake.IsInvincible);
        
        for (var i = 0; i < snake.Segments.Count; i++)
        {
            snake.Segments[i].Color = GetColor(snake.Color, i, snake.IsInvincible);
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
        var i = Random.Shared.Next(0, Constants.InvincibleColors.Length);
        return Constants.InvincibleColors[i];
    }
    
    private void UpdateDirection(SnakeComponent snake)
    {
        var head = snake.Segments[0];
        var direction = snake.NewDirection;

        if (direction == null)
            return;

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
    
        snake.FollowingDirection = direction.Value;

        snake.NewDirection = null;
    }
    
    private float GetSpeed(SnakeComponent snake, float deltaTime)
    {
        var speed = snake.IsFaster || snake.SpeedTimer > 0f ? Constants.IncreasedSnakeSpeed : Constants.DefaultSnakeSpeed;

        if (snake.SpeedTimer > 0f)
            snake.SpeedTimer -= deltaTime;
        
        return speed;
    }
}