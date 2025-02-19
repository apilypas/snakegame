using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SnakeGame.DesktopGL.Core;

public class StartState : IState
{
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;

    public StateManager _stateManager;

    public StartState(StateManager stateManager)
    {
        _stateManager = stateManager;
    }

    public void Draw(GraphicsDevice graphicsDevice, GameTime gameTime)
    {
        graphicsDevice.Clear(new Color(0x45, 0x45, 0x45));

        _spriteBatch.Begin();

        var text = $"Click anywhere to start";
        _spriteBatch.DrawString(
            _font,
            text,
            new Vector2(graphicsDevice.Viewport.Width / 2f, graphicsDevice.Viewport.Height / 2f),
            Color.White,
            0,
            _font.MeasureString(text) / 2,
            1f,
            SpriteEffects.None,
            0f);

        _spriteBatch.End();
    }

    public void Initialize()
    {
    }

    public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _spriteBatch = new SpriteBatch(graphicsDevice);

        _font = content.Load<SpriteFont>("font1");
    }

    public void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();

        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            _stateManager.SwitchToPlayState();
        }
    }
}