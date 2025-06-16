using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
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
    
    public StartScreen(Game game) : base(game)
    {
        var assets = new AssetManager();
        assets.LoadContent(Content);
        
        _inputs = new InputManager();
        _inputs.BindKey(InputActions.Fullscreen, Keys.LeftAlt, Keys.Enter);
        _inputs.BindKey(InputActions.Fullscreen, Keys.RightAlt, Keys.Enter);
        
        _world = CreateUserInterface();
        
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
        
        _world.UpdateEntityTree(gameTime);
        
        _renderer.Update();
    }

    public override void Draw(GameTime gameTime)
    {
        _renderer.Render(gameTime);
    }
    
    private Entity CreateUserInterface()
    {
        var world = new Entity
        {
            Position = new Vector2(
                Constants.ScreenWidth / 2f - 50,
                Constants.ScreenHeight / 2f - 100)
        };

        var label = new Label
        {
            Text = "Snake Game",
            Position = new Vector2(0f, 0f)
        };
        world.AddChild(label);
            
        var startButton = new Button
        {
            Input = _inputs,
            Text = "Start",
            Position = new Vector2(0, 40),
            Size = new SizeF(100, 40)
        };

        startButton.OnClick += () =>
        {
            ScreenManager.LoadScreen(new PlayScreen(Game));
        };

        world.AddChild(startButton);
        
        var creditsButton = new Button
        {
            Input = _inputs,
            Text = "Credits",
            Position = new Vector2(0, 90),
            Size = new SizeF(100, 40)
        };

        creditsButton.OnClick += () =>
        {
            ScreenManager.LoadScreen(new CreditsScreen(Game));
        };

        world.AddChild(creditsButton);
        
        var quitButton = new Button
        {
            Input = _inputs,
            Text = "Quit",
            Position = new Vector2(0, 140f),
            Size = new SizeF(100, 40)
        };

        quitButton.OnClick += () =>
        {
            Game.Exit();
        };

        world.AddChild(quitButton);

        return world;
    }
}