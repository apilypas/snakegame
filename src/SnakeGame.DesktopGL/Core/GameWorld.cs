using System;
using Microsoft.Xna.Framework;
using SnakeGame.DesktopGL.Core.Entities;

namespace SnakeGame.DesktopGL.Core;

public class GameWorld
{
    public Snake Snake { get; set; }
    public int Score { get; set; }
    public bool IsPaused { get; set; }
    public bool IsEnded { get; set;}
    public BugSpawner BugSpawner { get; set; }

    private readonly Random _random;
    private float _bugSpawnTimer = 0f;

    public GameWorld()
    {
        _random = new Random();
        BugSpawner = new BugSpawner();
        Snake = new Snake();
        IsPaused = false;
        Score = 0;
    }

    public void Initialize()
    {
        Snake.Initialize();
    }

    public void Update(float deltaTime)
    {
        if (!IsPaused && !IsEnded)
        {
            Snake.Move(deltaTime);
        
            if (!BugSpawner.Any())
            {
                AddRandomBug();
            }

            _bugSpawnTimer += deltaTime;
            if (_bugSpawnTimer >= Constants.BugSpawnRate)
            {
                AddRandomBug();
                _bugSpawnTimer -= Constants.BugSpawnRate;
            }

            if (BugSpawner.Kill(Snake.Head.Location))
            {
                Score++;
                Snake.Grow();
            }

            if (Snake.IntersectsWithHead() || Snake.IsOutOfBounds())
            {
                Console.WriteLine("Game Ended");
                IsEnded = true;
            }
        }
    }

    private void AddRandomBug()
    {
        var x = _random.Next() % Constants.WallWidth;
        var y = _random.Next() % Constants.WallHeight;
        var location = new Vector2(x * Constants.SegmentSize, y * Constants.SegmentSize);
        BugSpawner.SpawnAt(location);
    }
}