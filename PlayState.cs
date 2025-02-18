using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SnakeGame;

public class PlayState
{
    private readonly UserInterfaceRenderer _userInterfaceRenderer;
    private readonly SnakeRenderer _snakeRenderer;

    private readonly GameWorld _gameWorld;

    private Texture2D _texture;

    private SpriteBatch _spriteBatch;

    private BugSprite _bugSprite;

    public PlayState()
    {
        _gameWorld = new GameWorld();

        _userInterfaceRenderer = new UserInterfaceRenderer(_gameWorld);
        _snakeRenderer = new SnakeRenderer(_gameWorld);
    }

    public void Initialize()
    {
        _gameWorld.Initialize();
    }

    public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _spriteBatch = new SpriteBatch(graphicsDevice);

        _texture = content.Load<Texture2D>("snake");

        _bugSprite = new BugSprite(_texture);

        _userInterfaceRenderer.LoadContent(content);
        _snakeRenderer.LoadContent(content);
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
        
        _userInterfaceRenderer.Render(_spriteBatch);
        _snakeRenderer.Render(_spriteBatch);

        _bugSprite.Rotation = (_bugSprite.Rotation + deltaTime * 10f) % (MathF.PI * 2f); // TODO: remove

        foreach (var location in _gameWorld.BugSpawner.Locations)
        {
            _bugSprite.Location = location;
            _bugSprite.Draw(_spriteBatch);
        }
        
        _spriteBatch.End();

        _spriteBatch.Begin();

        _userInterfaceRenderer.RenderModals(_spriteBatch);
        
        _spriteBatch.End();
    }
}