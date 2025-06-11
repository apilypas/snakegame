using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Entities;

public class VirtualGamePad
{
    public MonoGame.Extended.Graphics.Sprite ButtonSprite { get; private set; }
    public MonoGame.Extended.Graphics.Sprite BigButtonSprite { get; private set; }
    public MonoGame.Extended.Graphics.Sprite PressedButtonSprite { get; private set; }
    public MonoGame.Extended.Graphics.Sprite PressedBigButtonSprite { get; private set; }
    public MonoGame.Extended.Graphics.Sprite UpArrowSprite { get; private set; }
    public MonoGame.Extended.Graphics.Sprite DownArrowSprite { get; private set; }
    public MonoGame.Extended.Graphics.Sprite RightArrowSprite { get; private set; }
    public MonoGame.Extended.Graphics.Sprite LeftArrowSprite { get; private set; }
    public MonoGame.Extended.Graphics.Sprite ActionSprite { get; private set; }
    public MonoGame.Extended.Graphics.Sprite PauseSprite { get; private set; }

    public VirtualGamePad(AssetManager assets)
    {
        var texture = assets.GamePadTexture;
        ButtonSprite = new MonoGame.Extended.Graphics.Sprite(new Texture2DRegion(texture, new Rectangle(0, 0, 64, 64)));
        BigButtonSprite = new MonoGame.Extended.Graphics.Sprite(new Texture2DRegion(texture, new Rectangle(0, 96, 96, 96)));
        PressedButtonSprite = new MonoGame.Extended.Graphics.Sprite(new Texture2DRegion(texture, new Rectangle(64, 0, 64, 64)));
        PressedBigButtonSprite = new MonoGame.Extended.Graphics.Sprite(new Texture2DRegion(texture, new Rectangle(96, 96, 96, 96)));
        UpArrowSprite = new MonoGame.Extended.Graphics.Sprite(new Texture2DRegion(texture, new Rectangle(0, 64, 32, 32)));
        DownArrowSprite = new MonoGame.Extended.Graphics.Sprite(new Texture2DRegion(texture, new Rectangle(32, 64, 32, 32)));
        RightArrowSprite = new MonoGame.Extended.Graphics.Sprite(new Texture2DRegion(texture, new Rectangle(64, 64, 32, 32)));
        LeftArrowSprite = new MonoGame.Extended.Graphics.Sprite(new Texture2DRegion(texture, new Rectangle(96, 64, 32, 32)));
        ActionSprite = new MonoGame.Extended.Graphics.Sprite(new Texture2DRegion(texture, new Rectangle(128, 64, 32, 32)));
        PauseSprite = new MonoGame.Extended.Graphics.Sprite(new Texture2DRegion(texture, new Rectangle(160, 64, 32, 32)));
    }
}