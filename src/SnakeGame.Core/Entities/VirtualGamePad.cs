using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Entities;

public class VirtualGamePad
{
    public Sprite ButtonSprite { get; private set; }
    public Sprite BigButtonSprite { get; private set; }
    public Sprite PressedButtonSprite { get; private set; }
    public Sprite PressedBigButtonSprite { get; private set; }
    public Sprite UpArrowSprite { get; private set; }
    public Sprite DownArrowSprite { get; private set; }
    public Sprite RightArrowSprite { get; private set; }
    public Sprite LeftArrowSprite { get; private set; }
    public Sprite ActionSprite { get; private set; }
    public Sprite PauseSprite { get; private set; }

    public VirtualGamePad(AssetManager assets)
    {
        var texture = assets.GamePadTexture;
        ButtonSprite = new Sprite(new Texture2DRegion(texture, new Rectangle(0, 0, 64, 64)));
        BigButtonSprite = new Sprite(new Texture2DRegion(texture, new Rectangle(0, 96, 96, 96)));
        PressedButtonSprite = new Sprite(new Texture2DRegion(texture, new Rectangle(64, 0, 64, 64)));
        PressedBigButtonSprite = new Sprite(new Texture2DRegion(texture, new Rectangle(96, 96, 96, 96)));
        UpArrowSprite = new Sprite(new Texture2DRegion(texture, new Rectangle(0, 64, 32, 32)));
        DownArrowSprite = new Sprite(new Texture2DRegion(texture, new Rectangle(32, 64, 32, 32)));
        RightArrowSprite = new Sprite(new Texture2DRegion(texture, new Rectangle(64, 64, 32, 32)));
        LeftArrowSprite = new Sprite(new Texture2DRegion(texture, new Rectangle(96, 64, 32, 32)));
        ActionSprite = new Sprite(new Texture2DRegion(texture, new Rectangle(128, 64, 32, 32)));
        PauseSprite = new Sprite(new Texture2DRegion(texture, new Rectangle(160, 64, 32, 32)));
    }
}