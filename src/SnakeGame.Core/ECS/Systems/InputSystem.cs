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

public class InputSystem : EntityUpdateSystem
{
    private readonly InputManager _inputs;
    private readonly GraphicsDeviceManager _graphicsDeviceManager;
    private readonly EntityFactory _entityFactory;
    private readonly GameState _gameState;

    public InputSystem(InputManager inputs, Game game, EntityFactory entityFactory, GameState gameState) 
        : base(Aspect.All())
    {
        _inputs = inputs;
        _graphicsDeviceManager = game.Services.GetService<GraphicsDeviceManager>();
        _entityFactory = entityFactory;
        _gameState = gameState;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
    }

    public override void Update(GameTime gameTime)
    {
        if (_inputs.IsActionPressed(InputActions.Pause))
        {
            if (_gameState.State == GameWorldState.Running)
            {
                if (!_gameState.IsPaused)
                {
                    _gameState.IsPaused = true;
    
                    if (_gameState.IsPaused)
                    {
                        var dialogId = _entityFactory.Dialogs.CreateDialog(
                            "Paused",
                            "Your game is paused",
                            new SizeF(220, 140),
                            ("Resume", ButtonEvents.Resume),
                            ("Exit", ButtonEvents.Close));
    
                        _gameState.PausedDialogId = dialogId;
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