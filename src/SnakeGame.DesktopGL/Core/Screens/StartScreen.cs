using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SnakeGame.DesktopGL.Core.Screens;

public class StartScreen : Screen
{
    private StartScreenRenderer _startScreenRenderer;

    public ScreenManager _stateManager;

    public StartScreen(ScreenManager stateManager)
    {
        _stateManager = stateManager;
        _startScreenRenderer = new StartScreenRenderer();
    }

    public override void Draw(GraphicsDevice graphicsDevice, float deltaTime, SpriteBatch spriteBatch)
    {
        _startScreenRenderer.Render(spriteBatch, deltaTime);
    }

    public override void Initialize()
    {
    }

    public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _startScreenRenderer.LoadContent(content);
    }

    public override void Update(float deltaTime)
    {
        var mouseState = Mouse.GetState();

        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            _stateManager.SwitchToPlayScreen();
        }
    }
}