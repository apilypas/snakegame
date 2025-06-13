using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using SnakeGame.Core.Commands;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Events;
using SnakeGame.Core.Forms;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Renderers;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Screens;

public class PlayScreen : GameScreen, IObserver
{
    private readonly VirtualGamePadManager _virtualGamePadManager;
    private readonly InputManager _inputs;
    private readonly PlayScreenForms _forms;
    private readonly VirtualGamePad _virtualGamePad;
    private readonly AssetManager _assets;
    private readonly RenderSystem _renderer;
    private readonly GameManager _gameManager;
    private readonly ThemeManager _theme;

    public PlayScreenCommands Commands { get; }
    public GlobalCommands GlobalCommands { get; }

    public PlayScreen(Game game, ScreenManager screenManager) : base(game)
    {
        _assets = new AssetManager();
        _assets.LoadContent(Content);
        
        _inputs = new InputManager();
        
        _gameManager = new GameManager(_assets);
        
        _virtualGamePad = new VirtualGamePad(_assets);
        GlobalCommands = new GlobalCommands(Game, screenManager);
        Commands = new PlayScreenCommands(this);
        _forms = new PlayScreenForms(this, _gameManager.World, _inputs);
        
        _virtualGamePadManager = new VirtualGamePadManager(_inputs);
        _inputs.GamePad.AttachVirtualGamePad(_virtualGamePadManager);
        
        _gameManager.Events.AddObserver(this);

        _renderer = new RenderSystem(GraphicsDevice);
        _renderer.Add(new EntityRenderer(_gameManager.World));
        _renderer.Add(new VirtualGamePadRenderer(_virtualGamePadManager, _virtualGamePad));
        
        _theme = new ThemeManager(_assets);
        _theme.Apply(_gameManager.World);
        
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
        
        _gameManager.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        _virtualGamePadManager.Update();
        _inputs.Update();
        
        _gameManager.Update(gameTime);
        
        _renderer.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        _renderer.Render(gameTime);
    }

    public void Notify(NotifyEvent notifyEvent)
    {
        if (notifyEvent.EventType == NotifyEventType.GameEnded)
        {
            _forms.GameOver.UpdateResults(
                _gameManager.World.Score.Score,
                _gameManager.World.Score.Deaths,
                _gameManager.World.Score.LongestSnake);

            _forms.GameOver.IsVisible = true;
        }

        if (notifyEvent.EventType == NotifyEventType.Paused)
        {
            _forms.Pause.IsVisible = true;
        }

        if (notifyEvent.EventType == NotifyEventType.Resume)
        {
            if (_forms.Pause.IsVisible)
                _forms.Pause.IsVisible = false;
        }
    }

    public void ChangeDirection(SnakeDirection direction)
    {
        _gameManager.ChangeDirection(direction);
    }

    public void Faster()
    {
        _gameManager.Faster();
    }

    public void Slower()
    {
        _gameManager.Slower();
    }

    public void TogglePause()
    {
        _gameManager.TogglePause();
    }
}