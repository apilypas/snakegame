using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.DesktopGL.Core.Sprites;

public class SnakeColorSprite : Sprite
{
    public SnakeColorSprite(Texture2D texture)
        : base(texture, new Rectangle(4 * Constants.SegmentSize, 0, Constants.SegmentSize, Constants.SegmentSize))
    {
    }
}
