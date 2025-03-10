using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using SnakeGame.Core.Screens;

namespace SnakeGame.Android;

public class SnakeGame : Game
{
    private ScreenManager _screenManager;
    private GraphicsDeviceManager _graphics;
    
    public SnakeGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        
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