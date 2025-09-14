using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;

namespace SnakeGame.Core.ECS.Systems;

public class SnakeSpeedSystem : EntityProcessingSystem
{
    private readonly GameState _gameState;
    private ComponentMapper<SnakeComponent> _snakeMapper;
    private ComponentMapper<SpeedUpComponent> _speedUpMapper;
    private ComponentMapper<SoundEffectComponent> _soundEffectMapper;
    private ComponentMapper<PlayerComponent> _playerMapper;

    public SnakeSpeedSystem(GameState gameState) 
        : base(Aspect.All(typeof(SnakeComponent)))
    {
        _gameState = gameState;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _snakeMapper = mapperService.GetMapper<SnakeComponent>();
        _speedUpMapper = mapperService.GetMapper<SpeedUpComponent>();
        _soundEffectMapper = mapperService.GetMapper<SoundEffectComponent>();
        _playerMapper = mapperService.GetMapper<PlayerComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        if (_gameState.IsPaused) return;
        
        var snake = _snakeMapper.Get(entityId);

        if (!snake.IsAlive) return;
        
        var speedUp = _speedUpMapper.Get(entityId);

        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        var isFaster = false;
        
        if (speedUp.Timer > 0)
        {
            speedUp.Timer -= deltaTime;
            isFaster = true;
        }

        if (speedUp.IsForced)
        {
            isFaster = true;
        }
        
        var newSpeed = isFaster ? Constants.IncreasedSnakeSpeed : Constants.DefaultSnakeSpeed;
        var isSpeedChanged = MathF.Abs(snake.Speed - newSpeed) > 1f;
        snake.Speed = newSpeed;

        if (isFaster && isSpeedChanged && _playerMapper.Has(entityId))
        {
            _soundEffectMapper.Put(entityId,
                new SoundEffectComponent
                {
                    Type = SoundEffectTypes.SpeedUp
                });
        }
    }
}