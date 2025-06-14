using System;
using Microsoft.Xna.Framework;
using SnakeGame.Core.Entities;
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

    public static bool IsCounterClockwise(this SnakeDirection direction, SnakeDirection other)
    {
        return direction.GetCounterClockwise() == other;
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
    
    public static Vector2 FindNextPoint(this SnakeDirection direction, Vector2 location, float size)
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