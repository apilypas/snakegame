using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SnakeGame.Core.Entities;

namespace SnakeGame.Core.Systems;

public class EntityManager
{
    private readonly Random _random = new();

    private float _diamondSpawnTimer;
    private float _speedBoostSpawnTimer;
    private float _enemySpawnTimer;
    
    private int _lastEntityId;
    
    private readonly World _world;
    private readonly AssetManager _assets;
    
    public HashSet<Snake> Snakes { get; } = [];
    public HashSet<Collectable> Collectables { get; } = [];

    public EntityManager(World world, AssetManager assets)
    {
        _world = world;
        _assets = assets;
    }

    public void Update(GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        SpawnPlayerSnake();
        SpawnEnemySnake(deltaTime);
        SpawnRandomDiamond(deltaTime);
        SpawnRandomSpeedBoost(deltaTime);
        DespawnSnake(deltaTime);
    }
    
    public void SpawnPlayerSnake(Vector2 at, int length, SnakeDirection direction)
    {
        var playerSnake = new PlayerSnake(_assets, at, length, direction);
        
        Snakes.Add(playerSnake);
        _world.PlayField.AddChild(playerSnake);
        _world.PlayerSnake = playerSnake;
        
        playerSnake.Initialize();
    }

    public void SpawnEnemySnake(Vector2 at, int length, SnakeDirection direction)
    {
        var enemySnake = new EnemySnake(_assets, at, length, direction, this)
        {
            Id = GetNextId()
        };

        Snakes.Add(enemySnake);
        _world.PlayField.AddChild(enemySnake);
        
        enemySnake.Initialize();
    }

    public void SpawnFadingText(Vector2 at, string text)
    {
        var fadingText = new FadingText(text)
        {
            Position = at
        };

        _world.FadingTextLayer.AddChild(fadingText);
    }
    
    public Collectable GetCollectableAt(Rectangle targetRectangle)
    {
        foreach (var collectable in Collectables)
        {
            if (collectable.GetRectangle().Intersects(targetRectangle))
            {
                return collectable;
            }
        }

        return null;
    }

    private void SpawnRandomDiamond(float deltaTime)
    {
        _diamondSpawnTimer += deltaTime;
        
        var diamondCount = 0;
        
        foreach (var collectable in Collectables)
        {
            if (collectable.Type == CollectableType.Diamond)
                diamondCount++;
        }
        
        if (diamondCount > 0 && _diamondSpawnTimer < Constants.DiamondSpawnRate)
            return;

        if (diamondCount >= Constants.MaxDiamondLimit)
            return;

        var location = FindEmptyLocation();

        if (location != null)
        {
            var collectable = new Collectable(_assets.CollectableTexture, CollectableType.Diamond)
            {
                Id = GetNextId(),
                Position = location.Value
            };
            
            Collectables.Add(collectable);
            _world.PlayField.AddChild(collectable);
        }
            
        _diamondSpawnTimer -= Constants.DiamondSpawnRate;
    }

    private void SpawnRandomSpeedBoost(float deltaTime)
    {
        _speedBoostSpawnTimer += deltaTime;

        if (_speedBoostSpawnTimer < Constants.SpeedBoostSpawnRate)
            return;
        
        var speedBoostCount = 0;

        foreach (var collectable in Collectables)
        {
            if (collectable.Type == CollectableType.SpeedBoost)
                speedBoostCount++;
        }
        
        if (speedBoostCount >= Constants.MaxSpeedBoostLimit)
            return;

        var location = FindEmptyLocation();

        if (location != null)
        {
            var collectable = new Collectable(_assets.CollectableTexture, CollectableType.SpeedBoost)
            {
                Id = GetNextId(),
                Position = location.Value
            };
            
            Collectables.Add(collectable);
            _world.PlayField.AddChild(collectable);
        }
        
        _speedBoostSpawnTimer -= Constants.SpeedBoostSpawnRate;
    }

    private Vector2? FindEmptyLocation()
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

        foreach (var snake in Snakes)
        {
            if (snake.CollidesWith(rectangle)) return false;
        }

        foreach (var collectable in Collectables)
        {
            if (collectable.GetRectangle().Intersects(rectangle)) return false;
        }
        
        return true;
    }

    private void SpawnPlayerSnake()
    {
        if (_world.PlayerSnake != null)
            return;
        
        // Find best location
        var location = FindBestLocationForSnake();
        
        if (location == null)
            return;

        var direction = location.Value.X < (Constants.WallWidth * Constants.SegmentSize) / 2f ?
            SnakeDirection.Right : SnakeDirection.Left;

        SpawnPlayerSnake(location.Value, 2, direction);
    }

    private void SpawnEnemySnake(float deltaTime)
    {
        _enemySpawnTimer += deltaTime;
        
        var count = 0;

        foreach (var snake in Snakes)
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

        var direction = location.Value.X < (Constants.WallWidth * Constants.SegmentSize) / 2f
            ? SnakeDirection.Right
            : SnakeDirection.Left;
        
        SpawnEnemySnake(location.Value, 2, direction);

        _enemySpawnTimer -= Constants.EnemySpawnRate;
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

    private void DespawnSnake(float deltaTime)
    {
        foreach (var snake in Snakes)
        {
            if (snake.State == SnakeState.Dead)
            {
                if (snake.Reduce(deltaTime) && snake.Segments.Count > 0)
                {
                    SpawnSnakePart(snake);
                }

                if (snake.Segments.Count == 0)
                {
                    Snakes.Remove(snake);
                    snake.QueueRemove = true;
                    
                    if (snake is PlayerSnake)
                        _world.PlayerSnake = null;
                }
            }
        }
    }
    
    private void SpawnSnakePart(Snake snake)
    {
        var spawnSnakePart = _random.Next() % 3 == 0;

        if (spawnSnakePart)
        {
            var snakePart = new Collectable(_assets.CollectableTexture, CollectableType.SnakePart)
            {
                Id = GetNextId(),
                Position = snake.Segments[0].Position
            };

            Collectables.Add(snakePart);
            _world.PlayField.AddChild(snakePart);
        }
        else if (snake is EnemySnake) // Only enemies can spawn clocks
        {
            var spawnClock = _random.Next() % 6 == 0;

            if (spawnClock)
            {
                var clock = new Collectable(_assets.CollectableTexture, CollectableType.Clock)
                {
                    Id = GetNextId(),
                    Position = snake.Segments[0].Position
                };

                Collectables.Add(clock);
                _world.PlayField.AddChild(clock);
            }
        }
    }

    private int GetNextId()
    {
        return _lastEntityId++;
    }
}