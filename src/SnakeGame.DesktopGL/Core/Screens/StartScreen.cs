using Microsoft.Xna.Framework.Input;
using SnakeGame.DesktopGL.Core.Commands;
using SnakeGame.DesktopGL.Core.Events;
using SnakeGame.DesktopGL.Core.Renderers;

namespace SnakeGame.DesktopGL.Core.Screens;

public class StartScreen : ScreenBase
{
    private readonly InputManager _inputManager;
    private readonly GlobalCommands _globalCommands;

    public StartScreen(ScreenManager screenManager)
    {
        _inputManager = new InputManager();
        _globalCommands = new GlobalCommands(screenManager);
        
        AddRenderer(new StartScreenRenderer());
    }

    public override void Initialize()
    {
        _inputManager.BindKeyPressed(Keys.Q, _globalCommands.Quit);
        _inputManager.BindKeyPressed(Keys.Space, _globalCommands.OpenPlayScreen);
        _inputManager.BindLeftClick(_globalCommands.OpenPlayScreen);
    }

    public override void Update(float deltaTime)
    {
        _inputManager.Update();
    }
}