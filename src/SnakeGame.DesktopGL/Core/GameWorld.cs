using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SnakeGame.DesktopGL.Core.Entities;

namespace SnakeGame.DesktopGL.Core;

public class GameWorld
{
    public PlayerSnake PlayerSnake { get; private set; }
    public IList<EnemySnake> EnemySnakes { get; private set; } = [];
    public EntitySpawner EntitySpawner { get; private set; }
    public int Score { get; private set; }
    public bool IsPaused { get; private set; }
    public bool HasGrid { get; private set; }

    public GameWorld()
    {
        EntitySpawner = new EntitySpawner(this);
        PlayerSnake = new PlayerSnake();
        EnemySnakes.Add(new EnemySnake(this, new Vector2(100f, 20f)));
        EnemySnakes.Add(new EnemySnake(this, new Vector2(100f, 60f)));
        EnemySnakes.Add(new EnemySnake(this, new Vector2(100f, 100f)));
        EnemySnakes.Add(new EnemySnake(this, new Vector2(100f, 140f)));
        EnemySnakes.Add(new EnemySnake(this, new Vector2(100f, 180f)));
        IsPaused = false;
        HasGrid = false;
        Score = 0;
    }

    public void Initialize()
    {
        PlayerSnake.Initialize();

        foreach (var enemySnake in EnemySnakes)
        {
            enemySnake.Initialize();
        }
    }

    public void Update(float deltaTime)
    {
        if (IsPaused)
            return;

        if (PlayerSnake.State == SnakeState.Alive)
        {
            PlayerSnake.Update(deltaTime);

            var headRectangle = PlayerSnake.Head.GetRectangle();

            if (EntitySpawner.KillBugAt(headRectangle))
            {
                Score += Constants.BugKillScore;
                PlayerSnake.Grow();
            }

            if (EntitySpawner.KillSnakePartAt(headRectangle))
            {
                Score += Constants.SnakePartKillScore;
                PlayerSnake.Grow();
            }

            if (EntitySpawner.KillSpeedBugAt(headRectangle))
            {
                Score += Constants.SpeedBugKillScore;
                PlayerSnake.Grow();
                PlayerSnake.ResetSpeedUpTimer();
            }

            if (PlayerSnake.IntersectsWithHead()
                || !GetRectangle().Contains(headRectangle)
                || EnemySnakes.Any(x => x.Intersects(headRectangle)))
            {
                PlayerSnake.Die();
            }
        }

        foreach (var enemySnake in EnemySnakes)
        {
            if (enemySnake.State == SnakeState.Alive)
            {
                enemySnake.Update(deltaTime);

                var headRectangle = enemySnake.Head.GetRectangle();

                if (EntitySpawner.KillBugAt(headRectangle))
                {
                    enemySnake.Grow();
                }

                if (EntitySpawner.KillSnakePartAt(headRectangle))
                {
                    enemySnake.Grow();
                }

                if (EntitySpawner.KillSpeedBugAt(headRectangle))
                {
                    enemySnake.Grow();
                    enemySnake.ResetSpeedUpTimer();
                }

                if (enemySnake.IntersectsWithHead()
                    || !GetRectangle().Contains(headRectangle)
                    || PlayerSnake.Intersects(headRectangle)
                    || EnemySnakes.Any(x => x != enemySnake && x.Intersects(headRectangle)))
                {
                    enemySnake.Die();
                }
            }
        }

        if (PlayerSnake.State == SnakeState.Dead)
        {
            // TODO: should we use observer pattern here?
            if (PlayerSnake.Reduce(deltaTime))
            {
                if (PlayerSnake.Segments.Count > 0)
                    EntitySpawner.SpawnSnakePart(PlayerSnake.Segments[0].Location);
            }

            if (PlayerSnake.Segments.Count == 0)
            {
                PlayerSnake.Reset();
            }
        }

        foreach (var enemySnake in EnemySnakes)
        {
            if (enemySnake.State == SnakeState.Dead)
            {
                // TODO: should we use observer pattern here?
                if (enemySnake.Reduce(deltaTime))
                {
                    if (enemySnake.Segments.Count > 0)
                        EntitySpawner.SpawnSnakePart(enemySnake.Segments[0].Location);
                }

                if (enemySnake.Segments.Count == 0)
                {
                    enemySnake.Reset();
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
        PlayerSnake.SpeedUp();
    }

    public void SpeedDown()
    {
        PlayerSnake.SpeedDown();
    }

    public Rectangle GetRectangle()
    {
        return new Rectangle(
            0,
            0,
            Constants.WallWidth * Constants.SegmentSize,
            Constants.WallHeight * Constants.SegmentSize
            );
    }
}