using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.DesktopGL.Core.Sprites;

public class SnakeFaceSprite : Sprite
{
    public SnakeFaceSprite(Texture2D texture)
        : base(texture, new Rectangle(Constants.SegmentSize, 0, Constants.SegmentSize, Constants.SegmentSize))
    {
    }
}
