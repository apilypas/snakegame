using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Core.Commands;
using SnakeGame.Core.Forms;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Renderers;

namespace SnakeGame.Core.Screens;

public class StartScreen(Game game) : ScreenBase(game)
{
    private InputManager _inputs;
    private FormsManager _formManager;
    private StartScreenForms _forms;
    
    public GlobalCommands GlobalCommands { get; private set; }

    public override void Initialize()
    {
        _inputs = new InputManager();
        _formManager = new FormsManager(this, _inputs);
        GlobalCommands = new GlobalCommands(Game, ScreenManager);
        _forms = new StartScreenForms(this);
        
        _formManager.Add(_forms.MainMenu);
        
        AddRenderer(new StartScreenRenderer());
        AddRenderer(new FormsRenderer(_formManager));
        
        _inputs.Bindings.BindKeyboardKeyPressed(Keys.F, GlobalCommands.FullScreen);
        
        _inputs.Bindings.BindGamePadButtonPressed(Buttons.Start, GlobalCommands.OpenPlayScreen);
        
        _formManager.Show(StartScreenForms.MainMenuFormId);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        _inputs.Update();
        _formManager.Update();
    }
}