using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Core.Services;

public class ContentManager
{
    public Texture2D CollectableTexture { get; private set; }
    public Texture2D SnakeTexture { get; private set; }
    public Texture2D UserInterfaceTexture { get; private set; }
    public Texture2D TilesTexture { get; private set; }
    public SpriteFont SmallFont { get; private set; }
    public SpriteFont MainFont { get; private set; }
    public SpriteFont BigFont { get; private set; }
    public SpriteFont LogoFont { get; private set; }
    public SoundEffect PickupSoundEffect { get; private set; }
    public SoundEffect HitSoundEffect { get; private set; }
    public SoundEffect GameEndSoundEffect { get; private set; }
    public SoundEffect TimerSoundEffect { get; private set; }
    public SoundEffect SpeedUpSoundEffect { get; private set; }
    
    public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
    {
        CollectableTexture = content.Load<Texture2D>("Collectables");
        SnakeTexture = content.Load<Texture2D>("Snake");
        UserInterfaceTexture = content.Load<Texture2D>("UserInterface");
        TilesTexture = content.Load<Texture2D>("Tiles");
        SmallFont = content.Load<SpriteFont>("SmallFont");
        MainFont = content.Load<SpriteFont>("MainFont");
        BigFont = content.Load<SpriteFont>("BigFont");
        LogoFont = content.Load<SpriteFont>("LogoFont");
        PickupSoundEffect = content.Load<SoundEffect>("Sounds/Pickup");
        HitSoundEffect = content.Load<SoundEffect>("Sounds/Hit");
        GameEndSoundEffect = content.Load<SoundEffect>("Sounds/GameEnd");
        TimerSoundEffect = content.Load<SoundEffect>("Sounds/Timer");
        SpeedUpSoundEffect = content.Load<SoundEffect>("Sounds/SpeedUp");
    }
}