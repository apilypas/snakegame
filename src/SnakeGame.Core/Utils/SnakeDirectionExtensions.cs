using System;
using SnakeGame.Core.Enums;

namespace SnakeGame.Core.Utils;

public static class SnakeDirectionExtensions
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
    
    public static float ToAngle(this SnakeDirection direction)
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