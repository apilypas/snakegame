using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SnakeGame.Core.Events;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Entities;

public class EntitySpawner(GameManager gameManager, AssetManager assets)
{
    private readonly Random _random = new();

    private float _diamondSpawnTimer;
    private float _speedBoostSpawnTimer;
    private float _enemySpawnTimer;
    
    private int _lastEntityId;

    public void Update(float deltaTime)
    {
        SpawnPlayerSnake();
        SpawnEnemySnake(deltaTime);
        SpawnRandomDiamond(deltaTime);
        SpawnRandomSpeedBoost(deltaTime);
        DespawnSnake(deltaTime);
    }

    public Collectable RemoveCollectable(Snake snake)
    {
        var targetRectangle = snake.Head.GetRectangle();
        var at = -1;
        
        for (var i = 0; i < gameManager.Collectables.Count; i++)
        {
            var collectable = gameManager.Collectables[i];

            var rectangle = new Rectangle(
                (int)collectable.Position.X,
                (int)collectable.Position.Y,
                Constants.SegmentSize,
                Constants.SegmentSize);

            if (rectangle.Intersects(targetRectangle))
            {
                at = i;
                break;
            }
        }

        if (at >= 0)
        {
            var collectable = gameManager.Collectables[at];
            collectable.QueueRemove = true;
            gameManager.Collectables.RemoveAt(at);
            gameManager.Events.Notify(new NotifyEvent(collectable, snake, NotifyEventType.CollectableRemoved));
            return collectable;
        }

        return null;
    }

    private void SpawnRandomDiamond(float deltaTime)
    {
        _diamondSpawnTimer += deltaTime;
        
        var diamondCount = 0;
        
        foreach (var collectable in gameManager.Collectables)
        {
            if (collectable.Type == CollectableType.Diamond)
                diamondCount++;
        }
        
        if (diamondCount > 0 && _diamondSpawnTimer < Constants.DiamondSpawnRate)
            return;

        if (diamondCount >= Constants.MaxDiamondLimit)
            return;

        var location = FindFreeLocation();

        if (location != null)
        {
            var collectable = new Collectable(assets.CollectableTexture, CollectableType.Diamond)
            {
                Id = GetNextId(),
                Position = location.Value
            };
            
            gameManager.Collectables.Add(collectable);
            gameManager.World.PlayField.Add(collectable);
        }
            
        _diamondSpawnTimer -= Constants.DiamondSpawnRate;
    }

    private void SpawnRandomSpeedBoost(float deltaTime)
    {
        _speedBoostSpawnTimer += deltaTime;

        if (_speedBoostSpawnTimer < Constants.SpeedBoostSpawnRate)
            return;
        
        var speedBoostCount = 0;

        foreach (var collectable in gameManager.Collectables)
        {
            if (collectable.Type == CollectableType.SpeedBoost)
                speedBoostCount++;
        }
        
        if (speedBoostCount >= Constants.MaxSpeedBoostLimit)
            return;

        var location = FindFreeLocation();

        if (location != null)
        {
            var collectable = new Collectable(assets.CollectableTexture, CollectableType.SpeedBoost)
            {
                Id = GetNextId(),
                Position = location.Value
            };
            
            gameManager.Collectables.Add(collectable);
            gameManager.World.PlayField.Add(collectable);
        }
        
        _speedBoostSpawnTimer -= Constants.SpeedBoostSpawnRate;
    }

    private Vector2? FindFreeLocation()
    {
        var random = _random.Next() % (Constants.WallWidth * Constants.WallHeight);

        while (random >= 0)
        {
            var foundFree = false;

            for (var i = 0; i < Constants.WallHeight; i++)
            {
                for (var j = 0; j < Constants.WallWidth; j++)
                {
                    var location = new Vector2(j * Constants.SegmentSize, i * Constants.SegmentSize);

                    if (IsLocationFree(location))
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

    private bool IsLocationFree(Vector2 location)
    {
        var rectangle = new Rectangle(
            (int)location.X,
            (int)location.Y,
            Constants.SegmentSize,
            Constants.SegmentSize);

        foreach (var snake in gameManager.Snakes)
        {
            if (snake.CollidesWith(rectangle)) return false;
        }

        foreach (var collectable in gameManager.Collectables)
        {
            if (collectable.Position == location) return false;
        }
        
        return true;
    }

    private void SpawnPlayerSnake()
    {
        Snake playerSnake = null;

        foreach (var snake in gameManager.Snakes)
        {
            if (snake is PlayerSnake)
            {
                playerSnake = snake;
                break;
            }
        }
        
        if (playerSnake != null)
            return;

        // Find best location
        var location = FindBestLocationForSnake();
        
        if (location == null)
            return;

        var direction = location.Value.X < (Constants.WallWidth * Constants.SegmentSize) / 2f ?
            SnakeDirection.Right : SnakeDirection.Left;
                
        playerSnake = new PlayerSnake(assets, location.Value, 2, direction);
        playerSnake.Id = GetNextId();
        playerSnake.Initialize();
                
        gameManager.Snakes.Add(playerSnake);
        gameManager.World.PlayField.Add(playerSnake);
    }

    private void SpawnEnemySnake(float deltaTime)
    {
        _enemySpawnTimer += deltaTime;
        
        var count = 0;

        foreach (var snake in gameManager.Snakes)
        {
            if (snake is EnemySnake)
            {
                count++;
            }
        }
        
        if (count > 0 && _enemySpawnTimer < Constants.EnemySpawnRate)
            return;

        if (count >= Constants.MaxEnemies)
        {
            _enemySpawnTimer -= Constants.EnemySpawnRate;
            return;
        }

        // Find best location
        var location = FindBestLocationForSnake();
        
        if (location == null)
            return;

        var direction = location.Value.X < (Constants.WallWidth * Constants.SegmentSize) / 2f ?
            SnakeDirection.Right : SnakeDirection.Left;
                
        var enemySnake = new EnemySnake(assets, location.Value, 2, direction, gameManager);
        enemySnake.Id = GetNextId();
        enemySnake.Initialize();
                
        gameManager.Snakes.Add(enemySnake);
        gameManager.World.PlayField.Add(enemySnake);

        _enemySpawnTimer -= Constants.EnemySpawnRate;
    }

    private Vector2? FindBestLocationForSnake()
    {
        var location = FindFreeLocation();

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
                if (!IsLocationFree(new Vector2(x, y)))
                    return null;
            }
        }

        return location;
    }

    private readonly HashSet<Snake> _despawnSnakes = [];
    private void DespawnSnake(float deltaTime)
    {
        _despawnSnakes.Clear();
        
        foreach (var snake in gameManager.Snakes)
        {
            if (snake.State == SnakeState.Dead)
            {
                if (snake.Reduce(deltaTime) && snake.Segments.Count > 0)
                {
                    SpawnSnakePart(snake);
                }

                if (snake.Segments.Count == 0)
                    _despawnSnakes.Add(snake);
            }
        }

        foreach (var snake in _despawnSnakes)
        {
            gameManager.Snakes.Remove(snake);
            snake.QueueRemove = true;
        }
    }
    
    private void SpawnSnakePart(Snake snake)
    {
        var spawnSnakePart = _random.Next() % 3 == 0;

        if (spawnSnakePart)
        {
            var snakePart = new Collectable(assets.CollectableTexture, CollectableType.SnakePart)
            {
                Id = GetNextId(),
                Position = snake.Segments[0].Position
            };

            gameManager.Collectables.Add(snakePart);
            gameManager.World.PlayField.Add(snakePart);
        }
        else if (snake is EnemySnake) // Only enemies can spawn clocks
        {
            var spawnClock = _random.Next() % 6 == 0;

            if (spawnClock)
            {
                var clock = new Collectable(assets.CollectableTexture, CollectableType.Clock)
                {
                    Id = GetNextId(),
                    Position = snake.Segments[0].Position
                };

                gameManager.Collectables.Add(clock);
                gameManager.World.PlayField.Add(clock);
            }
        }
    }

    private int GetNextId()
    {
        return _lastEntityId++;
    }
}