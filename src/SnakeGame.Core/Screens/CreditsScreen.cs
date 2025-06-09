using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using SnakeGame.Core.Commands;
using SnakeGame.Core.Forms;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Renderers;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Screens;

public class CreditsScreen : ScreenBase
{
    private readonly InputManager _inputs;
    private readonly FormsManager _formManager;
    private readonly CreditsScreenForms _forms;
    private readonly AssetManager _assets;

    public GlobalCommands GlobalCommands { get; }
    
    public CreditsScreen(Game game, ScreenManager screenManager) : base(game)
    {
        _assets = new AssetManager();
        _assets.LoadContent(Content);
        _inputs = new InputManager();
        _formManager = new FormsManager(_inputs, null);
        GlobalCommands = new GlobalCommands(Game, screenManager);
        _forms = new CreditsScreenForms(this);
        
        _formManager.Add(_forms.Credits);
        
        AddRenderer(new FormsRenderer(_assets, _formManager));
        
        _inputs.Bindings.BindKeyboardKeyPressed(Keys.F, GlobalCommands.FullScreen);
        
        _inputs.Bindings.BindGamePadButtonPressed(Buttons.Start, GlobalCommands.OpenStartScreen);
        
        _formManager.Show(CreditsScreenForms.CreditsFormId);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        _inputs.Update();
        _formManager.Update();
    }
}