using Microsoft.Xna.Framework;

namespace SnakeGame.Core;

public static class Globals
{
    public static bool IsMobileDevice { get; set; }
    public static Vector2 PlayFieldOffset { get; set; }
    public static Rectangle PlayFieldRectangle { get; }
    public static Vector2 SnakeSegmentOrigin { get; }

    static Globals()
    {
        PlayFieldOffset = new Vector2(
            (Constants.ScreenWidth - Constants.WallWidth * Constants.SegmentSize) / 2f,
            (Constants.ScreenHeight - Constants.WallHeight * Constants.SegmentSize) / 2f);
        
        PlayFieldRectangle = new(
            0,
            0,
            Constants.WallWidth * Constants.SegmentSize,
            Constants.WallHeight * Constants.SegmentSize);

        SnakeSegmentOrigin = new(Constants.SegmentSize / 2f, Constants.SegmentSize / 2f);
    }
}