using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using SnakeGame.DesktopGL.Core.Screens;

namespace SnakeGame.DesktopGL;

public class SnakeGame : Game
{
    private GraphicsDeviceManager _graphics;
    
    private ScreenManager _screenManager;

    public SnakeGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1024;
        _graphics.PreferredBackBufferHeight = 768;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _screenManager = new ScreenManager();
        _screenManager.Initialize();
        
        Components.Add(_screenManager);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _screenManager.LoadScreen(new StartScreen(this));
        
        base.LoadContent();
    }
}
