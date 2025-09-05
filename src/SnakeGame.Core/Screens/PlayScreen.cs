using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Screens;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
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
            .AddSystem(new GameSystem(contents, gameState, entityFactory))
            .AddSystem(new SnakeSystem(gameState))
            .AddSystem(new FadingTextSystem())
            .AddSystem(new PlayFieldSystem())
            .AddSystem(new ScoreDisplaySystem())
            .AddSystem(new DialogSystem())
            .AddSystem(new ButtonSystem(Game.GraphicsDevice, _inputs))
            .AddSystem(new ButtonEventSystem(gameState, entityFactory))
            .AddSystem(new SoundEffectSystem(contents))
            .AddSystem(new RenderSystem(Game.GraphicsDevice, contents, cameraManager))
            .AddSystem(new DialogRenderSystem(Game.GraphicsDevice, contents))
            .Build();
        
        entityFactory.Initialize(_world, contents);

        // TODO: remove gameStateEntity
        var gameStateEntity = _world.CreateEntity();
        gameStateEntity.Attach(gameState);

        var playFieldEntity = entityFactory.CreatePlayField(contents.TilesTexture);
        
        var playerAt = new Vector2(7f * Constants.SegmentSize, 20f * Constants.SegmentSize);
        
        var playerSnakeEntity = entityFactory.CreatePlayerSnake(playerAt, Constants.InitialSnakeSize, SnakeDirection.Up);
        gameState.Snakes.Add(playerSnakeEntity);
        gameState.PlayerSnake = playerSnakeEntity;
        
        var enemyAt = new Vector2(23f * Constants.SegmentSize, 20f * Constants.SegmentSize);
        
        var enemySnakeEntity = entityFactory.CreateEnemySnake(gameState, enemyAt, Constants.InitialSnakeSize, SnakeDirection.Up);
        
        gameState.Snakes.Add(enemySnakeEntity);

        var scoreLabelId = entityFactory.CreateLabel(contents.BigFont, string.Empty, Color.White);
        _world.GetEntity(scoreLabelId).Get<TransformComponent>().Position = new Vector2(530f, 18f);
        gameState.ScoreLabelId = scoreLabelId;
        
        var multiplicatorLabelId = entityFactory.CreateLabel(contents.MainFont, string.Empty, Colors.ScoreMultiplicatorColor);
        _world.GetEntity(multiplicatorLabelId).Get<TransformComponent>().Position = new Vector2(686f, 44f);
        gameState.MultiplicatorLabelId = multiplicatorLabelId;
        
        var timeLabelId = entityFactory.CreateLabel(contents.MainFont, string.Empty, Colors.ScoreTimeColor);
        _world.GetEntity(timeLabelId).Get<TransformComponent>().Position = new Vector2(550f, 9f);
        gameState.TimeLabelId = timeLabelId;

        var clockSpriteId = entityFactory.CreateSprite(contents.CollectableTexture, new Rectangle(16, 0, 16, 16));
        _world.GetEntity(clockSpriteId).Get<TransformComponent>().Position = new Vector2(532f, 12f);
        
        List<KeyValuePair<string, string>> inputBindings = [
            new("Pause", "Esc"),
            new("Move up", "W"),
            new("Move down", "S"),
            new("Move left", "A"),
            new("Move right", "D"),
            new("Faster", "Spc")
        ];

        var p = 4f;
        foreach (var inputBinding in inputBindings)
        {
            var id1 = entityFactory.CreateLabel( contents.MainFont, inputBinding.Key, Color.White);
            _world.GetEntity(id1).Get<TransformComponent>().Position = new Vector2(-100f, p);

            var id2 = entityFactory.CreateSprite(contents.CollectableTexture, new Rectangle(32, 0, 32, 32));
            _world.GetEntity(id2).Get<TransformComponent>().Position = new Vector2(-140f, p);

            var id3 = entityFactory.CreateLabel(contents.MainFont, inputBinding.Value, Color.White);
            _world.GetEntity(id3).Get<TransformComponent>().Position = new Vector2(-140f, p);
            
            p += 40f;
        }
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