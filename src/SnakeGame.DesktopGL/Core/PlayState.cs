using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SnakeGame.DesktopGL.Core.Entities;
using SnakeGame.DesktopGL.Core.Renderers;

namespace SnakeGame.DesktopGL.Core;

public class PlayState
{
    private readonly UserInterfaceRenderer _userInterfaceRenderer;
    private readonly SnakeRenderer _snakeRenderer;
    private readonly BugRenderer _bugRenderer;

    private readonly GameWorld _gameWorld;

    private SpriteBatch _spriteBatch;

    public PlayState()
    {
        _gameWorld = new GameWorld();

        _userInterfaceRenderer = new UserInterfaceRenderer(_gameWorld);
        _snakeRenderer = new SnakeRenderer(_gameWorld);
        _bugRenderer = new BugRenderer(_gameWorld);
    }

    public void Initialize()
    {
        _gameWorld.Initialize();
    }

    public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _spriteBatch = new SpriteBatch(graphicsDevice);

        _userInterfaceRenderer.LoadContent(content);
        _snakeRenderer.LoadContent(content);
        _bugRenderer.LoadContent(content);
    }

    public void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            _gameWorld.Snake.ChangeDirection(SnakeDirection.Up);

        if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            _gameWorld.Snake.ChangeDirection(SnakeDirection.Down);

        if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            _gameWorld.Snake.ChangeDirection(SnakeDirection.Left);

        if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            _gameWorld.Snake.ChangeDirection(SnakeDirection.Right);

        if (keyboardState.IsKeyDown(Keys.Space))
            _gameWorld.IsPaused = true;

        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _gameWorld.Update(deltaTime);
    }

    public void Draw(GraphicsDevice graphicsDevice, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        graphicsDevice.Clear(Color.Black);

        var x = (graphicsDevice.Viewport.Width - Constants.WallWidth * Constants.SegmentSize) / 2;
        var y = (graphicsDevice.Viewport.Height - Constants.WallHeight * Constants.SegmentSize) / 2;
        var transformMatrix = Matrix.CreateTranslation(x, y, 0);

        _spriteBatch.Begin(transformMatrix: transformMatrix);
    
        _userInterfaceRenderer.Render(_spriteBatch, deltaTime);
        _snakeRenderer.Render(_spriteBatch, deltaTime);
        _bugRenderer.Render(_spriteBatch, deltaTime);
    
        _spriteBatch.End();

        _spriteBatch.Begin();

        _userInterfaceRenderer.RenderModals(_spriteBatch);
    
        _spriteBatch.End();
    }
}