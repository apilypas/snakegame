using SnakeGame.Core.Entities;
using SnakeGame.Core.Screens;

namespace SnakeGame.Core.Commands;

public class PlayScreenCommands(PlayScreen playScreen)
{
    private class MoveUpCommand(PlayScreen playScreen) : ICommand
    {
        public void Execute()
        {
            playScreen.ChangeDirection(SnakeDirection.Up);
        }
    }

    private class MoveLeftCommand(PlayScreen playScreen) : ICommand
    {
        public void Execute()
        {
            playScreen.ChangeDirection(SnakeDirection.Left);
        }
    }

    private class MoveRightCommand(PlayScreen playScreen) : ICommand
    {
        public void Execute()
        {
            playScreen.ChangeDirection(SnakeDirection.Right);
        }
    }

    private class MoveDownCommand(PlayScreen playScreen) : ICommand
    {
        public void Execute()
        {
            playScreen.ChangeDirection(SnakeDirection.Down);
        }
    }
    
    private class SpeedUpCommand(PlayScreen playScreen) : ICommand
    {
        public void Execute()
        {
            playScreen.SpeedUp();
        }
    }
    
    private class SpeedDownCommand(PlayScreen playScreen) : ICommand
    {
        public void Execute()
        {
            playScreen.SpeedDown();
        }
    }
    
    private class PauseCommand(PlayScreen playScreen) : ICommand
    {
        public void Execute()
        {
            playScreen.TogglePause();
        }
    }

    public ICommand MoveUp { get; } = new MoveUpCommand(playScreen);
    public ICommand MoveLeft { get; } = new MoveLeftCommand(playScreen);
    public ICommand MoveRight { get; } = new MoveRightCommand(playScreen);
    public ICommand MoveDown { get; } = new MoveDownCommand(playScreen);
    public ICommand SpeedUp { get; } = new SpeedUpCommand(playScreen);
    public ICommand SpeedDown { get; } = new SpeedDownCommand(playScreen);
    public ICommand Pause { get; } = new PauseCommand(playScreen);
}