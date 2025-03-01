using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace SnakeGame.DesktopGL.Core.Entities;

public class EnemySnakeBehavior
{
    private readonly GameWorld _gameWorld;
    private readonly Snake _snake;
    private readonly Random _random;

    private enum ObjectType
    {
        Empty,
        Collectable,
        Unavoidable
    }

    private const int ObjectScanLength = 10;
    
    public EnemySnakeBehavior(GameWorld gameWorld, Snake snake)
    {
        _gameWorld = gameWorld;
        _snake = snake;
        _random = new Random(); // Let's make it less predictable (*devil smile*)
    }

    public SnakeDirection GetDirection()
    {
        var head = _snake.Segments[0].Location;
        
        var follow = _snake.Head.Direction;
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
                return _random.Next() % 2 == 1 ? right : left;
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
        if (!_gameWorld.GetRectangle().Contains(headRectangle))
        {
            return ObjectType.Unavoidable;
        }

        // Player snake
        if (_gameWorld.PlayerSnake.Intersects(headRectangle))
        {
            return ObjectType.Unavoidable;
        }

        // Enemy snake
        if (_gameWorld.EnemySnakes.Any(x => x.Intersects(headRectangle)))
        {
            return ObjectType.Unavoidable;
        }

        // Bug
        foreach (var bug in _gameWorld.EntitySpawner.Bugs)
        {
            if (headRectangle.Contains(bug.Location))
                return ObjectType.Collectable;
        }

        // Snake pars
        foreach (var snakePart in _gameWorld.EntitySpawner.SnakeParts)
        {
            if (headRectangle.Contains(snakePart.Location))
                return ObjectType.Collectable;
        }

        // Speed bug
        foreach (var speedBug in _gameWorld.EntitySpawner.SpeedBugs)
        {
            if (headRectangle.Contains(speedBug.Location))
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
}
