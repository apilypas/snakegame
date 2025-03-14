using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Core.Commands;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Renderers;

namespace SnakeGame.Core.Screens;

public class StartScreen(Game game) : ScreenBase(game)
{
    private InputManager _inputs;
    private GlobalCommands _globalCommands;

    public override void Initialize()
    {
        AddRenderer(new StartScreenRenderer());
        
        _inputs = new InputManager();
        _globalCommands = new GlobalCommands(Game, ScreenManager);
        
        _inputs.Bindings.BindKeyPressed(Keys.Q, _globalCommands.Quit);
        _inputs.Bindings.BindKeyPressed(Keys.Space, _globalCommands.OpenPlayScreen);
        _inputs.Bindings.BindKeyPressed(Keys.F, _globalCommands.FullScreen);
        
        _inputs.Bindings.BindMouseLeftClick(_globalCommands.OpenPlayScreen);
        
        _inputs.Touch.BindTouched(_globalCommands.OpenPlayScreen);
        
        _inputs.GamePad.BindButtonPressed(Buttons.Start, _globalCommands.OpenPlayScreen);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        _inputs.Update();
    }
}