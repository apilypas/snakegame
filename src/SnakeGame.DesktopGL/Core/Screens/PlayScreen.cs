using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SnakeGame.DesktopGL.Core.Entities;
using SnakeGame.DesktopGL.Core.Renderers;

namespace SnakeGame.DesktopGL.Core.Screens;

public class PlayScreen : Screen
{
    private readonly UserInterfaceRenderer _userInterfaceRenderer;
    private readonly SnakeRenderer _snakeRenderer;
    private readonly BugRenderer _bugRenderer;
    private readonly SpeedBugRenderer _speedBugRenderer;
    private readonly SnakePartRenderer _snakePartRenderer;
    private readonly PlayFieldRenderer _playFieldRenderer;

    private readonly GameWorld _gameWorld;

    private KeyboardState _oldKeyboardState;

    private ScreenManager _stateManager;

    public PlayScreen(ScreenManager stateManager)
    {
        _stateManager = stateManager;
        
        _gameWorld = new GameWorld();

        _userInterfaceRenderer = new UserInterfaceRenderer(_gameWorld);
        _snakeRenderer = new SnakeRenderer(_gameWorld.Snake);
        _bugRenderer = new BugRenderer(_gameWorld);
        _speedBugRenderer = new SpeedBugRenderer(_gameWorld);
        _snakePartRenderer = new SnakePartRenderer(_gameWorld);
        _playFieldRenderer = new PlayFieldRenderer(_gameWorld);
    }

    public override void Initialize()
    {
        _gameWorld.Initialize();
    }

    public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _userInterfaceRenderer.LoadContent(content);
        _snakeRenderer.LoadContent(content);
        _bugRenderer.LoadContent(content);
        _speedBugRenderer.LoadContent(content);
        _snakePartRenderer.LoadContent(content);
        _playFieldRenderer.LoadContent(content);
    }

    public override void Update(float deltaTime)
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

        if (keyboardState.IsKeyDown(Keys.Escape) && _oldKeyboardState.IsKeyUp(Keys.Escape))
            _gameWorld.Pause();
        
        if (keyboardState.IsKeyDown(Keys.G) && _oldKeyboardState.IsKeyUp(Keys.G))
            _gameWorld.ShowGrid();
        
        if (keyboardState.IsKeyDown(Keys.Space) && _oldKeyboardState.IsKeyUp(Keys.Space))
            _gameWorld.SpeedUp();
        
        if (keyboardState.IsKeyUp(Keys.Space) && _oldKeyboardState.IsKeyDown(Keys.Space))
            _gameWorld.SpeedDown();

        _oldKeyboardState = keyboardState;

        _gameWorld.Update(deltaTime);
    }

    public override void Draw(GraphicsDevice graphicsDevice, float deltaTime, SpriteBatch spriteBatch)
    {
        var offset = GetPlayScreenOffset(graphicsDevice);

        _userInterfaceRenderer.Offset = offset;
        _userInterfaceRenderer.Render(spriteBatch, deltaTime);

        _playFieldRenderer.Offset = offset;
        _playFieldRenderer.Render(spriteBatch, deltaTime);

        _snakeRenderer.Offset = offset;
        _snakeRenderer.Render(spriteBatch, deltaTime);

        _bugRenderer.Offset = offset;
        _bugRenderer.Render(spriteBatch, deltaTime);

        _speedBugRenderer.Offset = offset;
        _speedBugRenderer.Render(spriteBatch, deltaTime);

        _snakePartRenderer.Offset = offset;
        _snakePartRenderer.Render(spriteBatch, deltaTime);

        _userInterfaceRenderer.RenderModals(spriteBatch);
    }

    private Vector2 GetPlayScreenOffset(GraphicsDevice graphicsDevice)
    {
        return new Vector2(
            (graphicsDevice.Viewport.Width - Constants.WallWidth * Constants.SegmentSize) / 2f,
            (graphicsDevice.Viewport.Height - Constants.WallHeight * Constants.SegmentSize) / 2f);
    }
}