using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using SnakeGame.DesktopGL.Core.Screens;

namespace SnakeGame.DesktopGL.Core.Commands;

public class GlobalCommands(SnakeGame game, ScreenManager screenManager)
{
    private class QuitCommand(Game game) : ICommand
    {
        public void Execute()
        {
            game.Exit();
        }
    }
    
    private class OpenPlayScreenCommand(SnakeGame game, ScreenManager screenManager) : ICommand
    {
        public void Execute()
        {
            screenManager.LoadScreen(new PlayScreen(game));
        }
    }

    private class FullScreenCommand(SnakeGame game) : ICommand
    {
        public void Execute()
        {
            game.Graphics.ToggleFullScreen();
        }
    }

    public ICommand Quit => new QuitCommand(game);
    public ICommand OpenPlayScreen => new OpenPlayScreenCommand(game, screenManager);
    public ICommand FullScreen => new FullScreenCommand(game);
}