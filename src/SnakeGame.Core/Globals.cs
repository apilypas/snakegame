using Microsoft.Xna.Framework;

namespace SnakeGame.Core;

public static class Globals
{
    public static Rectangle PlayFieldRectangle { get; }
    public static Vector2 SnakeSegmentOrigin { get; }
    public static Matrix PlayFieldCenterViewTransform { get; }

    static Globals()
    {
        PlayFieldRectangle = new Rectangle(
            0,
            0,
            Constants.WallWidth * Constants.SegmentSize,
            Constants.WallHeight * Constants.SegmentSize);
        
        SnakeSegmentOrigin = new Vector2(Constants.SegmentSize / 2f, Constants.SegmentSize / 2f);

        PlayFieldCenterViewTransform = Matrix.CreateTranslation(new Vector3(
            new Vector2(
                Constants.VirtualScreenWidth / 2f - Constants.WallWidth * Constants.SegmentSize / 2f,
                Constants.VirtualScreenHeight / 2f - Constants.WallHeight * Constants.SegmentSize / 2f),
            0.0f));
    }
}