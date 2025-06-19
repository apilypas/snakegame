using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;

namespace SnakeGame.Core.Systems;

public class AssetManager
{
    public Texture2D CollectableTexture { get; private set; }
    public Texture2D GamePadTexture { get; private set; }
    public Texture2D SnakeTexture { get; set; }
    public Texture2D UserInterfaceTexture { get; set; }
    public Texture2D TilesTexture { get; set; }
    public SpriteFont MainFont { get; private set; }
    public SpriteFont BigFont { get; set; }
    
    public void LoadContent(ContentManager content)
    {
        CollectableTexture = content.Load<Texture2D>("Collectables");
        GamePadTexture = content.Load<Texture2D>("GamePad");
        SnakeTexture = content.Load<Texture2D>("Snake");
        UserInterfaceTexture = content.Load<Texture2D>("UserInterface");
        TilesTexture = content.Load<Texture2D>("Tiles");
        MainFont = content.Load<SpriteFont>("MainFont");
        BigFont = content.Load<SpriteFont>("BigFont");
    }
}