using Microsoft.Xna.Framework;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Entities;

public class PlayerSnake(AssetManager assets, Vector2 location, int length, SnakeDirection direction) 
    : Snake(assets, location, length, direction)
{
}