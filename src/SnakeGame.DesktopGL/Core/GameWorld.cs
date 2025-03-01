using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SnakeGame.DesktopGL.Core.Entities;

namespace SnakeGame.DesktopGL.Core;

public class GameWorld
{
    public IList<Snake> Snakes { get; private set; } = [];
    public EntitySpawner EntitySpawner { get; private set; }
    public int Score { get; private set; }
    public bool IsPaused { get; private set; }
    public bool HasGrid { get; private set; }

    public GameWorld()
    {
        EntitySpawner = new EntitySpawner(this);
        Snakes.Add(new PlayerSnake());
        Snakes.Add(new EnemySnake(this, new Vector2(100f, 20f)));
        Snakes.Add(new EnemySnake(this, new Vector2(100f, 60f)));
        Snakes.Add(new EnemySnake(this, new Vector2(100f, 100f)));
        Snakes.Add(new EnemySnake(this, new Vector2(100f, 140f)));
        Snakes.Add(new EnemySnake(this, new Vector2(100f, 180f)));
        IsPaused = false;
        HasGrid = false;
        Score = 0;
    }

    public void Initialize()
    {
        foreach (var snake in Snakes)
        {
            snake.Initialize();
        }
    }

    public void Update(float deltaTime)
    {
        if (IsPaused)
            return;

        foreach (var snake in Snakes)
        {
            if (snake.State == SnakeState.Alive)
            {
                snake.Update(deltaTime);

                var headRectangle = snake.Head.GetRectangle();

                if (EntitySpawner.KillBugAt(headRectangle))
                {
                    if (snake is PlayerSnake)
                        Score += Constants.BugKillScore;
                    snake.Grow();
                }

                if (EntitySpawner.KillSnakePartAt(headRectangle))
                {
                    if (snake is PlayerSnake)
                        Score += Constants.SnakePartKillScore;
                    snake.Grow();
                }

                if (EntitySpawner.KillSpeedBugAt(headRectangle))
                {
                    if (snake is PlayerSnake)
                        Score += Constants.SpeedBugKillScore;
                    snake.Grow();
                    snake.ResetSpeedUpTimer();
                }

                if (snake.IntersectsWithHead()
                    || !GetRectangle().Contains(headRectangle)
                    || Snakes.Any(x => x != snake && x.Intersects(headRectangle)))
                {
                    snake.Die();
                }
            }
        }

        foreach (var snake in Snakes)
        {
            if (snake.State == SnakeState.Dead)
            {
                // TODO: should we use observer pattern here?
                if (snake.Reduce(deltaTime))
                {
                    if (snake.Segments.Count > 0)
                        EntitySpawner.SpawnSnakePart(snake.Segments[0].Location);
                }

                if (snake.Segments.Count == 0)
                {
                    snake.Reset();
                }
            }
        }

        EntitySpawner.Update(deltaTime);
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
    }

    public void ToggleGrid()
    {
        HasGrid = !HasGrid;
    }

    public void SpeedUp()
    {
        Snakes[0].SpeedUp();
    }

    public void SpeedDown()
    {
        Snakes[0].SpeedDown();
    }

    public static Rectangle GetRectangle()
    {
        return new Rectangle(
            0,
            0,
            Constants.WallWidth * Constants.SegmentSize,
            Constants.WallHeight * Constants.SegmentSize
            );
    }

    public void ChangeDirection(SnakeDirection direction)
    {
        Snakes[0].ChangeDirection(direction);
    }
}