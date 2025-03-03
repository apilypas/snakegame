using Microsoft.Xna.Framework.Input;
using SnakeGame.DesktopGL.Core.Entities;
using SnakeGame.DesktopGL.Core.Renderers;

namespace SnakeGame.DesktopGL.Core.Screens;

public class PlayScreen : ScreenBase
{
    private readonly GameWorld _gameWorld;
    private readonly ScoreBoard _scoreBoard;
    private readonly ModalState _modalState;

    private KeyboardState _oldKeyboardState;

    private ScreenManager _screenManager;

    public PlayScreen(ScreenManager screenManager)
    {
        _screenManager = screenManager;
        
        _gameWorld = new GameWorld();
        _scoreBoard = new ScoreBoard();
        _modalState = new ModalState();

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
    }

    public override void Update(float deltaTime)
    {
        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            _gameWorld.ChangeDirection(SnakeDirection.Up);

        if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            _gameWorld.ChangeDirection(SnakeDirection.Down);

        if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            _gameWorld.ChangeDirection(SnakeDirection.Left);

        if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            _gameWorld.ChangeDirection(SnakeDirection.Right);

        if (keyboardState.IsKeyDown(Keys.Escape) && _oldKeyboardState.IsKeyUp(Keys.Escape))
            _modalState.TogglePausedModal();
        
        if (keyboardState.IsKeyDown(Keys.Space) && _oldKeyboardState.IsKeyUp(Keys.Space))
            _gameWorld.SpeedUp();
        
        if (keyboardState.IsKeyUp(Keys.Space) && _oldKeyboardState.IsKeyDown(Keys.Space))
            _gameWorld.SpeedDown();

        _oldKeyboardState = keyboardState;

        if (_modalState.Type == ModalState.ModalStateType.None)
            _gameWorld.Update(deltaTime);
    }
}