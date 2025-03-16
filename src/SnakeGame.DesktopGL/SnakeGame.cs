using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using SnakeGame.Core;
using SnakeGame.Core.Screens;

namespace SnakeGame.DesktopGL;

public class SnakeGame : Game
{
    private ScreenManager _screenManager;
    private GraphicsDeviceManager _graphics;

    public SnakeGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = Constants.ScreenWidth;
        _graphics.PreferredBackBufferHeight = Constants.ScreenHeight;
        
        Window.Title = "Snake Game";
        
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _screenManager = new ScreenManager();
        _screenManager.Initialize();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _screenManager.LoadScreen(new StartScreen(this));
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        _screenManager.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _screenManager.Draw(gameTime);
        base.Draw(gameTime);
    }
}
