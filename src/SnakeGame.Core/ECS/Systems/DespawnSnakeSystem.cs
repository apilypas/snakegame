using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.ECS.Entities;
using SnakeGame.Core.Enums;

namespace SnakeGame.Core.ECS.Systems;

public class DespawnSnakeSystem : EntityProcessingSystem
{
    private readonly GameState _gameState;
    private readonly EntityFactory _entityFactory;
    private ComponentMapper<SnakeComponent> _snakeMapper;
    private ComponentMapper<PlayerComponent> _playerMapper;

    public DespawnSnakeSystem(GameState gameState, EntityFactory entityFactory) 
        : base(Aspect.All(typeof(SnakeComponent)))
    {
        _gameState = gameState;
        _entityFactory = entityFactory;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _snakeMapper = mapperService.GetMapper<SnakeComponent>();
        _playerMapper = mapperService.GetMapper<PlayerComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var snake = _snakeMapper.Get(entityId);
            
        if (snake.IsInitialized && !snake.IsAlive)
        {
            if (Reduce(snake, deltaTime) && snake.Segments.Count > 0)
            {
                SpawnSnakePart(entityId);
            }

            if (snake.Segments.Count == 0)
            {
                _gameState.Snakes.Remove(GetEntity(entityId));
                    
                if (_playerMapper.Has(entityId))
                    _gameState.PlayerSnake = null;
                
                DestroyEntity(entityId);
            }
        }
    }
    
    private static bool Reduce(SnakeComponent snake, float deltaTime)
    {
        const float reduceByMs = .03f;
        var reduced = false;

        if (snake.Segments.Count > 0)
        {
            snake.DeathAnimationTimer += deltaTime;
            
            if (snake.DeathAnimationTimer >= reduceByMs)
            {
                snake.Segments.RemoveAt(0);
                reduced = true;

                if (snake.Segments.Count > 0)
                {
                    snake.Head = snake.Segments[0].Clone();
                    snake.DeathAnimationTimer -= reduceByMs;
                }
                else
                {
                    snake.Head = null;
                    snake.Tail = null;
                }
            }
        }

        return reduced;
    }
    
    private void SpawnSnakePart(int entityId)
    {
        var isSpawningSnakePart = Random.Shared.Next() % 3 == 0;
        var snake = _snakeMapper.Get(entityId);

        if (isSpawningSnakePart)
        {
            var collectableEntity = _entityFactory.World.CreateCollectable(CollectableType.SnakePart);
            collectableEntity.Get<TransformComponent>().Position = snake.Segments[0].Position;
            
            _gameState.Collectables.Add(collectableEntity);
        }
        else if (!_playerMapper.Has(entityId)) // Only enemies can spawn clocks
        {
            var isSpawningClock = Random.Shared.Next() % 6 == 0;

            if (isSpawningClock)
            {
                var collectableEntity = _entityFactory.World.CreateCollectable(CollectableType.Clock);
                collectableEntity.Get<TransformComponent>().Position = snake.Segments[0].Position;
                
                _gameState.Collectables.Add(collectableEntity);
            }
        }
    }
}