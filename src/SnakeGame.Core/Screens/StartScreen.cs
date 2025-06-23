using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using SnakeGame.Core.Dialogs;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Renderers;
using SnakeGame.Core.Systems;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.Screens;

public class StartScreen : GameScreen
{
    private readonly RenderSystem _renderer;
    private readonly Entity _world;
    private readonly InputManager _inputs;
    private readonly DialogManager _dialogs;

    public StartScreen(Game game) : base(game)
    {
        var assets = new AssetManager();
        assets.LoadContent(Content);

        _world = CreateUserInterface();
        
        _inputs = new InputManager(_world);
        _inputs.BindKey(InputActions.Fullscreen, Keys.LeftAlt, Keys.Enter);
        _inputs.BindKey(InputActions.Fullscreen, Keys.RightAlt, Keys.Enter);
        _inputs.BindKey(InputActions.Cancel, Keys.Escape);
        _inputs.Apply();

        _dialogs = new DialogManager(_inputs);
        _dialogs.AddDialog(new ScoreBoardDialog(_world));
        _dialogs.AddDialog(new CreditsDialog(_world));
        
        _renderer = new RenderSystem(GraphicsDevice, _inputs);
        
        _renderer.Add(new EntityRenderer(_world));
        
        var theme = new ThemeManager(assets);
        theme.Apply(_world);
        
        GC.Collect();
    }

    public override void Update(GameTime gameTime)
    {
        _inputs.Update();
        
        if (_inputs.IsActionPressed(InputActions.Fullscreen))
            Services.GetService<GraphicsDeviceManager>().ToggleFullScreen();
        
        if (_inputs.IsActionDown(InputActions.Cancel))
            _dialogs.HideCurrent();
        
        _world.UpdateEntityTree(gameTime);
        
        _renderer.Update();
    }

    public override void Draw(GameTime gameTime)
    {
        _renderer.Render(gameTime);
    }
    
    private Entity CreateUserInterface()
    {
        var world = new Entity();

        var buttons = new Entity
        {
            Position = new Vector2(
                Constants.ScreenWidth / 2f - 50,
                Constants.ScreenHeight / 2f - 100)
        };
        
        world.AddChild(buttons);

        var label = new Label
        {
            Text = "Snake Game",
            Position = new Vector2(0f, 0f)
        };
        buttons.AddChild(label);
            
        var startButton = new Button
        {
            Text = "Start",
            Position = new Vector2(0, 40),
            Size = new SizeF(120, 40)
        };

        startButton.OnClick += () =>
        {
            ScreenManager.LoadScreen(new PlayScreen(Game));
        };

        buttons.AddChild(startButton);
        
        var scoreBoardButton = new Button
        {
            Text = "Score Board",
            Position = new Vector2(0, 90),
            Size = new SizeF(120, 40)
        };

        scoreBoardButton.OnClick += () =>
        {
            _dialogs.Show<ScoreBoardDialog>();
        };

        buttons.AddChild(scoreBoardButton);
        
        var creditsButton = new Button
        {
            Text = "Credits",
            Position = new Vector2(0, 140),
            Size = new SizeF(120, 40)
        };

        creditsButton.OnClick += () =>
        {
            _dialogs.Show<CreditsDialog>();
        };

        buttons.AddChild(creditsButton);
        
        var quitButton = new Button
        {
            Text = "Quit",
            Position = new Vector2(0, 190f),
            Size = new SizeF(120, 40)
        };

        quitButton.OnClick += () =>
        {
            Game.Exit();
        };

        buttons.AddChild(quitButton);

        return world;
    }
}