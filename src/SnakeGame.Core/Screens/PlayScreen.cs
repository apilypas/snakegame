using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Core.Commands;
using SnakeGame.Core.Core.Systems;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Events;
using SnakeGame.Core.Forms;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Renderers;

namespace SnakeGame.Core.Screens;

public class PlayScreen(Game game) : ScreenBase(game), IObserver
{
    private ScoreBoard _scoreBoard;
    private VirtualGamePadManager _virtualGamePadManager;
    
    private InputManager _inputs;
    private FormsManager _formManager;
    private PlayScreenForms _forms;
    private VirtualGamePad _virtualGamePad;
    private PlayField _playField;
    private AssetManager _assets;

    public GameWorld GameWorld { get; private set; }
    public PlayScreenCommands Commands { get; private set; }
    public GlobalCommands GlobalCommands { get; private set; }

    public override void Initialize()
    {
        base.Initialize();

        _assets = new AssetManager();
        _assets.LoadContent(Content);
        
        GameWorld = new GameWorld(_assets);
        
        _scoreBoard = new ScoreBoard(_assets);
        _virtualGamePad = new VirtualGamePad(_assets);
        _inputs = new InputManager();
        GlobalCommands = new GlobalCommands(Game, ScreenManager);
        Commands = new PlayScreenCommands(this);
        _forms = new PlayScreenForms(this);
        _playField = new PlayField(_assets);
        
        _virtualGamePadManager = new VirtualGamePadManager(_inputs);
        _inputs.GamePad.AttachVirtualGamePad(_virtualGamePadManager);
        
        _formManager = new FormsManager(_inputs, _virtualGamePadManager);
        
        GameWorld.Events.AddObserver(this);
        GameWorld.Events.AddObserver(_scoreBoard);

        AddRenderer(new PlayFieldRenderer(GraphicsDevice, _playField));
        AddRenderer(new SnakeRenderer(GameWorld.Snakes));
        AddRenderer(new CollectableRenderer(GameWorld));
        AddRenderer(new FadeOutTextRenderer(GameWorld.FadeOutTexts));
        AddRenderer(new ScoreBoardRenderer(_scoreBoard));
        AddRenderer(new FormsRenderer(_assets, _formManager));
        AddRenderer(new VirtualGamePadRenderer(_virtualGamePadManager, _virtualGamePad));
        
        _formManager.Add(_forms.Pause);
        _formManager.Add(_forms.GameOver);
        
        _inputs.Bindings.BindKeyboardKeyDown(Keys.Up, Commands.MoveUp);
        _inputs.Bindings.BindKeyboardKeyDown(Keys.Left, Commands.MoveLeft);
        _inputs.Bindings.BindKeyboardKeyDown(Keys.Right, Commands.MoveRight);
        _inputs.Bindings.BindKeyboardKeyDown(Keys.Down, Commands.MoveDown);
        _inputs.Bindings.BindKeyboardKeyDown(Keys.W, Commands.MoveUp);
        _inputs.Bindings.BindKeyboardKeyDown(Keys.A, Commands.MoveLeft);
        _inputs.Bindings.BindKeyboardKeyDown(Keys.D, Commands.MoveRight);
        _inputs.Bindings.BindKeyboardKeyDown(Keys.S, Commands.MoveDown);
        _inputs.Bindings.BindKeyboardKeyPressed(Keys.Space, Commands.SpeedUp);
        _inputs.Bindings.BindKeyboardKeyReleased(Keys.Space, Commands.SpeedDown);
        _inputs.Bindings.BindKeyboardKeyPressed(Keys.Escape, Commands.Pause);
        _inputs.Bindings.BindKeyboardKeyPressed(Keys.F, GlobalCommands.FullScreen);
        
        _inputs.Bindings.BindGamePadButtonDown(Buttons.DPadUp, Commands.MoveUp);
        _inputs.Bindings.BindGamePadButtonDown(Buttons.DPadLeft, Commands.MoveLeft);
        _inputs.Bindings.BindGamePadButtonDown(Buttons.DPadRight, Commands.MoveRight);
        _inputs.Bindings.BindGamePadButtonDown(Buttons.DPadDown, Commands.MoveDown);
        _inputs.Bindings.BindGamePadButtonDown(Buttons.LeftThumbstickUp, Commands.MoveUp);
        _inputs.Bindings.BindGamePadButtonDown(Buttons.LeftThumbstickLeft, Commands.MoveLeft);
        _inputs.Bindings.BindGamePadButtonDown(Buttons.LeftThumbstickRight, Commands.MoveRight);
        _inputs.Bindings.BindGamePadButtonDown(Buttons.LeftThumbstickDown, Commands.MoveDown);
        _inputs.Bindings.BindGamePadButtonPressed(Buttons.A, Commands.SpeedUp);
        _inputs.Bindings.BindGamePadButtonReleased(Buttons.A, Commands.SpeedDown);
        _inputs.Bindings.BindGamePadButtonPressed(Buttons.Start, Commands.Pause);
        _inputs.Bindings.BindGamePadButtonPressed(Buttons.Back, GlobalCommands.OpenStartScreen);
        
        GameWorld.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        _virtualGamePadManager.Update();
        _inputs.Update();
        _formManager.Update();
        
        GameWorld.Update(gameTime);
    }

    public void Notify(NotifyEvent notifyEvent)
    {
        if (notifyEvent.EventType == NotifyEventType.GameEnded)
        {
            _forms.GameOver.UpdateResultsText(
                _scoreBoard.Score,
                _scoreBoard.Deaths,
                _scoreBoard.LongestSnake);
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