using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using SnakeGame.Core;
using SnakeGame.Core.Screens;
using SnakeGame.Core.Utils;

namespace SnakeGame.DesktopGL;

public class SnakeGame : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private ScreenManager _screenManager;

    public SnakeGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = Constants.VirtualScreenWidth;
        _graphics.PreferredBackBufferHeight = Constants.VirtualScreenHeight;
        
        Window.Title = $"Yet another Snake Game (v{VersionUtils.GetVersion()})";
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += OnClientSizeChanged;
        
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        Services.AddService(_graphics);
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
    
    private void OnClientSizeChanged(object sender, EventArgs e)
    {
        var isChanged = false;
        var width = Window.ClientBounds.Width;
        var height = Window.ClientBounds.Height;

        if (width < Constants.VirtualScreenWidth)
        {
            width = Constants.VirtualScreenWidth;
            isChanged = true;
        }
        
        if (height < Constants.VirtualScreenHeight)
        {
            height = Constants.VirtualScreenHeight;
            isChanged = true;
        }

        if (isChanged)
        {
            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;
            _graphics.ApplyChanges();
        }
    }
}
