using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;

namespace SnakeGame.Core.ECS.Systems;

public class CollisionSystem : EntityUpdateSystem
{
    private readonly GameState _gameState;
    private ComponentMapper<SnakeComponent> _snakeMapper;
    private ComponentMapper<CollectableComponent> _collectableMapper;
    private ComponentMapper<CollisionEventComponent> _collisionEventMapper;
    private ComponentMapper<TransformComponent> _transformMapper;

    public CollisionSystem(GameState gameState) 
        : base(Aspect.One(typeof(SnakeComponent), typeof(CollectableComponent)))
    {
        _gameState = gameState;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _snakeMapper = mapperService.GetMapper<SnakeComponent>();
        _collectableMapper = mapperService.GetMapper<CollectableComponent>();
        _collisionEventMapper = mapperService.GetMapper<CollisionEventComponent>();
        _transformMapper = mapperService.GetMapper<TransformComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        if (_gameState.IsPaused) return;
        
        foreach (var entityId in ActiveEntities)
        {
            var snake = _snakeMapper.Get(entityId);

            if (snake is { IsInitialized: true, IsAlive: true })
            {
                var headRectangle = snake.Head.GetRectangle();
                
                if (CollidesWithSelf(snake) || !Globals.PlayFieldRectangle.Contains(headRectangle))
                {
                    _collisionEventMapper.Put(entityId, new CollisionEventComponent
                    {
                        EntityId = entityId,
                        CollidesWithEntityId = entityId
                    });
                    continue;
                }
                
                foreach (var otherEntityId in ActiveEntities)
                {
                    if (entityId != otherEntityId)
                    {
                        var otherSnake = _snakeMapper.Get(otherEntityId);

                        if (otherSnake is { IsInitialized: true })
                        {
                            if (snake != otherSnake && CollidesWith(otherSnake, headRectangle))
                            {
                                _collisionEventMapper.Put(entityId, new CollisionEventComponent
                                {
                                    EntityId = entityId,
                                    CollidesWithEntityId = otherEntityId
                                });
                            }
                        }
                        
                        var collectable = _collectableMapper.Get(otherEntityId);

                        if (collectable != null)
                        {
                            var transform = _transformMapper.Get(otherEntityId);
                            var headRect = snake.Head.GetRectangle();
                            
                            var rect = new Rectangle(
                                    (int)transform.Position.X,
                                    (int)transform.Position.Y,
                                    Constants.SegmentSize,
                                    Constants.SegmentSize);

                            if (rect.Intersects(headRect))
                            {
                                _collisionEventMapper.Put(entityId, new CollisionEventComponent
                                {
                                    EntityId = entityId,
                                    CollidesWithEntityId = otherEntityId
                                });
                            }
                        }
                    }
                }
            }
        }
    }
    
    private static bool CollidesWithSelf(SnakeComponent snake)
    {
        var headRectangle = snake.Head.GetRectangle();

        for (var i = 1; i < snake.Segments.Count; i++)
        {
            if (snake.Segments[i].GetRectangle().Intersects(headRectangle))
                return true;
        }

        return false;
    }
    
    private static bool CollidesWith(SnakeComponent snake, Rectangle rectangle)
    {
        if (snake.Head.GetRectangle().Intersects(rectangle))
            return true;

        if (snake.Tail.GetRectangle().Intersects(rectangle))
            return true;

        foreach (var segment in snake.Segments)
        {
            if (segment.GetRectangle().Intersects(rectangle))
                return true;
        }

        return false;
    }
}