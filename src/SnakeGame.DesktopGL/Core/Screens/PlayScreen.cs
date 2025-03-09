using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.DesktopGL.Core.Commands;
using SnakeGame.DesktopGL.Core.Events;
using SnakeGame.DesktopGL.Core.Forms;
using SnakeGame.DesktopGL.Core.Renderers;

namespace SnakeGame.DesktopGL.Core.Screens;

public class PlayScreen(SnakeGame game) : ScreenBase(game), IObserver
{
    private ScoreBoard _scoreBoard;
    
    private InputManager _inputs;
    private FormsManager _forms;
    private GlobalCommands _globalCommands;
    private PlayScreenCommands _playScreenCommands;
    private PlayScreenForms _playScreenForms;

    private GameWorld _gameWorld;

    public override void Initialize()
    {
        _gameWorld = new GameWorld();
        _scoreBoard = new ScoreBoard();
        
        _inputs = new InputManager(this);
        _forms = new FormsManager(_inputs);
        _globalCommands = new GlobalCommands(game, ScreenManager);
        _playScreenCommands = new PlayScreenCommands(_gameWorld);
        _playScreenForms = new PlayScreenForms(_playScreenCommands, _globalCommands);
        
        _gameWorld.EventManager.AddObserver(this);
        _gameWorld.EventManager.AddObserver(_scoreBoard);

        AddRenderer(new PlayFieldRenderer());
        AddRenderer(new SnakeRenderer(_gameWorld.Snakes));
        AddRenderer(new CollectableRenderer(_gameWorld));
        AddRenderer(new ScoreBoardRenderer(_scoreBoard));
        AddRenderer(new FormsRenderer(_forms));
        
        _forms.Add(_playScreenForms.Pause);
        _forms.Add(_playScreenForms.GameOver);
        
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
        _inputs.BindKeyPressed(Keys.F, _globalCommands.FullScreen);
        
        _gameWorld.Initialize();
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
            _forms.Show(PlayScreenForms.GameOverFormId);
        }

        if (notifyEvent.EventType == NotifyEventType.Paused)
        {
            _forms.Show(PlayScreenForms.PauseFormId);
        }
    }
}