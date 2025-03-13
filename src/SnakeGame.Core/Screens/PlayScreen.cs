using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Core.Commands;
using SnakeGame.Core.Events;
using SnakeGame.Core.Forms;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Renderers;

namespace SnakeGame.Core.Screens;

public class PlayScreen(Game game) : ScreenBase(game), IObserver
{
    private ScoreBoard _scoreBoard;
    private VirtualGamePad _virtualGamePad;
    
    private InputManager _inputs;
    private FormsManager _formManager;
    private PlayScreenForms _forms;

    public GameWorld GameWorld { get; private set; }
    public PlayScreenCommands Commands { get; private set; }
    public GlobalCommands GlobalCommands { get; private set; }

    public override void Initialize()
    {
        GameWorld = new GameWorld();
        _scoreBoard = new ScoreBoard();
        
        _inputs = new InputManager();
        _formManager = new FormsManager(this, _inputs);
        GlobalCommands = new GlobalCommands(Game, ScreenManager);
        Commands = new PlayScreenCommands(this);
        _forms = new PlayScreenForms(this);
        _virtualGamePad = new VirtualGamePad(this, _inputs);
        _inputs.GamePad.VirtualGamePad = _virtualGamePad;
        
        GameWorld.EventManager.AddObserver(this);
        GameWorld.EventManager.AddObserver(_scoreBoard);

        AddRenderer(new PlayFieldRenderer());
        AddRenderer(new SnakeRenderer(GameWorld.Snakes));
        AddRenderer(new CollectableRenderer(GameWorld));
        AddRenderer(new ScoreBoardRenderer(_scoreBoard));
        AddRenderer(new FormsRenderer(_formManager));
        AddRenderer(new VirtualGamePadRenderer(_virtualGamePad));
        
        _formManager.Add(_forms.Pause);
        _formManager.Add(_forms.GameOver);
        
        _inputs.Bindings.BindKeyDown(Keys.Up, Commands.MoveUp);
        _inputs.Bindings.BindKeyDown(Keys.Left, Commands.MoveLeft);
        _inputs.Bindings.BindKeyDown(Keys.Right, Commands.MoveRight);
        _inputs.Bindings.BindKeyDown(Keys.Down, Commands.MoveDown);
        _inputs.Bindings.BindKeyDown(Keys.W, Commands.MoveUp);
        _inputs.Bindings.BindKeyDown(Keys.A, Commands.MoveLeft);
        _inputs.Bindings.BindKeyDown(Keys.D, Commands.MoveRight);
        _inputs.Bindings.BindKeyDown(Keys.S, Commands.MoveDown);
        _inputs.Bindings.BindKeyPressed(Keys.Space, Commands.SpeedUp);
        _inputs.Bindings.BindKeyReleased(Keys.Space, Commands.SpeedDown);
        _inputs.Bindings.BindKeyPressed(Keys.Escape, Commands.Pause);
        _inputs.Bindings.BindKeyPressed(Keys.Q, GlobalCommands.OpenStartScreen);
        _inputs.Bindings.BindKeyPressed(Keys.F, GlobalCommands.FullScreen);
        
        _inputs.GamePad.BindButtonDown(Buttons.DPadUp, Commands.MoveUp);
        _inputs.GamePad.BindButtonDown(Buttons.DPadLeft, Commands.MoveLeft);
        _inputs.GamePad.BindButtonDown(Buttons.DPadRight, Commands.MoveRight);
        _inputs.GamePad.BindButtonDown(Buttons.DPadDown, Commands.MoveDown);
        _inputs.GamePad.BindButtonDown(Buttons.LeftThumbstickUp, Commands.MoveUp);
        _inputs.GamePad.BindButtonDown(Buttons.LeftThumbstickLeft, Commands.MoveLeft);
        _inputs.GamePad.BindButtonDown(Buttons.LeftThumbstickRight, Commands.MoveRight);
        _inputs.GamePad.BindButtonDown(Buttons.LeftThumbstickDown, Commands.MoveDown);
        _inputs.GamePad.BindButtonPressed(Buttons.A, Commands.SpeedUp);
        _inputs.GamePad.BindButtonReleased(Buttons.A, Commands.SpeedDown);
        _inputs.GamePad.BindButtonPressed(Buttons.Start, Commands.Pause);
        _inputs.GamePad.BindButtonPressed(Buttons.Back, GlobalCommands.OpenStartScreen);
        
        GameWorld.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        _inputs.Update();
        _formManager.Update();
        _virtualGamePad.Update();
        
        GameWorld.Update(gameTime);
    }

    public void Notify(NotifyEvent notifyEvent)
    {
        if (notifyEvent.EventType == NotifyEventType.GameEnded)
        {
            _formManager.Show(PlayScreenForms.GameOverFormId);
        }

        if (notifyEvent.EventType == NotifyEventType.Paused)
        {
            _formManager.Show(PlayScreenForms.PauseFormId);
        }

        if (notifyEvent.EventType == NotifyEventType.Resume)
        {
            _formManager.Close();
        }
    }
}