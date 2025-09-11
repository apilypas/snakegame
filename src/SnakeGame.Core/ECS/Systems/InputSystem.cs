using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.ECS.Entities;
using SnakeGame.Core.Services;

namespace SnakeGame.Core.ECS.Systems;

public class InputSystem : EntityUpdateSystem
{
    private readonly InputManager _inputs;
    private readonly GraphicsDeviceManager _graphicsDeviceManager;
    private readonly EntityFactory _entityFactory;
    private readonly GameState _gameState;
    private ComponentMapper<NavigationIntentComponent> _navigationIntentMapper;

    public InputSystem(InputManager inputs, Game game, EntityFactory entityFactory, GameState gameState) 
        : base(Aspect.One(typeof(NavigationIntentComponent)))
    {
        _inputs = inputs;
        _graphicsDeviceManager = game.Services.GetService<GraphicsDeviceManager>();
        _entityFactory = entityFactory;
        _gameState = gameState;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _navigationIntentMapper = mapperService.GetMapper<NavigationIntentComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        NavigationIntentComponent navigationIntent = null;

        foreach (var entityId in ActiveEntities)
        {
            navigationIntent = _navigationIntentMapper.Get(entityId);
        }
        
        if (_inputs.WasActionPressed(InputActions.Pause))
        {
            if (_gameState.State == GameWorldState.Running)
            {
                if (!_gameState.IsPaused)
                {
                    _gameState.IsPaused = true;
    
                    if (_gameState.IsPaused)
                    {
                        _entityFactory.Dialog.CreatePauseDialog();
                    }
                }
            }
        }
    
        if (_inputs.WasActionPressed(InputActions.Fullscreen))
        {
            _graphicsDeviceManager?.ToggleFullScreen();
        }

        if (_inputs.WasActionPressed(InputActions.Down) || _inputs.WasActionPressed(InputActions.Right))
        {
            if (navigationIntent != null)
                navigationIntent.Event = NavigationEvent.FocusNext;
        }
        
        if (_inputs.WasActionPressed(InputActions.Up) || _inputs.WasActionPressed(InputActions.Left))
        {
            if (navigationIntent != null)
                navigationIntent.Event = NavigationEvent.FocusPrevious;
        }

        if (_inputs.WasActionPressed(InputActions.Faster))
        {
            if (navigationIntent != null)
                navigationIntent.Event = NavigationEvent.Select;
        }
    }
}