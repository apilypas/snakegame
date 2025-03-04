using SnakeGame.DesktopGL.Core.Screens;

namespace SnakeGame.DesktopGL.Core.Commands;

public class GlobalCommands(ScreenManager screenManager)
{
    private class QuitCommand(ScreenManager screenManager) : ICommand
    {
        public void Execute()
        {
            screenManager.ExitGame();
        }
    }
    
    private class OpenPlayScreenCommand(ScreenManager screenManager) : ICommand
    {
        public void Execute()
        {
            screenManager.OpenPlayScreen();
        }
    }

    public ICommand Quit => new QuitCommand(screenManager);
    public ICommand OpenPlayScreen => new OpenPlayScreenCommand(screenManager);
}