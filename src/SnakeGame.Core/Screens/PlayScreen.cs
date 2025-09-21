using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
    private readonly GameContentManager _contents;

    public PlayScreen(Game game) : base(game)
    {
        _contents = new GameContentManager();
        _contents.LoadContent(Content);
        
        var gameState = new GameState();

        var bindings = new[]
        {
            InputBinding.FromKeys(InputActions.Up, Keys.W),
            InputBinding.FromKeys(InputActions.Up, Keys.Up),
            InputBinding.FromButton(InputActions.Up, Buttons.DPadUp),
            InputBinding.FromButton(InputActions.Up, Buttons.LeftThumbstickUp),
            InputBinding.FromKeys(InputActions.Down, Keys.S),
            InputBinding.FromKeys(InputActions.Down, Keys.Down),
            InputBinding.FromButton(InputActions.Down, Buttons.DPadDown),
            InputBinding.FromButton(InputActions.Down, Buttons.LeftThumbstickDown),
            InputBinding.FromKeys(InputActions.Left, Keys.A),
            InputBinding.FromKeys(InputActions.Left, Keys.Left),
            InputBinding.FromButton(InputActions.Left, Buttons.DPadLeft),
            InputBinding.FromButton(InputActions.Left, Buttons.LeftThumbstickLeft),
            InputBinding.FromKeys(InputActions.Right, Keys.D),
            InputBinding.FromKeys(InputActions.Right, Keys.Right),
            InputBinding.FromButton(InputActions.Right, Buttons.DPadRight),
            InputBinding.FromButton(InputActions.Right, Buttons.LeftThumbstickRight),
            InputBinding.FromKeys(InputActions.Faster, Keys.J),
            InputBinding.FromButton(InputActions.Faster, Buttons.A),
            InputBinding.FromKeys(InputActions.Pause, Keys.Escape),
            InputBinding.FromKeys(InputActions.Select, Keys.Space),
            InputBinding.FromButton(InputActions.Select, Buttons.B),
            InputBinding.FromButton(InputActions.Pause, Buttons.Start),
            InputBinding.FromKeys(InputActions.Fullscreen, Keys.LeftAlt, Keys.Enter),
            InputBinding.FromKeys(InputActions.Fullscreen, Keys.RightAlt, Keys.Enter)
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
            .AddSystem(new CollisionEventSystem(gameState, entityFactory))
            .AddSystem(new SnakeColorSystem())
            .AddSystem(new SpawnSystem(gameState, entityFactory))
            .AddSystem(new SnakeDespawnSystem(gameState, entityFactory))
            .AddSystem(new ScreenShakeSystem())
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
            .AddSystem(new SoundEffectSystem(_contents))
            .AddSystem(new LevelSystem(gameState, entityFactory))
            .AddSystem(new LevelBonusSystem(gameState, entityFactory))
            .AddSystem(new RenderSystem(Game.GraphicsDevice, _contents, Game.Window))
            .Build();
        
        entityFactory.Initialize(_world, _contents);

        entityFactory.World.CreatePlayField();
        
        var playerAt = new Vector2(7f * Constants.SegmentSize, 10f * Constants.SegmentSize);
        
        entityFactory.World.CreatePlayerSnake(playerAt, Constants.InitialSnakeSize, SnakeDirection.Up);
        
        var enemyAt = new Vector2(23f * Constants.SegmentSize, 10f * Constants.SegmentSize);
        
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

    public override void LoadContent()
    {
        base.LoadContent();
        
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = .3f;
        MediaPlayer.Play(_contents.MainTrackSong);
    }

    public override void UnloadContent()
    {
        base.UnloadContent();
        
        MediaPlayer.Stop();
    }
}