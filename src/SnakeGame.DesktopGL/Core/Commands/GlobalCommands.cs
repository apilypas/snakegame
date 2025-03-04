using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using SnakeGame.DesktopGL.Core.Screens;

namespace SnakeGame.DesktopGL.Core.Commands;

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

    public ICommand Quit => new QuitCommand(game);
    public ICommand OpenPlayScreen => new OpenPlayScreenCommand(game, screenManager);
}