using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using SnakeGame.Core.Commands;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Forms;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Renderers;
using SnakeGame.Core.Systems;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.Screens;

public class CreditsScreen : GameScreen
{
    private readonly CreditsScreenForms _forms;
    private readonly AssetManager _assets;
    private readonly RenderSystem _renderer;
    private readonly ThemeManager _theme;

    public Entity World { get; }
    public InputManager Inputs { get; }

    public GlobalCommands GlobalCommands { get; }
    
    public CreditsScreen(Game game, ScreenManager screenManager) : base(game)
    {
        _assets = new AssetManager();
        _assets.LoadContent(Content);
        
        Inputs = new InputManager();
        
        World = new Entity();
        
        GlobalCommands = new GlobalCommands(Game, screenManager);
        _forms = new CreditsScreenForms(this);
        
        _renderer = new RenderSystem(GraphicsDevice);
        
        _renderer.Add(new EntityRenderer(World));
        
        _theme = new ThemeManager(_assets);
        _theme.Apply(World);
        
        Inputs.Bindings.BindKeyboardKeyPressed(Keys.F, GlobalCommands.FullScreen);
        
        Inputs.Bindings.BindGamePadButtonPressed(Buttons.Start, GlobalCommands.OpenStartScreen);
    }

    public override void Update(GameTime gameTime)
    {
        Inputs.Update();
        World.UpdateEntityTree(gameTime);
        
        _renderer.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        _renderer.Render(gameTime);
    }
}