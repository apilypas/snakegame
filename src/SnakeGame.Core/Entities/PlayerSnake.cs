using Microsoft.Xna.Framework;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Entities;

public class PlayerSnake : Snake
{
    public PlayerSnake(
        AssetManager assets,
        Vector2 location,
        int length,
        SnakeDirection direction) 
        : base(assets, location, length, direction)
    {
        Color = Color.Orange;
    }
}