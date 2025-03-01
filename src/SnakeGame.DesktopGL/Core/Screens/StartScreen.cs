using Microsoft.Xna.Framework.Input;

namespace SnakeGame.DesktopGL.Core.Screens;

public class StartScreen : ScreenBase
{
    public ScreenManager _stateManager;

    public StartScreen(ScreenManager stateManager)
    {
        _stateManager = stateManager;
        
        AddRenderer(new StartScreenRenderer());
    }

    public override void Initialize()
    {
    }

    public override void Update(float deltaTime)
    {
        var mouseState = Mouse.GetState();
        var keyboardState = Keyboard.GetState();

        if (mouseState.LeftButton == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Space))
        {
            _stateManager.SwitchToPlayScreen();
        }
    }
}