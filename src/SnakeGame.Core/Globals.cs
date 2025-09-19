using Microsoft.Xna.Framework;

namespace SnakeGame.Core;

public static class Globals
{
    public static Rectangle PlayFieldRectangle { get; }
    public static Vector2 SnakeSegmentOrigin { get; }
    
    static Globals()
    {
        PlayFieldRectangle = new Rectangle(
            0,
            0,
            Constants.WallWidth * Constants.SegmentSize,
            Constants.WallHeight * Constants.SegmentSize);
        
        SnakeSegmentOrigin = new Vector2(Constants.SegmentSize / 2f, Constants.SegmentSize / 2f);
   }
}