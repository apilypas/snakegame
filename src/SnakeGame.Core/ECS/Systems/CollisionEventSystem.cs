using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;

namespace SnakeGame.Core.ECS.Systems;

public class CollisionEventSystem : EntityProcessingSystem
{
    private readonly GameState _gameState;
    private ComponentMapper<CollisionEventComponent> _collisionEventMapper;
    private ComponentMapper<SnakeComponent> _snakeMapper;
    private ComponentMapper<InvincibleComponent> _invincibleMapper;
    private ComponentMapper<PlayerComponent> _playerMapper;
    private ComponentMapper<CollectableComponent> _collectableMapper;

    public CollisionEventSystem(GameState gameState) 
        : base(Aspect.All(typeof(CollisionEventComponent)))
    {
        _gameState = gameState;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _collisionEventMapper = mapperService.GetMapper<CollisionEventComponent>();
        _snakeMapper = mapperService.GetMapper<SnakeComponent>();
        _invincibleMapper = mapperService.GetMapper<InvincibleComponent>();
        _playerMapper = mapperService.GetMapper<PlayerComponent>();
        _collectableMapper = mapperService.GetMapper<CollectableComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        if (_gameState.IsPaused) return;
        
        var collisionEvent = _collisionEventMapper.Get(entityId);

        if (_snakeMapper.Has(collisionEvent.CollidesWithEntityId))
        {
            HandleSnakeCollision(collisionEvent);
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

    private void HandleSnakeCollision(CollisionEventComponent collisionEvent)
    {
        var snake = _snakeMapper.Get(collisionEvent.EntityId);
        var isInvincible = _invincibleMapper.Has(collisionEvent.EntityId);
        var isPlayer = _playerMapper.Has(collisionEvent.EntityId);

        if (collisionEvent.EntityId != collisionEvent.CollidesWithEntityId && isInvincible)
        {
            var otherSnake = _snakeMapper.Get(collisionEvent.CollidesWithEntityId);

            otherSnake.IsAlive = false;
        }
        else
        {
            snake.IsAlive = false;

            if (isPlayer)
            {
                _gameState.Deaths++;

                GetEntity(collisionEvent.EntityId).Attach(new SoundEffectComponent
                {
                    Type = SoundEffectTypes.PlayerDied
                });
            }
        }
    }
}