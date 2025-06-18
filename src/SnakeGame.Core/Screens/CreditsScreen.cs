using System.Text;
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

public class CreditsScreen : GameScreen
{
    private readonly RenderSystem _renderer;
    private readonly Entity _world;
    private readonly InputManager _inputs;
    
    public CreditsScreen(Game game) : base(game)
    {
        var assets = new AssetManager();
        assets.LoadContent(Content);

        _world = CreateUserInterface();
        
        _inputs = new InputManager(_world);
        _inputs.BindKey(InputActions.Fullscreen, Keys.LeftAlt, Keys.Enter);
        _inputs.BindKey(InputActions.Fullscreen, Keys.RightAlt, Keys.Enter);
        _inputs.Apply();

        _renderer = new RenderSystem(GraphicsDevice, _inputs);
        
        _renderer.Add(new EntityRenderer(_world));
        
        var theme = new ThemeManager(assets);
        theme.Apply(_world);
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
                Constants.ScreenWidth / 2f - 140,
                Constants.ScreenHeight / 2f - 100)
        };
        
        var label = new Label
        {
            Text = new StringBuilder()
                .AppendLine("Yet another implementation of Snake Game")
                .AppendLine("Created by: Andrius Pilypas")
                .ToString(),
            Position = new Vector2(0f, 10f)
        };
        world.AddChild(label);
            
        var backButton = new Button
        {
            Text = "Back",
            Position = new Vector2(80, 80),
            Size = new SizeF(100, 40),
        };

        backButton.OnClick += () =>
        {
            ScreenManager.LoadScreen(new StartScreen(Game));
        };
        
        world.AddChild(backButton);

        return world;
    }
}