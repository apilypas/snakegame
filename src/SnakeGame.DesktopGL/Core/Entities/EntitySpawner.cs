using System;
using System.Linq;
using Microsoft.Xna.Framework;
using SnakeGame.DesktopGL.Core.Events;

namespace SnakeGame.DesktopGL.Core.Entities;

public class EntitySpawner(GameWorld gameWorld)
{
    private readonly Random _random = new();

    private float _diamondSpawnTimer = 0f;
    private float _speedBoostSpawnTimer = 0f;
    private float _enemySpawnTimer = 0f;
    
    private int _lastEntityId = 0;

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
        
        for (var i = 0; i < gameWorld.Collectables.Count; i++)
        {
            var collectable = gameWorld.Collectables[i];

            var rectangle = new Rectangle(
                (int)collectable.Location.X,
                (int)collectable.Location.Y,
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
            var collectable = gameWorld.Collectables[at];
            gameWorld.Collectables.RemoveAt(at);
            gameWorld.EventManager.Notify(new NotifyEvent(collectable, snake, NotifyEventType.CollectableRemoved));
            return collectable;
        }

        return null;
    }

    private void SpawnRandomDiamond(float deltaTime)
    {
        _diamondSpawnTimer += deltaTime;
        
        if (gameWorld.Collectables.Count != 0 && _diamondSpawnTimer < Constants.DiamondSpawnRate)
            return;

        if (gameWorld.Collectables.Count(x => x.Type == CollectableType.Diamond) >= Constants.MaxDiamondLimit)
            return;

        var location = FindFreeLocation();

        if (location != null)
        {
            gameWorld.Collectables.Add(new Collectable
            {
                Id = GetNextId(),
                Type = CollectableType.Diamond,
                Location = location.Value
            });
        }
            
        _diamondSpawnTimer -= Constants.DiamondSpawnRate;
    }

    private void SpawnRandomSpeedBoost(float deltaTime)
    {
        _speedBoostSpawnTimer += deltaTime;

        if (_speedBoostSpawnTimer < Constants.SpeedBoostSpawnRate)
            return;
        
        if (gameWorld.Collectables.Count(x => x.Type == CollectableType.SpeedBoost) >= Constants.MaxSpeedBoostLimit)
            return;

        var location = FindFreeLocation();

        if (location != null)
        {
            gameWorld.Collectables.Add(new Collectable
            {
                Id = GetNextId(),
                Type = CollectableType.SpeedBoost,
                Location = location.Value
            });
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

        if (gameWorld.Snakes.Any(x => x.Intersects(rectangle)))
            return false;
        
        if (gameWorld.Collectables.Any(x => x.Location == location))
            return false;
        
        return true;
    }

    private void SpawnPlayerSnake()
    {
        var playerSnake = gameWorld.Snakes.SingleOrDefault(x => x is PlayerSnake);

        if (playerSnake != null)
            return;

        // Find best location
        var location = FindBestLocationForSnake();
        
        if (location == null)
            return;

        var direction = location.Value.X < (Constants.WallWidth * Constants.SegmentSize) / 2f ?
            SnakeDirection.Right : SnakeDirection.Left;
                
        playerSnake = new PlayerSnake(location.Value, 2, direction);
        playerSnake.Id = GetNextId();
        playerSnake.Initialize();
                
        gameWorld.Snakes.Add(playerSnake);
    }

    private void SpawnEnemySnake(float deltaTime)
    {
        _enemySpawnTimer += deltaTime;
        
        var count = gameWorld.Snakes.Count(x => x is EnemySnake);
        
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
                
        var enemySnake = new EnemySnake(location.Value, 2, direction, gameWorld);
        enemySnake.Id = GetNextId();
        enemySnake.Initialize();
                
        gameWorld.Snakes.Add(enemySnake);

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

    private void DespawnSnake(float deltaTime)
    {
        var deadSnakes = gameWorld.Snakes.Where(x => x.State == SnakeState.Dead).ToList();
        
        foreach (var snake in deadSnakes)
        {
            if (snake.Reduce(deltaTime) && snake.Segments.Count > 0)
                SpawnSnakePart(snake.Segments[0].Location);

            if (snake.Segments.Count == 0)
                gameWorld.Snakes.Remove(snake);
        }
    }
    
    private void SpawnSnakePart(Vector2 location)
    {
        var shouldSpawn = _random.Next() % 3 == 0;
        
        if (!shouldSpawn)
            return;

        var snakePart = new Collectable
        {
            Id = GetNextId(),
            Type = CollectableType.SnakePart,
            Location = location
        };
        gameWorld.Collectables.Add(snakePart);
    }

    private int GetNextId()
    {
        return _lastEntityId++;
    }
}