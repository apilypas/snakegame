using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Enums;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.StateMachines;

public class EnemySnakeState : CharacterState
{
    private readonly Entity _snakeEntity;
    private readonly GameState _gameState;

    private enum ObjectType
    {
        Empty,
        Collectable,
        Unavoidable
    }

    private const int ObjectScanLength = 10;
    
    public EnemySnakeState(GameState gameState, Entity snakeEntity)
    {
        _gameState = gameState;
        _snakeEntity = snakeEntity;
    }
    
    public override void Update(GameTime gameTime)
    {
        var snake = _snakeEntity.Get<SnakeComponent>();
        
        if (!snake.IsAlive)
            return;

        var direction = GetDirection();

        snake.NewDirection = direction;
    }

    private SnakeDirection GetDirection()
    {
        var snake = _snakeEntity.Get<SnakeComponent>();
        
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
        foreach (var snakeEntity in _gameState.Snakes)
        {
            var snake = snakeEntity.Get<SnakeComponent>();
            
            if (!snake.IsInitialized) continue;
            
            if (CollidesWith(snake, headRectangle))
            {
                return ObjectType.Unavoidable;
            }
        }

        // Collectables
        foreach (var collectableEntity in _gameState.Collectables)
        {
            if (headRectangle.Contains(collectableEntity.Get<TransformComponent>().Position))
                return ObjectType.Collectable;
        }

        return ObjectType.Empty;
    }

    private Vector2 GetNextMove(Vector2 location, SnakeDirection direction)
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
