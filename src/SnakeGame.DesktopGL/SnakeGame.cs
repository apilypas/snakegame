using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SnakeGame.DesktopGL.Core.Screens;

namespace SnakeGame.DesktopGL;

public class SnakeGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private ScreenManager _stateManager;

    public SnakeGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1024;
        _graphics.PreferredBackBufferHeight = 768;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _stateManager = new ScreenManager(GraphicsDevice, Content);
        _stateManager.Initialize();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _stateManager.GetCurrentScreen().LoadContent(GraphicsDevice, Content);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _stateManager.GetCurrentScreen().Update(deltaTime);
    
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var screen = _stateManager.GetCurrentScreen();

        GraphicsDevice.Clear(Colors.DefaultBackgroundColor);

        _spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp
            );

        screen.Draw(GraphicsDevice, deltaTime, _spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
