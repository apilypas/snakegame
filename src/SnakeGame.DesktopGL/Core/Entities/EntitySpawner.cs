using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SnakeGame.DesktopGL.Core.Entities;

public class EntitySpawner
{
    private GameWorld _gameWorld;

    private readonly Random _random;

    private float _bugSpawnTimer = 0f;
    private float _speedBugSpawnTimer = 0f;

    private IList<Bug> _bugs = [];
    private IList<SnakePart> _snakeParts = [];
    private IList<SpeedBug> _speedBugs = []; 

    public IList<Bug> Bugs => _bugs;
    public IList<SnakePart> SnakeParts => _snakeParts;
    public IList<SpeedBug> SpeedBugs => _speedBugs;

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

        _speedBugSpawnTimer += deltaTime;
        if (_speedBugSpawnTimer >= Constants.SpeedBugSpawnRate)
        {
            SpawnRandomSpeedBug();
            _speedBugSpawnTimer -= Constants.SpeedBugSpawnRate;
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

    public bool KillBugAt(Rectangle targetRectangle)
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

        return false;
    }

    public bool KillSnakePartAt(Rectangle targetRectangle)
    {
        var at = -1;

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

    public bool KillSpeedBugAt(Rectangle targetRectangle)
    {
        var at = -1;

        for (var i = 0; i < _speedBugs.Count; i++)
        {
            var speedBug = _speedBugs[i];

            var rectangle = new Rectangle(
                (int)speedBug.Location.X,
                (int)speedBug.Location.Y,
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
            _speedBugs.RemoveAt(at);
            return true;
        }

        return false;
    }

    private void SpawnRandomBug()
    {
        if (_bugs.Count >= Constants.MaxBugLimit)
            return;

        var bugLocation = GetRandomFreeLocation();

        if (bugLocation != null)
        {
            _bugs.Add(new Bug
            {
                Location = bugLocation.Value
            });
        }
    }

    public void SpawnRandomSpeedBug()
    {
        if (_speedBugs.Count >= Constants.MaxSpeedBugLimit)
            return;

        var speedBugLocation = GetRandomFreeLocation();

        if (speedBugLocation != null)
        {
            _speedBugs.Add(new SpeedBug
            {
                Location = speedBugLocation.Value
            });
        }
    }

    private Vector2? GetRandomFreeLocation()
    {
        var random = _random.Next() % (Constants.WallWidth * Constants.WallHeight);

        for (var i = 0; i < Constants.WallHeight * Constants.WallWidth; i++)
        {
            if (i >= random) // If index is our random number
            {
                var x = i % Constants.WallWidth;
                var y = i / Constants.WallHeight;
                var location = new Vector2(x * Constants.SegmentSize, y * Constants.SegmentSize);
                if (!_gameWorld.Snake.Intersects(location))
                {
                    return location;
                }
            }
        }

        return null;
    }
}