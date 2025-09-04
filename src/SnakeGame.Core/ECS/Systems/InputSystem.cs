using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.ECS.Entities;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Services;

namespace SnakeGame.Core.ECS.Systems;

public class InputSystem : EntityProcessingSystem
{
    private readonly InputManager _inputs;
    private readonly GraphicsDeviceManager _graphicsDeviceManager;
    private readonly EntityFactory _entityFactory;
    private ComponentMapper<GameState> _gameStateMapper;

    public InputSystem(InputManager inputs, Game game, EntityFactory entityFactory) 
        : base(Aspect.All(typeof(GameState)))
    {
        _inputs = inputs;
        _graphicsDeviceManager = game.Services.GetService<GraphicsDeviceManager>();
        _entityFactory = entityFactory;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _gameStateMapper = mapperService.GetMapper<GameState>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var gameState = _gameStateMapper.Get(entityId);
        
        if (_inputs.IsActionPressed(InputActions.Pause))
        {
            if (gameState.State == GameWorldState.Running)
            {
                if (!gameState.IsPaused)
                {
                    gameState.IsPaused = true;

                    if (gameState.IsPaused)
                    {
                        var dialogId = _entityFactory.CreateDialog(
                            "Paused",
                            "Your game is paused",
                            new SizeF(220, 140),
                            ("Resume", () =>
                            {
                                GetEntity(entityId).Attach(new ButtonEventComponent
                                {
                                    Event = ButtonEvents.Resume
                                });
                            }),
                            ("Exit", () =>
                            {
                                GetEntity(entityId).Attach(new ButtonEventComponent
                                {
                                    Event = ButtonEvents.Exit
                                });
                            }));

                        gameState.PausedDialogId = dialogId;
                    }
                }
            }
        }

        if (_inputs.IsActionPressed(InputActions.Fullscreen))
        {
            _graphicsDeviceManager?.ToggleFullScreen();
        }
    }
}