using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame;

public class BugSprite : Sprite
{
    public BugSprite(Texture2D texture) 
        : base(texture, new Rectangle(0, Constants.SegmentSize, Constants.SegmentSize, Constants.SegmentSize))
    {
    }
}
