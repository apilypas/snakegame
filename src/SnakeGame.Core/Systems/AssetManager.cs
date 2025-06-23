using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Core.Systems;

public class AssetManager
{
    public Texture2D CollectableTexture { get; private set; }
    public Texture2D GamePadTexture { get; private set; }
    public Texture2D SnakeTexture { get; private set; }
    public Texture2D UserInterfaceTexture { get; private set; }
    public Texture2D TilesTexture { get; private set; }
    public SpriteFont SmallFont { get; private set; }
    public SpriteFont MainFont { get; private set; }
    public SpriteFont BigFont { get; private set; }
    public SoundEffect Sound1 { get; private set; }
    public SoundEffect Sound2 { get; private set; }
    public SoundEffect Sound3 { get; private set; }
    public SoundEffect Sound4 { get; private set; }
    
    public void LoadContent(ContentManager content)
    {
        CollectableTexture = content.Load<Texture2D>("Collectables");
        GamePadTexture = content.Load<Texture2D>("GamePad");
        SnakeTexture = content.Load<Texture2D>("Snake");
        UserInterfaceTexture = content.Load<Texture2D>("UserInterface");
        TilesTexture = content.Load<Texture2D>("Tiles");
        SmallFont = content.Load<SpriteFont>("SmallFont");
        MainFont = content.Load<SpriteFont>("MainFont");
        BigFont = content.Load<SpriteFont>("BigFont");
        Sound1 = content.Load<SoundEffect>("Sound1");
        Sound2 = content.Load<SoundEffect>("Sound2");
        Sound3 = content.Load<SoundEffect>("Sound3");
        Sound4 = content.Load<SoundEffect>("Sound4");
    }
}