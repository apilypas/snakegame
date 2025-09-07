using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Enums;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.ECS.Systems;

public class EnemyStateSystem : EntityUpdateSystem
{
    private enum ObjectType
    {
        Empty,
        Collectable,
        Unavoidable
    }
    
    private const int ObjectScanLength = 10;
    private readonly GameState _gameState;

    private ComponentMapper<SnakeComponent> _snakeMapper;
    private ComponentMapper<CollectableComponent> _collectableMapper;
    private ComponentMapper<EnemyComponent> _enemyMapper;
    private ComponentMapper<TransformComponent> _transformMapper;

    private int _frames;

    public EnemyStateSystem(GameState gameState) 
        : base(Aspect.One(typeof(SnakeComponent), typeof(CollectableComponent)))
    {
        _gameState = gameState;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _snakeMapper = mapperService.GetMapper<SnakeComponent>();
        _collectableMapper = mapperService.GetMapper<CollectableComponent>();
        _enemyMapper = mapperService.GetMapper<EnemyComponent>();
        _transformMapper = mapperService.GetMapper<TransformComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        if (_gameState.IsPaused) return;

        _frames++;

        if (_frames == 5) // Limit AI frames for better performance
        {
            _frames = 0;
            
            foreach (var entityId in ActiveEntities)
            {
                var snake = _snakeMapper.Get(entityId);

                if (snake != null && _enemyMapper.Has(entityId))
                {
                    if (snake.IsAlive)
                    {
                        snake.NewDirection = GetDirection(snake);
                    }
                }
            }
        }
    }
    
    private SnakeDirection GetDirection(SnakeComponent snake)
    {
        var head = snake.Segments[0].Position;
        
        var follow = snake.Head.Direction;
        var left = follow.GetCounterClockwise();
        var right = follow.GetClockwise();
        
        var nextMove = GetNextMove(head, follow);
        
        // Check if there is an unavoidable object at front
        if (GetObjectAt(GetNextMove(nextMove, follow)) == ObjectType.Unavoidable)
        {
            var objectAtRight = GetObjectAt(GetNextMove(head, right));
            var objectAtLeft = GetObjectAt(GetNextMove(head, left));
            
            if (objectAtRight != ObjectType.Unavoidable && objectAtLeft != ObjectType.Unavoidable)
            {
                // If we can go both ways, let's make it less predictable
                return Random.Shared.Next() % 2 == 1 ? right : left;
            }
            
            return objectAtRight != ObjectType.Unavoidable ? right : left;
        }

        // Go for collectable in front
        if (GetFirstObjectAt(nextMove, follow, ObjectScanLength) == ObjectType.Collectable)
        {
            return follow;
        }

        // If there is no collectable in front, let's check on right
        if (GetFirstObjectAt(nextMove, right, ObjectScanLength) == ObjectType.Collectable)
        {
            return right;
        }

        // If there is no collectable on right, let's check on left
        if (GetFirstObjectAt(nextMove, left, ObjectScanLength) == ObjectType.Collectable)
        {
            return left;
        }

        return follow;
    }

    private ObjectType GetFirstObjectAt(Vector2 location, SnakeDirection direction, int length)
    {
        var next = GetNextMove(location, direction);
        
        for (var i = 0; i < length; i++)
        {
            var objectAt = GetObjectAt(next);
            
            if (objectAt != ObjectType.Empty)
                return objectAt;
            
            next = GetNextMove(next, direction);
        }

        return ObjectType.Empty;
    }

    private ObjectType GetObjectAt(Vector2 location)
    {
        var headRectangle = new Rectangle(
            (int)location.X,
            (int)location.Y,
            Constants.SegmentSize,
            Constants.SegmentSize);
        
        // Wall
        if (!Globals.PlayFieldRectangle.Contains(headRectangle))
        {
            return ObjectType.Unavoidable;
        }

        // Other snake
        foreach (var entityId in ActiveEntities)
        {
            var snake = _snakeMapper.Get(entityId);

            if (snake is { IsInitialized: true })
            {
                if (CollidesWith(snake, headRectangle))
                {
                    return ObjectType.Unavoidable;
                }
            }
        }

        // Collectables
        foreach (var entityId in ActiveEntities)
        {
            if (_collectableMapper.Has(entityId))
            {
                var transform = _transformMapper.Get(entityId);
                if (headRectangle.Contains(transform.Position))
                    return ObjectType.Collectable;
            }
        }

        return ObjectType.Empty;
    }

    private static Vector2 GetNextMove(Vector2 location, SnakeDirection direction)
    {
        if (direction == SnakeDirection.Right)
            return location + new Vector2(Constants.SegmentSize, 0);
        
        if (direction == SnakeDirection.Down)
            return location + new Vector2(0, Constants.SegmentSize);
        
        if (direction == SnakeDirection.Left)
            return location - new Vector2(Constants.SegmentSize, 0);
        
        if (direction == SnakeDirection.Up)
            return location - new Vector2(0, Constants.SegmentSize);
        
        return location;
    }
    
    private static bool CollidesWith(SnakeComponent snake, Rectangle rectangle)
    {
        if (snake.Head.GetRectangle().Intersects(rectangle))
            return true;

        if (snake.Tail.GetRectangle().Intersects(rectangle))
            return true;

        foreach (var segment in snake.Segments)
        {
            if (segment.GetRectangle().Intersects(rectangle))
                return true;
        }

        return false;
    }
}