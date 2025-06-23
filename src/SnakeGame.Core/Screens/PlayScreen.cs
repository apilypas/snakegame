using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using NLog;
using SnakeGame.Core.Dialogs;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Enums;
using SnakeGame.Core.Events;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Renderers;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Screens;

public class PlayScreen : GameScreen
{
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    private readonly VirtualGamePadManager _virtualGamePadManager;
    private readonly DialogManager _dialogs;
    private readonly RenderSystem _renderer;
    private int _lastScoreBoardEntryId;

    public GameManager GameManager { get; }
    public InputManager Inputs { get; }

    public PlayScreen(Game game) : base(game)
    {
        var assets = new AssetManager();
        assets.LoadContent(Content);
        
        GameManager = new GameManager(assets);

        Inputs = new InputManager(GameManager.World);
        Inputs.BindKey(InputActions.Up, Keys.W);
        Inputs.BindKey(InputActions.Up, Keys.Up);
        Inputs.BindKey(InputActions.Down, Keys.S);
        Inputs.BindKey(InputActions.Down, Keys.Down);
        Inputs.BindKey(InputActions.Left, Keys.A);
        Inputs.BindKey(InputActions.Left, Keys.Left);
        Inputs.BindKey(InputActions.Right, Keys.D);
        Inputs.BindKey(InputActions.Right, Keys.Right);
        Inputs.BindKey(InputActions.Faster, Keys.Space);
        Inputs.BindKey(InputActions.Pause, Keys.Escape);
        Inputs.BindKey(InputActions.Fullscreen, Keys.LeftAlt, Keys.Enter);
        Inputs.BindKey(InputActions.Fullscreen, Keys.RightAlt, Keys.Enter);
        Inputs.Apply();

        var virtualGamePad = new VirtualGamePad(assets);
        _dialogs = new DialogManager(Inputs);
        _dialogs.AddDialog(new PauseDialog(this, GameManager.World));
        _dialogs.AddDialog(new GameOverDialog(this, GameManager.World));
        _dialogs.AddDialog(new ScoreBoardDialog(GameManager.World));
        
        _virtualGamePadManager = new VirtualGamePadManager(Inputs);
        Inputs.GamePad.AttachVirtualGamePad(_virtualGamePadManager);
        
        GameManager.EventBus.Subscribe<PausedEvent>(OnPaused);
        GameManager.EventBus.Subscribe<ResumeEvent>(OnResume);
        GameManager.EventBus.Subscribe<GameEndedEvent>(OnGameEnded);

        _renderer = new RenderSystem(GraphicsDevice, Inputs);
        _renderer.Add(new EntityRenderer(GameManager.World));
        _renderer.Add(new VirtualGamePadRenderer(_virtualGamePadManager, virtualGamePad));
        
        var theme = new ThemeManager(assets);
        theme.Apply(GameManager.World);
        
        _ = new SoundManager(assets, GameManager.EventBus);

        GameManager.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        _virtualGamePadManager.Update();
        
        Inputs.Update();

        if (Inputs.IsActionDown(InputActions.Up))
            GameManager.ChangeDirection(SnakeDirection.Up);
        
        if (Inputs.IsActionDown(InputActions.Down))
            GameManager.ChangeDirection(SnakeDirection.Down);
        
        if (Inputs.IsActionDown(InputActions.Left))
            GameManager.ChangeDirection(SnakeDirection.Left);
        
        if (Inputs.IsActionDown(InputActions.Right))
            GameManager.ChangeDirection(SnakeDirection.Right);
        
        if (Inputs.IsActionDown(InputActions.Faster))
            GameManager.Faster();
        
        if (Inputs.IsActionReleased(InputActions.Faster))
            GameManager.Slower();

        if (Inputs.IsActionPressed(InputActions.Pause))
        {
            if (GameManager.IsEnded)
                _dialogs.HideCurrent();
            else
                GameManager.TogglePause();
        }

        if (Inputs.IsActionPressed(InputActions.Fullscreen))
            Services.GetService<GraphicsDeviceManager>().ToggleFullScreen();
        
        GameManager.Update(gameTime);
        
        _renderer.Update();
    }

    public override void Draw(GameTime gameTime)
    {
        _renderer.Render(gameTime);
    }

    public void TogglePause()
    {
        GameManager.TogglePause();
    }

    public void ShowScoreBoardDialog()
    {
        _dialogs.Show<ScoreBoardDialog>(_lastScoreBoardEntryId);
    }

    private void OnPaused(PausedEvent e)
    {
        _logger.Info("Paused");
        _dialogs.Show<PauseDialog>();
    }

    private void OnGameEnded(GameEndedEvent e)
    {
        _logger.Info("Game ended");
        
        _dialogs.Show<GameOverDialog>();
        
        var dataManager = new DataManager();
        _lastScoreBoardEntryId = dataManager.SaveScore(GameManager.Score, (int)GameManager.TotalTime);
    }

    private void OnResume(ResumeEvent obj)
    {
        _logger.Info("Resumed");
        _dialogs.Hide<PauseDialog>();
    }
}