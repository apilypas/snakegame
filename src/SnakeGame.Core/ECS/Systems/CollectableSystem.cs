using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.ECS.Entities;
using SnakeGame.Core.Enums;

namespace SnakeGame.Core.ECS.Systems;

public class CollectableSystem : EntityProcessingSystem
{
    private readonly GameState _gameState;
    private readonly EntityFactory _entityFactory;
    
    private ComponentMapper<PlayerComponent> _playerMapper;
    private ComponentMapper<CollectableComponent> _collectableMapper;
    private ComponentMapper<SnakeComponent> _snakeMapper;

    public CollectableSystem(GameState gameState,
        EntityFactory entityFactory)
        : base(Aspect.All(typeof(CollectableComponent)))
    {
        _gameState = gameState;
        _entityFactory = entityFactory;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _playerMapper = mapperService.GetMapper<PlayerComponent>();
        _collectableMapper = mapperService.GetMapper<CollectableComponent>();
        _snakeMapper = mapperService.GetMapper<SnakeComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        if (_gameState.IsPaused) return;
        
        var collectable = _collectableMapper.Get(entityId);

        if (collectable.CollectedByEntityId != null)
        {
            HandleCollectableBonus(collectable);

            if (_playerMapper.Has(collectable.CollectedByEntityId.Value))
            {
                GetEntity(entityId).Attach(new SoundEffectComponent
                {
                    Type = SoundEffectTypes.ScoreChanged
                });
            }

            DestroyEntity(entityId);
        }
    }

    private void SpawnFadingText(Vector2 at, string text)
    {
        var fadingTextEntity = _entityFactory.World.CreateFadingText(text);
        fadingTextEntity.Get<TransformComponent>().Position = at;
    }

    private void HandleCollectableBonus(CollectableComponent collectable)
    {
        if (collectable.CollectedByEntityId == null) return;
        
        var snake = _snakeMapper.Get(collectable.CollectedByEntityId.Value);
        
        if (collectable.CollectableType == CollectableType.Diamond)
        {
            snake.SegmentsToGrow++;
            if (_playerMapper.Has(collectable.CollectedByEntityId.Value))
            {
                var score = _gameState.ScoreMultiplicator * Constants.DiamondCollectScore;
                _gameState.Score += score;
                SpawnFadingText(snake.Head.Position, $"+{score}");
            }
        }
        else if (collectable.CollectableType == CollectableType.SnakePart)
        {
            snake.SegmentsToGrow++;
            if (_playerMapper.Has(collectable.CollectedByEntityId.Value))
            {
                var score = _gameState.ScoreMultiplicator * Constants.SnakePartCollectScore;
                _gameState.Score += score;
                SpawnFadingText(snake.Head.Position, $"+{score}");
            }
        }
        else if (collectable.CollectableType == CollectableType.SpeedBoost)
        {
            snake.SegmentsToGrow++;
            snake.SpeedTimer = Constants.SpeedUpTimer;
            if (_playerMapper.Has(collectable.CollectedByEntityId.Value))
            {
                var score = _gameState.ScoreMultiplicator * Constants.SpeedBoostCollectScore;
                _gameState.Score += score;
                SpawnFadingText(snake.Head.Position, $"+{score} (+Speed)");
            }
        }
        else if (collectable.CollectableType == CollectableType.Crown)
        {
            if (_playerMapper.Has(collectable.CollectedByEntityId.Value))
            {
                GetEntity(collectable.CollectedByEntityId.Value).Attach(new InvincibleComponent
                {
                    Timer = Constants.InvincibleTimer
                });
                var score = _gameState.ScoreMultiplicator * Constants.CrownCollectScore;
                _gameState.Score += score;
                SpawnFadingText(snake.Head.Position, $"+{score} (+Invincible)");
            }
        }
        else if (collectable.CollectableType == CollectableType.Clock)
        {
            snake.SegmentsToGrow++;
            if (_playerMapper.Has(collectable.CollectedByEntityId.Value))
            {
                _gameState.Timer += Constants.ClockBonus;
                _gameState.Timer = Math.Min(_gameState.Timer, Constants.MaxTimer);
                
                var score = _gameState.ScoreMultiplicator * Constants.ClockCollectScore;
                _gameState.Score += score;
                SpawnFadingText(snake.Head.Position, $"+{score} (+Time)");
            }
        }
    }
}