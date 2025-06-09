using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using SnakeGame.Core.Commands;
using SnakeGame.Core.Forms;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Renderers;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Screens;

public class StartScreen : GameScreen
{
    private readonly InputManager _inputs;
    private readonly FormsManager _formManager;
    private readonly StartScreenForms _forms;
    private readonly AssetManager _assets;
    private readonly RenderSystem _renderer;

    public GlobalCommands GlobalCommands { get; }
    
    public StartScreen(Game game, ScreenManager screenManager) : base(game)
    {
        _assets = new AssetManager();
        _assets.LoadContent(Content);
        
        _inputs = new InputManager();
        _formManager = new FormsManager(_inputs, null);
        GlobalCommands = new GlobalCommands(Game, screenManager);
        _forms = new StartScreenForms(this);
        
        _formManager.Add(_forms.MainMenu);
        
        _renderer = new RenderSystem(GraphicsDevice);
        
        _renderer.Add(new StartScreenRenderer());
        _renderer.Add(new FormsRenderer(_assets, _formManager));
        
        _inputs.Bindings.BindKeyboardKeyPressed(Keys.F, GlobalCommands.FullScreen);
        
        _inputs.Bindings.BindGamePadButtonPressed(Buttons.Start, GlobalCommands.OpenPlayScreen);
        
        _formManager.Show(StartScreenForms.MainMenuFormId);
    }

    public override void Update(GameTime gameTime)
    {
        _inputs.Update();
        _formManager.Update();
        
        _renderer.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        _renderer.Render(gameTime);
    }
}