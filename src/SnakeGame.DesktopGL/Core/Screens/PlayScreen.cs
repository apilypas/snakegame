using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.DesktopGL.Core.Commands;
using SnakeGame.DesktopGL.Core.Events;
using SnakeGame.DesktopGL.Core.Forms;
using SnakeGame.DesktopGL.Core.Renderers;

namespace SnakeGame.DesktopGL.Core.Screens;

public class PlayScreen(Game game) : ScreenBase(game), IObserver
{
    private ScoreBoard _scoreBoard;
    
    private InputManager _inputs;
    private FormsManager _forms;
    private GlobalCommands _globalCommands;
    private PlayScreenCommands _playScreenCommands;

    private GameWorld _gameWorld;

    private const int PauseFormId = 1;
    private const int GameOverFormId = 2;

    public override void Initialize()
    {
        _gameWorld = new GameWorld();
        _scoreBoard = new ScoreBoard();
        
        _inputs = new InputManager();
        _forms = new FormsManager(_inputs);
        _globalCommands = new GlobalCommands(Game, ScreenManager);
        _playScreenCommands = new PlayScreenCommands(_gameWorld);
        
        _gameWorld.EventManager.AddObserver(this);
        _gameWorld.EventManager.AddObserver(_scoreBoard);

        AddRenderer(new PlayFieldRenderer());
        AddRenderer(new SnakeRenderer(_gameWorld.Snakes));
        AddRenderer(new CollectableRenderer(_gameWorld));
        AddRenderer(new ScoreBoardRenderer(_scoreBoard));
        AddRenderer(new FormsRenderer(_forms));
        
        _gameWorld.Initialize();

        var pauseForm = new Form(PauseFormId);
        pauseForm.Add(new FormText("Game is paused"));
        pauseForm.AddAction(new FormAction("Resume", Keys.Escape, _playScreenCommands.Resume));
        pauseForm.AddAction(new FormAction("Quit", Keys.Q, _globalCommands.Quit));
        _forms.Add(pauseForm);
        
        var gameOverForm = new Form(GameOverFormId);
        gameOverForm.Add(new FormText("Game is over"));
        gameOverForm.AddAction(new FormAction("Quit", Keys.Q, _globalCommands.Quit));
        _forms.Add(gameOverForm);
        
        _inputs.BindKeyDown(Keys.Up, _playScreenCommands.MoveUp);
        _inputs.BindKeyDown(Keys.Left, _playScreenCommands.MoveLeft);
        _inputs.BindKeyDown(Keys.Right, _playScreenCommands.MoveRight);
        _inputs.BindKeyDown(Keys.Down, _playScreenCommands.MoveDown);
        _inputs.BindKeyDown(Keys.W, _playScreenCommands.MoveUp);
        _inputs.BindKeyDown(Keys.A, _playScreenCommands.MoveLeft);
        _inputs.BindKeyDown(Keys.D, _playScreenCommands.MoveRight);
        _inputs.BindKeyDown(Keys.S, _playScreenCommands.MoveDown);
        _inputs.BindKeyPressed(Keys.Space, _playScreenCommands.SpeedUp);
        _inputs.BindKeyReleased(Keys.Space, _playScreenCommands.SpeedDown);
        _inputs.BindKeyPressed(Keys.Escape, _playScreenCommands.Pause);
        _inputs.BindKeyPressed(Keys.Q, _globalCommands.Quit);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        _inputs.Update();
        _forms.Update();

        _gameWorld.Update(gameTime);
    }

    public void Notify(NotifyEvent notifyEvent)
    {
        if (notifyEvent.EventType == NotifyEventType.GameEnded)
        {
            _forms.Show(GameOverFormId);
        }

        if (notifyEvent.EventType == NotifyEventType.Paused)
        {
            _forms.Show(PauseFormId);
        }
    }
}