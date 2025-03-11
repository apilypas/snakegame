using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Core.Commands;
using SnakeGame.Core.Events;
using SnakeGame.Core.Renderers;

namespace SnakeGame.Core.Screens;

public class StartScreen(Game game) : ScreenBase(game)
{
    private InputManager _inputManager;
    private GlobalCommands _globalCommands;

    public override void Initialize()
    {
        AddRenderer(new StartScreenRenderer());
        
        _inputManager = new InputManager(this);
        _globalCommands = new GlobalCommands(Game, ScreenManager);
        
        _inputManager.Keyboard.BindKeyPressed(Keys.Q, _globalCommands.Quit);
        _inputManager.Keyboard.BindKeyPressed(Keys.Space, _globalCommands.OpenPlayScreen);
        _inputManager.Keyboard.BindKeyPressed(Keys.F, _globalCommands.FullScreen);
        _inputManager.Mouse.BindLeftClick(_globalCommands.OpenPlayScreen);
        _inputManager.Touch.BindTouched(_globalCommands.OpenPlayScreen);
        _inputManager.GamePad.BindButtonPressed(Buttons.Start, _globalCommands.OpenPlayScreen);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        _inputManager.Update();
    }
}