using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Screens;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Entities;
using SnakeGame.Core.ECS.Systems;
using SnakeGame.Core.Enums;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Services;

namespace SnakeGame.Core.Screens;

public class PlayScreen : GameScreen
{
    private readonly World _world;
    private readonly InputManager _inputs;

    public PlayScreen(Game game) : base(game)
    {
        var contents = new ContentManager();
        contents.LoadContent(Content);
        
        var gameState = new GameState();
        
        _inputs = new InputManager();
        _inputs.BindKey(InputActions.Up, Keys.W);
        _inputs.BindKey(InputActions.Up, Keys.Up);
        _inputs.BindButton(InputActions.Up, Buttons.DPadUp);
        _inputs.BindButton(InputActions.Up, Buttons.LeftThumbstickUp);
        _inputs.BindKey(InputActions.Down, Keys.S);
        _inputs.BindKey(InputActions.Down, Keys.Down);
        _inputs.BindButton(InputActions.Down, Buttons.DPadDown);
        _inputs.BindButton(InputActions.Down, Buttons.LeftThumbstickDown);
        _inputs.BindKey(InputActions.Left, Keys.A);
        _inputs.BindKey(InputActions.Left, Keys.Left);
        _inputs.BindButton(InputActions.Left, Buttons.DPadLeft);
        _inputs.BindButton(InputActions.Left, Buttons.LeftThumbstickLeft);
        _inputs.BindKey(InputActions.Right, Keys.D);
        _inputs.BindKey(InputActions.Right, Keys.Right);
        _inputs.BindButton(InputActions.Right, Buttons.DPadRight);
        _inputs.BindButton(InputActions.Right, Buttons.LeftThumbstickRight);
        _inputs.BindKey(InputActions.Faster, Keys.Space);
        _inputs.BindButton(InputActions.Faster, Buttons.A);
        _inputs.BindKey(InputActions.Pause, Keys.Escape);
        _inputs.BindButton(InputActions.Pause, Buttons.Start);
        _inputs.BindKey(InputActions.Fullscreen, Keys.LeftAlt, Keys.Enter);
        _inputs.BindKey(InputActions.Fullscreen, Keys.RightAlt, Keys.Enter);
        
        var cameraManager = new CameraManager(
            Game,
            Constants.VirtualScreenWidth,
            Constants.VirtualScreenHeight,
            Constants.Zoom);
        cameraManager.LookAt(new Vector2(
            Constants.WallWidth * Constants.SegmentSize / 2f, 
            Constants.WallHeight * Constants.SegmentSize / 2f));
        
        var entityFactory = new EntityFactory();

        _world = new WorldBuilder()
            .AddSystem(new InputSystem(_inputs, Game, entityFactory, gameState))
            .AddSystem(new PlayerInputSystem(_inputs, gameState))
            .AddSystem(new CollectableSystem(gameState, entityFactory))
            .AddSystem(new SnakeSystem(gameState))
            .AddSystem(new CollisionSystem(gameState))
            .AddSystem(new CollisionEventSystem(gameState))
            .AddSystem(new SnakeColorSystem())
            .AddSystem(new SpawnSystem(gameState, entityFactory))
            .AddSystem(new DespawnSnakeSystem(gameState, entityFactory))
            .AddSystem(new FadingTextSystem())
            .AddSystem(new PlayFieldSystem())
            .AddSystem(new GameTimerSystem(gameState, entityFactory))
            .AddSystem(new ScoreMultiplicatorSystem(gameState))
            .AddSystem(new InvincibleSystem(gameState))
            .AddSystem(new ScoreDisplaySystem(gameState))
            .AddSystem(new DialogSystem())
            .AddSystem(new ButtonSystem(Game.GraphicsDevice, _inputs))
            .AddSystem(new ButtonEventSystem(this, game, gameState, entityFactory))
            .AddSystem(new SoundEffectSystem(contents))
            .AddSystem(new RenderSystem(Game.GraphicsDevice, contents, cameraManager))
            .AddSystem(new HudRenderSystem(Game.GraphicsDevice))
            .AddSystem(new DialogRenderSystem(Game.GraphicsDevice, contents))
            .Build();
        
        entityFactory.Initialize(_world, contents);

        entityFactory.World.CreatePlayField();
        
        var playerAt = new Vector2(7f * Constants.SegmentSize, 20f * Constants.SegmentSize);
        
        var playerSnakeEntity = entityFactory.World.CreatePlayerSnake(playerAt, Constants.InitialSnakeSize, SnakeDirection.Up);
        gameState.Snakes.Add(playerSnakeEntity);
        gameState.PlayerSnake = playerSnakeEntity;
        
        var enemyAt = new Vector2(23f * Constants.SegmentSize, 20f * Constants.SegmentSize);
        
        var enemySnakeEntity = entityFactory.World.CreateEnemySnake(gameState, enemyAt, Constants.InitialSnakeSize, SnakeDirection.Up);
        
        gameState.Snakes.Add(enemySnakeEntity);
        
        entityFactory.Hud.CreateScoreDisplay();
        entityFactory.Hud.CreateKeybindsDisplay();
    }

    public override void Update(GameTime gameTime)
    {
        _inputs.Update();
        _world.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Game.GraphicsDevice.Clear(Colors.DefaultBackgroundColor);
        
        _world.Draw(gameTime);
    }
}