using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SnakeGame.DesktopGL.Core.Sprites;

namespace SnakeGame.DesktopGL.Core.Screens;

public class StartScreen : Screen
{
    private SpriteFont _font;
    private TextSprite _textSprite;

    public ScreenManager _stateManager;

    public StartScreen(ScreenManager stateManager)
    {
        _stateManager = stateManager;
    }

    public override void Draw(GraphicsDevice graphicsDevice, float deltaTime, SpriteBatch spriteBatch)
    {
        _textSprite.Location = new Vector2(graphicsDevice.Viewport.Width / 2f, graphicsDevice.Viewport.Height / 2f);
        _textSprite.Draw(spriteBatch);
    }

    public override void Initialize()
    {
    }

    public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _font = content.Load<SpriteFont>("font1");

        _textSprite = new TextSprite(_font) { Text = "Click anywhere to start" };
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