using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SnakeGame.DesktopGL.Core.Entities;

public class BugSpawner
{
    private GameWorld _gameWorld;

    private readonly Random _random;
    private float _bugSpawnTimer = 0f;

    public IList<Vector2> _locations = [];

    public IList<Vector2> Locations => _locations;

    public BugSpawner(GameWorld gameWorld)
    {
        _random = new Random();
        _gameWorld = gameWorld;
    }

    public void UpdateLocations(float deltaTime)
    {
        if (_locations.Count == 0)
        {
            AddRandomBug();
        }

        _bugSpawnTimer += deltaTime;
        if (_bugSpawnTimer >= Constants.BugSpawnRate)
        {
            AddRandomBug();
            _bugSpawnTimer -= Constants.BugSpawnRate;
        }
    }

    public void KillBugs()
    {
        if (KillAt(_gameWorld.Snake.Head.GetRectangle()))
        {
            _gameWorld.Score += Constants.BugKillScore;
            _gameWorld.Snake.Grow();
        }
    }

    private void AddRandomBug()
    {
        var x = _random.Next() % Constants.WallWidth;
        var y = _random.Next() % Constants.WallHeight;
        var location = new Vector2(x * Constants.SegmentSize, y * Constants.SegmentSize);
        _locations.Add(location);
    }

    private bool KillAt(Rectangle targetRectangle)
    {
        var at = -1;

        for (var i = 0; i < _locations.Count; i++)
        {
            var bug = _locations[i];

            var rectangle = new Rectangle(
                (int)bug.X,
                (int)bug.Y,
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
            _locations.RemoveAt(at);
            return true;
        }

        return false;
    }
}