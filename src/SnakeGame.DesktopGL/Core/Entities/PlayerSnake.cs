using Microsoft.Xna.Framework;

namespace SnakeGame.DesktopGL.Core.Entities;

public class PlayerSnake : Snake
{
    public PlayerSnake()
        : base(GetInitialLocation())
    {
    }

    private static Vector2 GetInitialLocation()
    {
        return new Vector2(
            Constants.WallWidth / 2f * Constants.SegmentSize,
            Constants.WallHeight / 2f * Constants.SegmentSize
        );
    }
}