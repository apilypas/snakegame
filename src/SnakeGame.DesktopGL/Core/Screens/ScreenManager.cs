using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.DesktopGL.Core.Screens;

public class ScreenManager
{
    private ScreenBase _currentScreen;

    private readonly ContentManager _content;
    private readonly GraphicsDevice _graphics;

    public ScreenManager(GraphicsDevice graphics, ContentManager content)
    {
        _graphics = graphics;
        _content = content;
    }

    public ScreenBase GetCurrentScreen()
    {
        return _currentScreen;
    }

    public void SwitchToPlayScreen()
    {
        _content.Unload();
        
        _currentScreen = new PlayScreen(this);
        _currentScreen.Initialize();
        _currentScreen.LoadContent(_graphics, _content);
    }

    public void Initialize()
    {
        _currentScreen = new StartScreen(this);
        _currentScreen.Initialize();
    }
}