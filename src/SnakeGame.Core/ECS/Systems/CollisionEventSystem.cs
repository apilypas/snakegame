using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.ECS.Entities;

namespace SnakeGame.Core.ECS.Systems;

public class CollisionEventSystem : EntityProcessingSystem
{
    private readonly GameState _gameState;
    private readonly EntityFactory _entityFactory;
    private ComponentMapper<CollisionEventComponent> _collisionEventMapper;
    private ComponentMapper<SnakeComponent> _snakeMapper;
    private ComponentMapper<InvincibleComponent> _invincibleMapper;
    private ComponentMapper<PlayerComponent> _playerMapper;
    private ComponentMapper<CollectableComponent> _collectableMapper;
    private ComponentMapper<SoundEffectComponent> _soundEffectMapper;
    private ComponentMapper<ScreenShakeComponent> _screenShakeMapper;

    public CollisionEventSystem(GameState gameState, EntityFactory entityFactory) 
        : base(Aspect.All(typeof(CollisionEventComponent)))
    {
        _gameState = gameState;
        _entityFactory = entityFactory;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _collisionEventMapper = mapperService.GetMapper<CollisionEventComponent>();
        _snakeMapper = mapperService.GetMapper<SnakeComponent>();
        _invincibleMapper = mapperService.GetMapper<InvincibleComponent>();
        _playerMapper = mapperService.GetMapper<PlayerComponent>();
        _collectableMapper = mapperService.GetMapper<CollectableComponent>();
        _soundEffectMapper  = mapperService.GetMapper<SoundEffectComponent>();
        _screenShakeMapper = mapperService.GetMapper<ScreenShakeComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        if (_gameState.IsPaused) return;
        
        var collisionEvent = _collisionEventMapper.Get(entityId);

        if (_snakeMapper.Has(collisionEvent.CollidesWithEntityId))
        {
            HandleSnakeCollision(entityId, collisionEvent);
        }

        if (_collectableMapper.Has(collisionEvent.CollidesWithEntityId))
        {
            HandleCollectableCollision(collisionEvent);
        }

        _collisionEventMapper.Delete(entityId);
    }

    private void HandleCollectableCollision(CollisionEventComponent collisionEvent)
    {
        var collectable = _collectableMapper.Get(collisionEvent.CollidesWithEntityId);

        collectable.CollectedByEntityId = collisionEvent.EntityId;
    }

    private void HandleSnakeCollision(int entityId, CollisionEventComponent collisionEvent)
    {
        var isInvincible = _invincibleMapper.Has(collisionEvent.EntityId);

        if (collisionEvent.EntityId != collisionEvent.CollidesWithEntityId && isInvincible)
        {
            var otherSnake = _snakeMapper.Get(collisionEvent.CollidesWithEntityId);
            var snake = _snakeMapper.Get(collisionEvent.EntityId);

            if (otherSnake.IsAlive)
            {
                _soundEffectMapper.Put(collisionEvent.EntityId,
                    new SoundEffectComponent
                    {
                        Type = SoundEffectTypes.EnemyHit
                    });

                _screenShakeMapper.Put(collisionEvent.EntityId, new ScreenShakeComponent());

                var score = _gameState.ScoreMultiplicator * otherSnake.Segments.Count;
                _gameState.Score += score;

                var fadingTextEntity = _entityFactory.World.CreateFadingText($"+{score}");
                fadingTextEntity.Get<TransformComponent>().Position = snake.Head.Position;

                otherSnake.IsAlive = false;
            }
        }
        else
        {
            var snake = _snakeMapper.Get(collisionEvent.EntityId);
            var isPlayer = _playerMapper.Has(collisionEvent.EntityId);
            
            snake.IsAlive = false;

            if (isPlayer)
            {
                _gameState.Deaths++;
                
                _soundEffectMapper.Put(entityId, new SoundEffectComponent
                {
                    Type = SoundEffectTypes.Hit
                });
                
                _screenShakeMapper.Put(entityId, new ScreenShakeComponent());
            }
        }
    }
}