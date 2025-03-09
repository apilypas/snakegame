using System;

namespace SnakeGame.Core.Entities;

public enum SnakeDirection
{
    Right,
    Down,
    Left,
    Up
}

public static class SnakeDirectionUtils
{
    public static SnakeDirection GetOpposite(this SnakeDirection direction)
    {
        return direction switch
        {
            SnakeDirection.Right => SnakeDirection.Left,
            SnakeDirection.Down => SnakeDirection.Up,
            SnakeDirection.Left => SnakeDirection.Right,
            SnakeDirection.Up => SnakeDirection.Down,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static SnakeDirection GetClockwise(this SnakeDirection direction)
    {
        return direction switch
        {
            SnakeDirection.Right => SnakeDirection.Down,
            SnakeDirection.Down => SnakeDirection.Left,
            SnakeDirection.Left => SnakeDirection.Up,
            SnakeDirection.Up => SnakeDirection.Right,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static SnakeDirection GetCounterClockwise(this SnakeDirection direction)
    {
        return direction switch
        {
            SnakeDirection.Right => SnakeDirection.Up,
            SnakeDirection.Up => SnakeDirection.Left,
            SnakeDirection.Left => SnakeDirection.Down,
            SnakeDirection.Down => SnakeDirection.Right,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static bool IsClockwise(this SnakeDirection direction, SnakeDirection other)
    {
        return direction.GetClockwise() == other;
    }

    public static bool IsCounterClockwise(this SnakeDirection direction, SnakeDirection other)
    {
        return direction.GetCounterClockwise() == other;
    }
}
