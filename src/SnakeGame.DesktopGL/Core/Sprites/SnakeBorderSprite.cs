using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.DesktopGL.Core.Sprites;

public class SnakeBorderSprite : Sprite
{
    public SnakeBorderSprite(Texture2D texture)
        : base(texture, new Rectangle(0, 0, Constants.SegmentSize, Constants.SegmentSize))
    {
    }
}
