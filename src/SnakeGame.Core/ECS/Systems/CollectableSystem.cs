using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Tweening;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.ECS.Entities;
using SnakeGame.Core.Enums;

namespace SnakeGame.Core.ECS.Systems;

public class CollectableSystem : EntityUpdateSystem
{
    private readonly GameState _gameState;
    private readonly EntityFactory _entityFactory;
    private readonly Tweener _tweener;

    private ComponentMapper<PlayerComponent> _playerMapper;
    private ComponentMapper<CollectableComponent> _collectableMapper;
    private ComponentMapper<SnakeComponent> _snakeMapper;
    private ComponentMapper<SpeedUpComponent> _speedUpMapper;
    private ComponentMapper<TransformComponent> _transformMapper;
    private ComponentMapper<SpriteComponent> _spriteMapper;
    private ComponentMapper<SoundEffectComponent> _soundEffectMapper;

    public CollectableSystem(GameState gameState,
        EntityFactory entityFactory)
        : base(Aspect.All(typeof(CollectableComponent)))
    {
        _gameState = gameState;
        _entityFactory = entityFactory;
        _tweener = new Tweener();
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _playerMapper = mapperService.GetMapper<PlayerComponent>();
        _collectableMapper = mapperService.GetMapper<CollectableComponent>();
        _snakeMapper = mapperService.GetMapper<SnakeComponent>();
        _speedUpMapper = mapperService.GetMapper<SpeedUpComponent>();
        _transformMapper = mapperService.GetMapper<TransformComponent>();
        _spriteMapper = mapperService.GetMapper<SpriteComponent>();
        _soundEffectMapper = mapperService.GetMapper<SoundEffectComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        _tweener.Update(deltaTime);
        
        foreach (var entityId in ActiveEntities)
            HandlePulseTween(entityId);
        
        if (_gameState.IsPaused) return;

        foreach (var entityId in ActiveEntities)
        {
            var collectable = _collectableMapper.Get(entityId);

            if (collectable.CollectedByEntityId != null)
            {
                HandleCollectableBonus(collectable);

                DestroyEntity(entityId);
            }
        }
    }

    private void HandlePulseTween(int entityId)
    {
        var collectable = _collectableMapper.Get(entityId);

        if (collectable.PulseTweens.Count == 0)
        {
            var transform = _transformMapper.Get(entityId);
            var sprite = _spriteMapper.Get(entityId);

            transform.Scale = new Vector2(1.2f);
            sprite.Sprite.Origin = new Vector2(2f, 2f);
            
            var pulseTween1 = _tweener.TweenTo(
                    transform,
                    c => c.Scale,
                    new Vector2(1),
                    .2f)
                .RepeatForever(1f)
                .Easing(EasingFunctions.BounceInOut);

            var pulseTween2 = _tweener.TweenTo(
                    sprite.Sprite,
                    c => c.Origin,
                    new Vector2(0f),
                    .2f)
                .RepeatForever(1f)
                .Easing(EasingFunctions.BounceInOut);
            
            collectable.PulseTweens.Add(pulseTween1);
            collectable.PulseTweens.Add(pulseTween2);
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
                _gameState.Experience++;
                SpawnFadingText(snake.Head.Position, $"+{score}");
                
                _soundEffectMapper.Put(collectable.CollectedByEntityId.Value, new SoundEffectComponent
                {
                    Type = SoundEffectTypes.Pickup
                });
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
                
                _soundEffectMapper.Put(collectable.CollectedByEntityId.Value, new SoundEffectComponent
                {
                    Type = SoundEffectTypes.Pickup
                });
            }
        }
        else if (collectable.CollectableType == CollectableType.SpeedBoost)
        {
            snake.SegmentsToGrow++;

            _speedUpMapper.Get(collectable.CollectedByEntityId.Value).Timer = Constants.SpeedUpTimer;
            
            if (_playerMapper.Has(collectable.CollectedByEntityId.Value))
            {
                var score = _gameState.ScoreMultiplicator * Constants.SpeedBoostCollectScore;
                _gameState.Score += score;
                SpawnFadingText(snake.Head.Position, $"+{score} (+Speed)");
                
                _soundEffectMapper.Put(collectable.CollectedByEntityId.Value, new SoundEffectComponent
                {
                    Type = SoundEffectTypes.Pickup
                });
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
                
                _soundEffectMapper.Put(collectable.CollectedByEntityId.Value, new SoundEffectComponent
                {
                    Type = SoundEffectTypes.PowerUp
                });
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
                
                _soundEffectMapper.Put(collectable.CollectedByEntityId.Value, new SoundEffectComponent
                {
                    Type = SoundEffectTypes.AddTime
                });
            }
        }
    }
}