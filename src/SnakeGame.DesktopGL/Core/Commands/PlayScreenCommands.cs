using SnakeGame.DesktopGL.Core.Entities;

namespace SnakeGame.DesktopGL.Core.Commands;

public class PlayScreenCommands(GameWorld gameWorld, ModalState modalState)
{
    private class MoveUpCommand(GameWorld gameWorld) : ICommand
    {
        public void Execute()
        {
            gameWorld.ChangeDirection(SnakeDirection.Up);
        }
    }

    private class MoveLeftCommand(GameWorld gameWorld) : ICommand
    {
        public void Execute()
        {
            gameWorld.ChangeDirection(SnakeDirection.Left);
        }
    }

    private class MoveRightCommand(GameWorld gameWorld) : ICommand
    {
        public void Execute()
        {
            gameWorld.ChangeDirection(SnakeDirection.Right);
        }
    }

    private class MoveDownCommand(GameWorld gameWorld) : ICommand
    {
        public void Execute()
        {
            gameWorld.ChangeDirection(SnakeDirection.Down);
        }
    }
    
    private class SpeedUpCommand(GameWorld gameWorld) : ICommand
    {
        public void Execute()
        {
            gameWorld.SpeedUp();
        }
    }
    
    private class SpeedDownCommand(GameWorld gameWorld) : ICommand
    {
        public void Execute()
        {
            gameWorld.SpeedDown();
        }
    }
    
    private class PauseCommand(ModalState modalState) : ICommand
    {
        public void Execute()
        {
            modalState.TogglePausedModal();
        }
    }

    public ICommand MoveUp { get; } = new MoveUpCommand(gameWorld);
    public ICommand MoveLeft { get; } = new MoveLeftCommand(gameWorld);
    public ICommand MoveRight { get; } = new MoveRightCommand(gameWorld);
    public ICommand MoveDown { get; } = new MoveDownCommand(gameWorld);
    public ICommand SpeedUp { get; } = new SpeedUpCommand(gameWorld);
    public ICommand SpeedDown { get; } = new SpeedDownCommand(gameWorld);
    public ICommand Pause { get; } = new PauseCommand(modalState);
}