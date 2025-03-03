using System;
using System.Linq;
using Microsoft.Xna.Framework;
using SnakeGame.DesktopGL.Core.Events;

namespace SnakeGame.DesktopGL.Core.Entities;

public class EntitySpawner
{
    private readonly GameWorld _gameWorld;
    private readonly Random _random;

    private float _diamondSpawnTimer = 0f;
    private float _speedBoostSpawnTimer = 0f;

    public EntitySpawner(GameWorld gameWorld)
    {
        _random = new Random();
        _gameWorld = gameWorld;
    }

    public void Update(float deltaTime)
    {
        if (_gameWorld.Collectables.Count == 0)
        {
            SpawnRandomDiamond();
        }

        _diamondSpawnTimer += deltaTime;
        if (_diamondSpawnTimer >= Constants.DiamondSpawnRate)
        {
            SpawnRandomDiamond();
            _diamondSpawnTimer -= Constants.DiamondSpawnRate;
        }

        _speedBoostSpawnTimer += deltaTime;
        if (_speedBoostSpawnTimer >= Constants.SpeedBoostSpawnRate)
        {
            SpawnRandomSpeedBoost();
            _speedBoostSpawnTimer -= Constants.SpeedBoostSpawnRate;
        }
    }

    public void SpawnSnakePart(Vector2 location)
    {
        var shouldSpawn = _random.Next() % 3 == 0;
        if (shouldSpawn)
        {
            var snakePart = new Collectable
            {
                Type = CollectableType.SnakePart,
                Location = location
            };
            _gameWorld.Collectables.Add(snakePart);
        }
    }

    public Collectable RemoveCollectable(Snake snake)
    {
        var targetRectangle = snake.Head.GetRectangle();
        var at = -1;
        
        for (var i = 0; i < _gameWorld.Collectables.Count; i++)
        {
            var collectable = _gameWorld.Collectables[i];

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
            var collectable = _gameWorld.Collectables[at];
            _gameWorld.Collectables.RemoveAt(at);
            _gameWorld.EventManager.Notify(new NotifyEvent(collectable, snake, NotifyEventType.CollectableRemoved));
            return collectable;
        }

        return null;
    }

    private void SpawnRandomDiamond()
    {
        if (_gameWorld.Collectables.Count(x => x.Type == CollectableType.Diamond) >= Constants.MaxDiamondLimit)
            return;

        var location = FindFreeLocation();

        if (location != null)
        {
            _gameWorld.Collectables.Add(new Collectable
            {
                Type = CollectableType.Diamond,
                Location = location.Value
            });
        }
    }

    private void SpawnRandomSpeedBoost()
    {
        if (_gameWorld.Collectables.Count(x => x.Type == CollectableType.SpeedBoost) >= Constants.MaxSpeedBoostLimit)
            return;

        var location = FindFreeLocation();

        if (location != null)
        {
            _gameWorld.Collectables.Add(new Collectable
            {
                Type = CollectableType.SpeedBoost,
                Location = location.Value
            });
        }
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

        if (_gameWorld.Snakes.Any(x => x.Intersects(rectangle)))
            return false;
        
        if (_gameWorld.Collectables.Any(x => x.Location == location))
            return false;
        
        return true;
    }
}