using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using SnakeGame.Core;
using SnakeGame.Core.Screens;

namespace SnakeGame.DesktopGL;

public class SnakeGame : Game
{
    private ScreenManager _screenManager;

    public SnakeGame()
    {
        var graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = Constants.ScreenWidth;
        graphics.PreferredBackBufferHeight = Constants.ScreenHeight;
        
        Window.Title = "Snake Game";
        //Window.AllowUserResizing = true;
        
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        Services.AddService(graphics);
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        
        _screenManager = new ScreenManager();
        _screenManager.Initialize();
        _screenManager.LoadScreen(new StartScreen(this));
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        _screenManager.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        
        _screenManager.Draw(gameTime);
    }
}
