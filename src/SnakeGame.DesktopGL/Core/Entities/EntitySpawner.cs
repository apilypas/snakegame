using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SnakeGame.DesktopGL.Core.Entities;

public class EntitySpawner
{
    private GameWorld _gameWorld;

    private readonly Random _random;

    private float _bugSpawnTimer = 0f;

    public IList<Bug> _bugs = [];
    public IList<SnakePart> _snakeParts = [];

    public IList<Bug> Bugs => _bugs;
    public IList<SnakePart> SnakeParts => _snakeParts;

    public EntitySpawner(GameWorld gameWorld)
    {
        _random = new Random();
        _gameWorld = gameWorld;
    }

    public void UpdateLocations(float deltaTime)
    {
        if (_bugs.Count == 0)
        {
            SpawnRandomBug();
        }

        _bugSpawnTimer += deltaTime;
        if (_bugSpawnTimer >= Constants.BugSpawnRate)
        {
            SpawnRandomBug();
            _bugSpawnTimer -= Constants.BugSpawnRate;
        }
    }

    public bool KillAt(Rectangle targetRectangle)
    {
        var at = -1;

        for (var i = 0; i < _bugs.Count; i++)
        {
            var bug = _bugs[i];

            var rectangle = new Rectangle(
                (int)bug.Location.X,
                (int)bug.Location.Y,
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
            _bugs.RemoveAt(at);
            return true;
        }

        for (var i = 0; i < _snakeParts.Count; i++)
        {
            var snakePart = _snakeParts[i];

            var rectangle = new Rectangle(
                (int)snakePart.Location.X,
                (int)snakePart.Location.Y,
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
            _snakeParts.RemoveAt(at);
            return true;
        }

        return false;
    }

    private void SpawnRandomBug()
    {
        if (_bugs.Count >= Constants.MaxBugLimit)
            return;

        var random = _random.Next() % (Constants.WallWidth * Constants.WallHeight);
        
        for (var i = 0; i < Constants.WallHeight * Constants.WallWidth; i++)
        {
            var x = i % Constants.WallWidth;
            var y = i / Constants.WallHeight;
            if (i >= random) // If index is our random number
            {
                var location = new Vector2(x * Constants.SegmentSize, y * Constants.SegmentSize);
                if (!_gameWorld.Snake.Intersects(location))
                {
                    var bug = new Bug();
                    bug.Location = location;
                    _bugs.Add(bug);
                    break;
                }
            }
        }
    }

    public void SpawnSnakePart(Vector2 location)
    {
        var shouldSpawn = _random.Next() % 3 == 0;
        if (shouldSpawn)
        {
            var snakePart = new SnakePart();
            snakePart.Location = location;
            _snakeParts.Add(snakePart);
        }
    }
}