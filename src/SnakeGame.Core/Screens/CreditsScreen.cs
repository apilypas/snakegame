using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Core.Commands;
using SnakeGame.Core.Forms;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Renderers;

namespace SnakeGame.Core.Screens;

public class CreditsScreen(Game game) : ScreenBase(game)
{
    private InputManager _inputs;
    private FormsManager _formManager;
    private CreditsScreenForms _forms;
    
    public GlobalCommands GlobalCommands { get; private set; }

    public override void Initialize()
    {
        _inputs = new InputManager();
        _formManager = new FormsManager(this, _inputs, null);
        GlobalCommands = new GlobalCommands(Game, ScreenManager);
        _forms = new CreditsScreenForms(this);
        
        _formManager.Add(_forms.Credits);
        
        AddRenderer(new FormsRenderer(_formManager));
        
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