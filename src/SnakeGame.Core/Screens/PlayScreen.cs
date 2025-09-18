using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Screens;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Entities;
using SnakeGame.Core.ECS.Systems;
using SnakeGame.Core.Enums;
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

        var bindings = new[]
        {
            InputBinding.Create(InputActions.Up, Keys.W),
            InputBinding.Create(InputActions.Up, Keys.Up),
            InputBinding.Create(InputActions.Up, Buttons.DPadUp),
            InputBinding.Create(InputActions.Up, Buttons.LeftThumbstickUp),
            InputBinding.Create(InputActions.Down, Keys.S),
            InputBinding.Create(InputActions.Down, Keys.Down),
            InputBinding.Create(InputActions.Down, Buttons.DPadDown),
            InputBinding.Create(InputActions.Down, Buttons.LeftThumbstickDown),
            InputBinding.Create(InputActions.Left, Keys.A),
            InputBinding.Create(InputActions.Left, Keys.Left),
            InputBinding.Create(InputActions.Left, Buttons.DPadLeft),
            InputBinding.Create(InputActions.Left, Buttons.LeftThumbstickLeft),
            InputBinding.Create(InputActions.Right, Keys.D),
            InputBinding.Create(InputActions.Right, Keys.Right),
            InputBinding.Create(InputActions.Right, Buttons.DPadRight),
            InputBinding.Create(InputActions.Right, Buttons.LeftThumbstickRight),
            InputBinding.Create(InputActions.Faster, Keys.J),
            InputBinding.Create(InputActions.Faster, Buttons.A),
            InputBinding.Create(InputActions.Pause, Keys.Escape),
            InputBinding.Create(InputActions.Select, Keys.Space),
            InputBinding.Create(InputActions.Select, Buttons.B),
            InputBinding.Create(InputActions.Pause, Buttons.Start),
            InputBinding.Create(InputActions.Fullscreen, Keys.LeftAlt, Keys.Enter),
            InputBinding.Create(InputActions.Fullscreen, Keys.RightAlt, Keys.Enter)
        };
        
        _inputs = new InputManager(bindings);
        
        var entityFactory = new EntityFactory();

        _world = new WorldBuilder()
            .AddSystem(new InputSystem(_inputs, Game, entityFactory, gameState))
            .AddSystem(new PlayerInputSystem(_inputs, gameState))
            .AddSystem(new EnemyStateSystem(gameState))
            .AddSystem(new CollectableSystem(gameState, entityFactory))
            .AddSystem(new SnakeSpeedSystem(gameState))
            .AddSystem(new SnakeMovementSystem(gameState))
            .AddSystem(new CollisionSystem(gameState))
            .AddSystem(new CollisionEventSystem(gameState))
            .AddSystem(new SnakeColorSystem())
            .AddSystem(new SpawnSystem(gameState, entityFactory))
            .AddSystem(new SnakeDespawnSystem(gameState, entityFactory))
            .AddSystem(new FadingTextSystem())
            .AddSystem(new PlayFieldSystem())
            .AddSystem(new GameTimerSystem(gameState, entityFactory))
            .AddSystem(new ScoreMultiplicatorSystem(gameState))
            .AddSystem(new InvincibleSystem(gameState))
            .AddSystem(new ScoreDisplaySystem(gameState))
            .AddSystem(new DialogSystem())
            .AddSystem(new DialogButtonFocusSystem())
            .AddSystem(new ButtonSystem(Game.GraphicsDevice, _inputs, Game.Window))
            .AddSystem(new ButtonEventSystem(this, game, gameState, entityFactory))
            .AddSystem(new SoundEffectSystem(contents))
            .AddSystem(new LevelSystem(gameState, entityFactory))
            .AddSystem(new LevelBonusSystem(gameState))
            .AddSystem(new RenderSystem(Game.GraphicsDevice, contents, Game.Window))
            .Build();
        
        entityFactory.Initialize(_world, contents);

        entityFactory.World.CreatePlayField();
        
        var playerAt = new Vector2(7f * Constants.SegmentSize, 20f * Constants.SegmentSize);
        
        entityFactory.World.CreatePlayerSnake(playerAt, Constants.InitialSnakeSize, SnakeDirection.Up);
        
        var enemyAt = new Vector2(23f * Constants.SegmentSize, 20f * Constants.SegmentSize);
        
        entityFactory.World.CreateEnemySnake(enemyAt, Constants.InitialSnakeSize, SnakeDirection.Up);
        
        entityFactory.Hud.CreateScoreDisplay();
        entityFactory.Hud.CreateKeybindsDisplay();
        entityFactory.Hud.CreateLevelDisplay();
        
        entityFactory.Dialog.CreateNavigationIntent();
    }

    public override void Update(GameTime gameTime)
    {
        _inputs.Update();
        _world.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        _world.Draw(gameTime);
    }
}