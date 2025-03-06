using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.DesktopGL.Core.Commands;
using SnakeGame.DesktopGL.Core.Events;
using SnakeGame.DesktopGL.Core.Renderers;

namespace SnakeGame.DesktopGL.Core.Screens;

public class PlayScreen(Game game) : ScreenBase(game)
{
    private GameWorld _gameWorld;
    private ScoreBoard _scoreBoard;
    
    private InputManager _inputManager;
    private GlobalCommands _globalCommands;
    private PlayScreenCommands _playScreenCommands;

    public override void Initialize()
    {
        _gameWorld = new GameWorld();
        _scoreBoard = new ScoreBoard();
        
        _inputManager = new InputManager();
        _globalCommands = new GlobalCommands(Game, ScreenManager);
        _playScreenCommands = new PlayScreenCommands(_gameWorld);
        
        _gameWorld.EventManager.AddObserver(_scoreBoard);

        AddRenderer(new PlayFieldRenderer());
        AddRenderer(new SnakeRenderer(_gameWorld.Snakes));
        AddRenderer(new CollectableRenderer(_gameWorld));
        AddRenderer(new ScoreBoardRenderer(_scoreBoard));
        AddRenderer(new ModalsRenderer(_gameWorld));
        
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

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        _inputManager.Update();

        _gameWorld.Update(gameTime);
    }
}