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
        PlayFieldRectangle = new(
            0,
            0,
            Constants.WallWidth * Constants.SegmentSize,
            Constants.WallHeight * Constants.SegmentSize);
        
        PlayFieldOffset = new Vector2(
            (Constants.VirtualScreenWidth - PlayFieldRectangle.Width) / 2f,
            (Constants.VirtualScreenHeight - PlayFieldRectangle.Height) / 2f);

        SnakeSegmentOrigin = new(Constants.SegmentSize / 2f, Constants.SegmentSize / 2f);
    }
}