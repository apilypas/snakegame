using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.DesktopGL.Core.Commands;
using SnakeGame.DesktopGL.Core.Events;
using SnakeGame.DesktopGL.Core.Renderers;

namespace SnakeGame.DesktopGL.Core.Screens;

public class StartScreen(Game game) : ScreenBase(game)
{
    private InputManager _inputManager;
    private GlobalCommands _globalCommands;

    public override void Initialize()
    {
        AddRenderer(new StartScreenRenderer());
        
        _inputManager = new InputManager();
        _globalCommands = new GlobalCommands(Game, ScreenManager);
        
        _inputManager.BindKeyPressed(Keys.Q, _globalCommands.Quit);
        _inputManager.BindKeyPressed(Keys.Space, _globalCommands.OpenPlayScreen);
        _inputManager.BindLeftClick(_globalCommands.OpenPlayScreen);
    }

    public override void Update(GameTime gameTime)
    {
        _inputManager.Update();
    }
}