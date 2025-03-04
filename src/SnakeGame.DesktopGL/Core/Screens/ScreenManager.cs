using Microsoft.Xna.Framework;

namespace SnakeGame.DesktopGL.Core.Screens;

public class ScreenManager
{
    private ScreenBase _currentScreen;

    private readonly Game _game;
    
    public ScreenManager(Game game)
    {
        _game = game;
    }

    public ScreenBase GetCurrentScreen()
    {
        return _currentScreen;
    }

    public void OpenPlayScreen()
    {
        _game.Content.Unload();
        
        _currentScreen = new PlayScreen(this);
        _currentScreen.Initialize();
        _currentScreen.LoadContent(_game.GraphicsDevice, _game.Content);
    }

    public void Initialize()
    {
        _currentScreen = new StartScreen(this);
        _currentScreen.Initialize();
    }

    public void ExitGame()
    {
        _game.Exit();
    }
}