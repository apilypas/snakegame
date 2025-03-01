using Microsoft.Xna.Framework.Input;
using SnakeGame.DesktopGL.Core.Entities;
using SnakeGame.DesktopGL.Core.Renderers;

namespace SnakeGame.DesktopGL.Core.Screens;

public class PlayScreen : ScreenBase
{
    private readonly GameWorld _gameWorld;

    private KeyboardState _oldKeyboardState;

    private ScreenManager _stateManager;

    public PlayScreen(ScreenManager stateManager)
    {
        _stateManager = stateManager;
        
        _gameWorld = new GameWorld();

        AddRenderer(new PlayFieldRenderer(_gameWorld));
        AddRenderer(new SnakeRenderer(_gameWorld.Snakes));
        AddRenderer(new BugRenderer(_gameWorld));
        AddRenderer(new SpeedBugRenderer(_gameWorld));
        AddRenderer(new SnakePartRenderer(_gameWorld));
        AddRenderer(new UserInterfaceRenderer(_gameWorld));
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
            _gameWorld.TogglePause();
        
        if (keyboardState.IsKeyDown(Keys.G) && _oldKeyboardState.IsKeyUp(Keys.G))
            _gameWorld.ToggleGrid();
        
        if (keyboardState.IsKeyDown(Keys.Space) && _oldKeyboardState.IsKeyUp(Keys.Space))
            _gameWorld.SpeedUp();
        
        if (keyboardState.IsKeyUp(Keys.Space) && _oldKeyboardState.IsKeyDown(Keys.Space))
            _gameWorld.SpeedDown();

        _oldKeyboardState = keyboardState;

        _gameWorld.Update(deltaTime);
    }
}