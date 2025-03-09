using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using SnakeGame.Core;
using SnakeGame.Core.Screens;

namespace SnakeGame.DesktopGL;

public class SnakeGame : Game
{
    private ScreenManager _screenManager;
    
    public GraphicsDeviceManager Graphics { get; }

    public SnakeGame()
    {
        Graphics = new GraphicsDeviceManager(this);
        Graphics.PreferredBackBufferWidth = Constants.ScreenWidth;
        Graphics.PreferredBackBufferHeight = Constants.ScreenHeight;
        
        Window.Title = "Snake Game";
        
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
