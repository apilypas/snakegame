using Microsoft.Xna.Framework;

namespace SnakeGame.Core;

public static class Globals
{
    public static bool IsMobileDevice { get; set; }
    public static int VirtualScreenWidth { get; set; }
    public static int VirtualScreenHeight { get; set; }
    public static Vector2 ScreenScale { get; set; }
    public static Vector2 PlayFieldOffset { get; set; }
    public static Rectangle PlayFieldRectangle { get; }
    public static Vector2 SnakeSegmentOrigin { get; }

    static Globals()
    {
        PlayFieldRectangle = new(
            0,
            0,
            Constants.WallWidth * Constants.SegmentSize,
            Constants.WallHeight * Constants.SegmentSize);

        SnakeSegmentOrigin = new(Constants.SegmentSize / 2f, Constants.SegmentSize / 2f);
    }
}