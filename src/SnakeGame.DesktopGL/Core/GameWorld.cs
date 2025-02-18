using System;
using SnakeGame.DesktopGL.Core.Entities;

namespace SnakeGame.DesktopGL.Core;

public class GameWorld
{
    public Snake Snake { get; set; }
    public BugSpawner BugSpawner { get; set; }
    public int Score { get; set; }
    public bool IsPaused { get; set; }
    public bool IsEnded { get; set;}

    public GameWorld()
    {
        BugSpawner = new BugSpawner(this);
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
            
            BugSpawner.UpdateLocations(deltaTime);
            BugSpawner.KillBugs();

            if (Snake.IntersectsWithHead() || Snake.IsOutOfBounds())
            {
                Console.WriteLine("Game Ended");
                IsEnded = true;
            }
        }
    }
}