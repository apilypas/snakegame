using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using SnakeGame.Core.Screens;

namespace SnakeGame.Core.Commands;

public class GlobalCommands(Game game, ScreenManager screenManager)
{
    private class QuitCommand(Game game) : ICommand
    {
        public void Execute()
        {
            game.Exit();
        }
    }
    
    private class OpenPlayScreenCommand(Game game, ScreenManager screenManager) : ICommand
    {
        public void Execute()
        {
            screenManager.LoadScreen(new PlayScreen(game));
        }
    }
    
    private class OpenStartScreenCommand(Game game, ScreenManager screenManager) : ICommand
    {
        public void Execute()
        {
            screenManager.LoadScreen(new StartScreen(game));
        }
    }

    private class FullScreenCommand(Game game) : ICommand
    {
        public void Execute()
        {
            if (game.Services.GetService<IGraphicsDeviceManager>() is GraphicsDeviceManager graphics)
                graphics.ToggleFullScreen();
        }
    }

    public ICommand Quit => new QuitCommand(game);
    public ICommand OpenPlayScreen => new OpenPlayScreenCommand(game, screenManager);
    public ICommand OpenStartScreen => new OpenStartScreenCommand(game, screenManager);
    public ICommand FullScreen => new FullScreenCommand(game);
}