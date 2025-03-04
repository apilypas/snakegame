using Microsoft.Xna.Framework.Input;
using SnakeGame.DesktopGL.Core.Commands;
using SnakeGame.DesktopGL.Core.Events;
using SnakeGame.DesktopGL.Core.Renderers;

namespace SnakeGame.DesktopGL.Core.Screens;

public class PlayScreen : ScreenBase
{
    private readonly GameWorld _gameWorld;
    private readonly ScoreBoard _scoreBoard;
    private readonly ModalState _modalState;

    private readonly InputManager _inputManager;
    private readonly GlobalCommands _globalCommands;
    private readonly PlayScreenCommands _playScreenCommands;

    public PlayScreen(ScreenManager screenManager)
    {
        _inputManager = new InputManager();
        
        _gameWorld = new GameWorld();
        _scoreBoard = new ScoreBoard();
        _modalState = new ModalState();

        _globalCommands = new GlobalCommands(screenManager);
        _playScreenCommands = new PlayScreenCommands(_gameWorld, _modalState);

        _gameWorld.EventManager.AddObserver(_scoreBoard);

        AddRenderer(new PlayFieldRenderer(_gameWorld));
        AddRenderer(new SnakeRenderer(_gameWorld.Snakes));
        AddRenderer(new CollectableRenderer(_gameWorld));
        AddRenderer(new ScoreBoardRenderer(_scoreBoard));
        AddRenderer(new ModalsRenderer(_modalState));
    }

    public override void Initialize()
    {
        _gameWorld.Initialize();
        
        _inputManager.BindKeyDown(Keys.Up, _playScreenCommands.MoveUp);
        _inputManager.BindKeyDown(Keys.Left, _playScreenCommands.MoveLeft);
        _inputManager.BindKeyDown(Keys.Right, _playScreenCommands.MoveRight);
        _inputManager.BindKeyDown(Keys.Down, _playScreenCommands.MoveDown);
        _inputManager.BindKeyDown(Keys.W, _playScreenCommands.MoveUp);
        _inputManager.BindKeyDown(Keys.A, _playScreenCommands.MoveLeft);
        _inputManager.BindKeyDown(Keys.D, _playScreenCommands.MoveRight);
        _inputManager.BindKeyDown(Keys.S, _playScreenCommands.MoveDown);
        _inputManager.BindKeyPressed(Keys.Space, _playScreenCommands.SpeedUp);
        _inputManager.BindKeyReleased(Keys.Space, _playScreenCommands.SpeedDown);
        _inputManager.BindKeyPressed(Keys.Escape, _playScreenCommands.Pause);
        _inputManager.BindKeyPressed(Keys.Q, _globalCommands.Quit);
    }

    public override void Update(float deltaTime)
    {
        _inputManager.Update();

        if (_modalState.Type == ModalState.ModalStateType.Paused)
            return;
        
        _gameWorld.Update(deltaTime);
    }
}