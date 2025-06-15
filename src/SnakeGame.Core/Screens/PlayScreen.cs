using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Enums;
using SnakeGame.Core.Events;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Renderers;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Screens;

public class PlayScreen : GameScreen
{
    private readonly VirtualGamePadManager _virtualGamePadManager;
    private readonly DialogManager _dialogs;
    private readonly RenderSystem _renderer;
    private readonly GameManager _gameManager;

    public InputManager Inputs { get; }

    public PlayScreen(Game game) : base(game)
    {
        var assets = new AssetManager();
        assets.LoadContent(Content);
        
        Inputs = new InputManager();
        Inputs.BindKey(InputActions.Up, Keys.W, Keys.Up);
        Inputs.BindKey(InputActions.Down, Keys.S, Keys.Down);
        Inputs.BindKey(InputActions.Left, Keys.A, Keys.Left);
        Inputs.BindKey(InputActions.Right, Keys.D, Keys.Right);
        Inputs.BindKey(InputActions.Faster, Keys.Space);
        Inputs.BindKey(InputActions.Pause, Keys.Escape);
        
        _gameManager = new GameManager(assets);
        
        var virtualGamePad = new VirtualGamePad(assets);
        _dialogs = new DialogManager(this, _gameManager.World.DialogLayer);
        
        _virtualGamePadManager = new VirtualGamePadManager(Inputs);
        Inputs.GamePad.AttachVirtualGamePad(_virtualGamePadManager);
        
        _gameManager.EventBus.Subscribe<PausedEvent>(OnPaused);
        _gameManager.EventBus.Subscribe<ResumeEvent>(OnResume);
        _gameManager.EventBus.Subscribe<GameEndedEvent>(OnGameEnded);

        _renderer = new RenderSystem(GraphicsDevice);
        _renderer.Add(new EntityRenderer(_gameManager.World));
        _renderer.Add(new VirtualGamePadRenderer(_virtualGamePadManager, virtualGamePad));
        
        var theme = new ThemeManager(assets);
        theme.Apply(_gameManager.World);

        _gameManager.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        _virtualGamePadManager.Update();
        
        Inputs.Update();

        if (Inputs.IsActionDown(InputActions.Up))
            _gameManager.ChangeDirection(SnakeDirection.Up);
        
        if (Inputs.IsActionDown(InputActions.Down))
            _gameManager.ChangeDirection(SnakeDirection.Down);
        
        if (Inputs.IsActionDown(InputActions.Left))
            _gameManager.ChangeDirection(SnakeDirection.Left);
        
        if (Inputs.IsActionDown(InputActions.Right))
            _gameManager.ChangeDirection(SnakeDirection.Right);
        
        if (Inputs.IsActionDown(InputActions.Faster))
            _gameManager.Faster();
        
        if (Inputs.IsActionReleased(InputActions.Faster))
            _gameManager.Slower();
        
        if (Inputs.IsActionPressed(InputActions.Pause))
            _gameManager.TogglePause();
        
        _gameManager.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        _renderer.Render(gameTime);
    }

    public void TogglePause()
    {
        _gameManager.TogglePause();
    }
    
    private void OnPaused(PausedEvent e)
    {
        _dialogs.Pause.IsVisible = true;
    }
    
    private void OnGameEnded(GameEndedEvent e)
    {
        _dialogs.GameOver.UpdateResults(
            _gameManager.Score,
            _gameManager.Deaths,
            _gameManager.LongestSnake);

        _dialogs.GameOver.IsVisible = true;
    }
    
    private void OnResume(ResumeEvent obj)
    {
        _dialogs.Pause.IsVisible = false;
    }
}