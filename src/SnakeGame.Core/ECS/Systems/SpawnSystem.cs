using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.ECS.Entities;
using SnakeGame.Core.Enums;

namespace SnakeGame.Core.ECS.Systems;

public class SpawnSystem : EntityUpdateSystem
{
    private readonly GameState _gameState;
    private readonly EntityFactory _entityFactory;
    
    private ComponentMapper<SnakeComponent> _snakeMapper;
    private ComponentMapper<CollectableComponent> _collectableMapper;
    private ComponentMapper<PlayerComponent> _playerMapper;
    private ComponentMapper<TransformComponent> _transformMapper;

    private float _enemySpawnTimer;
    private float _diamondSpawnTimer;
    private float _speedBoostSpawnTimer;
    private float _crownSpawnTimer;

    public SpawnSystem(GameState gameState, EntityFactory entityFactory) 
        : base(Aspect.One(typeof(SnakeComponent), typeof(CollectableComponent)))
    {
        _gameState = gameState;
        _entityFactory  = entityFactory;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _snakeMapper = mapperService.GetMapper<SnakeComponent>();
        _collectableMapper = mapperService.GetMapper<CollectableComponent>();
        _playerMapper = mapperService.GetMapper<PlayerComponent>();
        _transformMapper = mapperService.GetMapper<TransformComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        if (_gameState.IsPaused) return;
        
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        HandlePlayerSpawning();
        HandleEnemySpawning(deltaTime);
        HandleDiamondSpawning(deltaTime);
        HandleSpeedBoostSpawning(deltaTime);
        HandleCrownSpawning(deltaTime);
    }

    private void HandlePlayerSpawning()
    {
        var hasPlayerSnake = false;
        
        foreach (var entityId in ActiveEntities)
        {
            if (_snakeMapper.Has(entityId))
            {
                if (_playerMapper.Has(entityId))
                {
                    hasPlayerSnake = true;
                }
            }
        }

        if (!hasPlayerSnake)
        {
            // Find best location
            var location = FindBestLocationForSnake();

            if (location != null)
            {
                var direction = location.Value.X < Constants.WallWidth * Constants.SegmentSize / 2f
                    ? SnakeDirection.Right
                    : SnakeDirection.Left;

                var playerSnakeEntity = _entityFactory.World.CreatePlayerSnake(location.Value, 2, direction);

                _gameState.Snakes.Add(playerSnakeEntity);

                _gameState.PlayerSnake = playerSnakeEntity;
            }
        }
    }

    private void HandleEnemySpawning(float deltaTime)
    {
        var enemyCount = 0;
        
        _enemySpawnTimer += deltaTime;
        
        foreach (var entityId in ActiveEntities)
        {
            if (_snakeMapper.Has(entityId))
            {
                if (!_playerMapper.Has(entityId))
                {
                    enemyCount++;
                }
            }
        }

        if (enemyCount <= 0 || _enemySpawnTimer >= Constants.EnemySpawnRate)
        {
            _enemySpawnTimer -= Constants.EnemySpawnRate;

            if (enemyCount < Constants.MaxEnemies)
            {
                // Find best location
                var location = FindBestLocationForSnake();

                if (location != null)
                {
                    var direction = location.Value.X < Constants.WallWidth * Constants.SegmentSize / 2f
                        ? SnakeDirection.Right
                        : SnakeDirection.Left;

                    var enemySnakeEntity =
                        _entityFactory.World.CreateEnemySnake(_gameState, location.Value, 2, direction);

                    _gameState.Snakes.Add(enemySnakeEntity);
                }
            }
        }
    }

    private void HandleDiamondSpawning(float deltaTime)
    {
        _diamondSpawnTimer += deltaTime;
        
        var diamondCount = 0;
        
        foreach (var entityId in ActiveEntities)
        {
            var collectable = _collectableMapper.Get(entityId);

            if (collectable is { CollectableType: CollectableType.Diamond })
            {
                diamondCount++;
            }
        }
        
        if (diamondCount > 0 && _diamondSpawnTimer < Constants.DiamondSpawnRate)
            return;

        if (diamondCount >= Constants.MaxDiamondLimit)
            return;

        var location = FindEmptyLocation();

        if (location != null)
        {
            var collectableEntity = _entityFactory.World.CreateCollectable(CollectableType.Diamond);

            collectableEntity.Get<TransformComponent>().Position = location.Value;
            
            _gameState.Collectables.Add(collectableEntity);
        }
            
        _diamondSpawnTimer -= Constants.DiamondSpawnRate;
    }

    private void HandleSpeedBoostSpawning(float deltaTime)
    {
        _speedBoostSpawnTimer += deltaTime;

        if (_speedBoostSpawnTimer < Constants.SpeedBoostSpawnRate)
            return;
        
        var speedBoostCount = 0;

        foreach (var entityId in ActiveEntities)
        {
            var collectable = _collectableMapper.Get(entityId);
            
            if (collectable is { CollectableType: CollectableType.SpeedBoost })
                speedBoostCount++;
        }
        
        if (speedBoostCount >= Constants.MaxSpeedBoostLimit)
            return;

        var location = FindEmptyLocation();

        if (location != null)
        {
            var collectableEntity = _entityFactory.World.CreateCollectable(CollectableType.SpeedBoost);
            collectableEntity.Get<TransformComponent>().Position = location.Value;
            
            _gameState.Collectables.Add(collectableEntity);
        }
        
        _speedBoostSpawnTimer -= Constants.SpeedBoostSpawnRate;
    }
    
    private void HandleCrownSpawning(float deltaTime)
    {
        _crownSpawnTimer += deltaTime;

        if (_crownSpawnTimer < Constants.CrownSpawnRate)
            return;
        
        var crownCount = 0;

        foreach (var entityId in ActiveEntities)
        {
            var collectable = _collectableMapper.Get(entityId);
            
            if (collectable is { CollectableType: CollectableType.Crown })
            {
                crownCount++;
            }
        }
        
        if (crownCount >= Constants.MaxCrownLimit)
            return;

        var location = FindEmptyLocation();

        if (location != null)
        {
            var collectableEntity = _entityFactory.World.CreateCollectable(CollectableType.Crown);
            collectableEntity.Get<TransformComponent>().Position = location.Value;
            
            _gameState.Collectables.Add(collectableEntity);
        }
        
        _crownSpawnTimer -= Constants.CrownSpawnRate;
    }
    
    private Vector2? FindBestLocationForSnake()
    {
        var location = FindEmptyLocation();

        if (location == null)
            return null;

        var fromX = location.Value.X - 2 * Constants.SegmentSize;
        var fromY = location.Value.Y - 2 * Constants.SegmentSize;
        var toX = location.Value.X + 2 * Constants.SegmentSize;
        var toY = location.Value.Y + 2 * Constants.SegmentSize;

        if (fromX < 0f)
            return null;
        
        if (fromY < 0f)
            return null;

        if (toX > Constants.WallWidth * Constants.SegmentSize)
            return null;
        
        if (toY > Constants.WallHeight * Constants.SegmentSize)
            return null;

        for (var x = fromX; x <= toX; x++)
        {
            for (var y = fromY; y <= toY; y++)
            {
                if (!IsLocationEmpty(new Vector2(x, y)))
                    return null;
            }
        }

        return location;
    }
    
    private Vector2? FindEmptyLocation()
    {
        var random = Random.Shared.Next() % (Constants.WallWidth * Constants.WallHeight);

        while (random >= 0)
        {
            var foundFree = false;

            for (var i = 0; i < Constants.WallHeight; i++)
            {
                for (var j = 0; j < Constants.WallWidth; j++)
                {
                    var location = new Vector2(j * Constants.SegmentSize, i * Constants.SegmentSize);

                    if (IsLocationEmpty(location))
                    {
                        if (random <= 0)
                            return location;

                        random--;
                        foundFree = true;
                    }
                }
            }

            if (!foundFree)
                break;
        }

        return null;
    }

    private bool IsLocationEmpty(Vector2 location)
    {
        var rectangle = new Rectangle(
            (int)location.X,
            (int)location.Y,
            Constants.SegmentSize,
            Constants.SegmentSize);

        foreach (var entityId in ActiveEntities)
        {
            var snake = _snakeMapper.Get(entityId);

            if (snake is { IsInitialized: true, IsAlive: true })
            {
                if (CollidesWith(snake, rectangle))
                {
                    return false;
                }
            }
        }

        foreach (var entityId in ActiveEntities)
        {
            if (_collectableMapper.Has(entityId))
            {
                var transform = _transformMapper.Get(entityId);
                var rect = new Rectangle(
                    (int)transform.Position.X,
                    (int)transform.Position.Y,
                    Constants.SegmentSize,
                    Constants.SegmentSize);
                
                if (rect.Intersects(rectangle))
                {
                    return false;
                }
            }
        }
        
        return true;
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