using Microsoft.Xna.Framework;

namespace SnakeGame.DesktopGL.Core.Entities;

public class PlayerSnake(Vector2 location, int length, SnakeDirection direction) 
    : Snake(location, length, direction)
{
}