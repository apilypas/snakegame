using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Enums;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Services;

namespace SnakeGame.Core.ECS.Systems;

public class PlayerInputSystem : EntityProcessingSystem
{
    private readonly InputManager _inputs;
    private readonly GameState _gameState;
    private ComponentMapper<SnakeComponent> _snakeMapper;

    public PlayerInputSystem(InputManager inputs, GameState gameState)
        : base(Aspect.All(typeof(PlayerComponent)))
    {
        _inputs = inputs;
        _gameState = gameState;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _snakeMapper = mapperService.GetMapper<SnakeComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        if (_gameState.IsPaused) return;
        
        var snake = _snakeMapper.Get(entityId);

        if (!snake.IsAlive)
            return;
        
        if (_inputs.IsActionDown(InputActions.Up) && snake.Direction != SnakeDirection.Up)
        {
            snake.NewDirection = SnakeDirection.Up;
        }

        if (_inputs.IsActionDown(InputActions.Down) && snake.Direction != SnakeDirection.Down)
        {
            snake.NewDirection = SnakeDirection.Down;
        }

        if (_inputs.IsActionDown(InputActions.Left) && snake.Direction != SnakeDirection.Left)
        {
            snake.NewDirection = SnakeDirection.Left;
        }

        if (_inputs.IsActionDown(InputActions.Right) && snake.Direction != SnakeDirection.Right)
        {
            snake.NewDirection = SnakeDirection.Right;
        }

        if (_inputs.IsActionDown(InputActions.Faster))
        {
            snake.IsFaster = true;
        }

        if (!_inputs.IsActionDown(InputActions.Faster))
        {
            snake.IsFaster = false;
        }
    }
}