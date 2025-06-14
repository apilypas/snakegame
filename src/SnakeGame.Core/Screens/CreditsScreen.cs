using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using SnakeGame.Core.Entities;
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
        
        _inputs = new InputManager();
        
        _world = CreateUserInterface();
        
        _renderer = new RenderSystem(GraphicsDevice);
        
        _renderer.Add(new EntityRenderer(_world));
        
        var theme = new ThemeManager(assets);
        theme.Apply(_world);
    }

    public override void Update(GameTime gameTime)
    {
        _inputs.Update();
        _world.UpdateEntityTree(gameTime);
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
                GraphicsDevice.Viewport.Width / 2f - 140,
                GraphicsDevice.Viewport.Height / 2f - 100)
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
            Input = _inputs,
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