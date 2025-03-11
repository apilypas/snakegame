using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Core.Commands;
using SnakeGame.Core.Events;
using SnakeGame.Core.Forms;
using SnakeGame.Core.Renderers;

namespace SnakeGame.Core.Screens;

public class PlayScreen(Game game) : ScreenBase(game), IObserver
{
    private ScoreBoard _scoreBoard;
    private VirtualGamePad _virtualGamePad;
    
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
        _forms = new FormsManager(this, _inputs);
        _globalCommands = new GlobalCommands(Game, ScreenManager);
        _playScreenCommands = new PlayScreenCommands(_gameWorld);
        _playScreenForms = new PlayScreenForms(_playScreenCommands, _globalCommands);
        _virtualGamePad = new VirtualGamePad(this, _inputs);
        _inputs.GamePad.VirtualGamePad = _virtualGamePad;
        
        _gameWorld.EventManager.AddObserver(this);
        _gameWorld.EventManager.AddObserver(_scoreBoard);

        AddRenderer(new PlayFieldRenderer());
        AddRenderer(new SnakeRenderer(_gameWorld.Snakes));
        AddRenderer(new CollectableRenderer(_gameWorld));
        AddRenderer(new ScoreBoardRenderer(_scoreBoard));
        AddRenderer(new FormsRenderer(_forms));
        AddRenderer(new VirtualGamePadRenderer(_virtualGamePad));
        
        _forms.Add(_playScreenForms.Pause);
        _forms.Add(_playScreenForms.GameOver);
        
        _inputs.Keyboard.BindKeyDown(Keys.Up, _playScreenCommands.MoveUp);
        _inputs.Keyboard.BindKeyDown(Keys.Left, _playScreenCommands.MoveLeft);
        _inputs.Keyboard.BindKeyDown(Keys.Right, _playScreenCommands.MoveRight);
        _inputs.Keyboard.BindKeyDown(Keys.Down, _playScreenCommands.MoveDown);
        _inputs.Keyboard.BindKeyDown(Keys.W, _playScreenCommands.MoveUp);
        _inputs.Keyboard.BindKeyDown(Keys.A, _playScreenCommands.MoveLeft);
        _inputs.Keyboard.BindKeyDown(Keys.D, _playScreenCommands.MoveRight);
        _inputs.Keyboard.BindKeyDown(Keys.S, _playScreenCommands.MoveDown);
        _inputs.Keyboard.BindKeyPressed(Keys.Space, _playScreenCommands.SpeedUp);
        _inputs.Keyboard.BindKeyReleased(Keys.Space, _playScreenCommands.SpeedDown);
        _inputs.Keyboard.BindKeyPressed(Keys.Escape, _playScreenCommands.Pause);
        _inputs.Keyboard.BindKeyPressed(Keys.Q, _globalCommands.Quit);
        _inputs.Keyboard.BindKeyPressed(Keys.F, _globalCommands.FullScreen);
        _inputs.GamePad.BindButtonDown(Buttons.DPadUp, _playScreenCommands.MoveUp);
        _inputs.GamePad.BindButtonDown(Buttons.DPadLeft, _playScreenCommands.MoveLeft);
        _inputs.GamePad.BindButtonDown(Buttons.DPadRight, _playScreenCommands.MoveRight);
        _inputs.GamePad.BindButtonDown(Buttons.DPadDown, _playScreenCommands.MoveDown);
        _inputs.GamePad.BindButtonDown(Buttons.LeftThumbstickUp, _playScreenCommands.MoveUp);
        _inputs.GamePad.BindButtonDown(Buttons.LeftThumbstickLeft, _playScreenCommands.MoveLeft);
        _inputs.GamePad.BindButtonDown(Buttons.LeftThumbstickRight, _playScreenCommands.MoveRight);
        _inputs.GamePad.BindButtonDown(Buttons.LeftThumbstickDown, _playScreenCommands.MoveDown);
        _inputs.GamePad.BindButtonPressed(Buttons.A, _playScreenCommands.SpeedUp);
        _inputs.GamePad.BindButtonReleased(Buttons.A, _playScreenCommands.SpeedDown);
        _inputs.GamePad.BindButtonPressed(Buttons.Start, _playScreenCommands.Pause);
        _inputs.GamePad.BindButtonPressed(Buttons.Back, _globalCommands.Quit);
        
        _gameWorld.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        _inputs.Update();
        _forms.Update();
        _virtualGamePad.Update();
        
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