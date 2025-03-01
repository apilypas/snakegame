using System;
using System.Linq;
using Microsoft.Xna.Framework;
using SnakeGame.DesktopGL.Core.Entities;

namespace SnakeGame.DesktopGL.Core;

public class GameWorld
{
    public PlayerSnake PlayerSnake { get; private set; }
    public EnemySnake EnemySnake { get; private set; }
    public EntitySpawner EntitySpawner { get; private set; }
    public int Score { get; private set; }
    public bool IsPaused { get; private set; }
    public bool HasGrid { get; private set; }

    public GameWorld()
    {
        EntitySpawner = new EntitySpawner(this);
        PlayerSnake = new PlayerSnake();
        EnemySnake = new EnemySnake(this);
        IsPaused = false;
        HasGrid = false;
        Score = 0;
    }

    public void Initialize()
    {
        PlayerSnake.Initialize();
        EnemySnake.Initialize();
    }

    public void Update(float deltaTime)
    {
        if (IsPaused)
            return;

        if (PlayerSnake.State == SnakeState.Alive)
        {
            PlayerSnake.Update(deltaTime);

            if (EntitySpawner.KillBugAt(PlayerSnake.Head.GetRectangle()))
            {
                Score += Constants.BugKillScore;
                PlayerSnake.Grow();
            }

            if (EntitySpawner.KillSnakePartAt(PlayerSnake.Head.GetRectangle()))
            {
                Score += Constants.SnakePartKillScore;
                PlayerSnake.Grow();
            }

            if (EntitySpawner.KillSpeedBugAt(PlayerSnake.Head.GetRectangle()))
            {
                Score += Constants.SpeedBugKillScore;
                PlayerSnake.Grow();
                PlayerSnake.ResetSpeedUpTimer();
            }

            if (PlayerSnake.IntersectsWithHead()
                || !GetRectangle().Contains(PlayerSnake.Head.GetRectangle())
                || EnemySnake.Intersects(PlayerSnake.Head.GetRectangle()))
            {
                PlayerSnake.Die();
            }
        }

        if (EnemySnake.State == SnakeState.Alive)
        {
            EnemySnake.Update(deltaTime);

            if (EntitySpawner.KillBugAt(EnemySnake.Head.GetRectangle()))
            {
                EnemySnake.Grow();
            }

            if (EntitySpawner.KillSnakePartAt(EnemySnake.Head.GetRectangle()))
            {
                EnemySnake.Grow();
            }

            if (EntitySpawner.KillSpeedBugAt(EnemySnake.Head.GetRectangle()))
            {
                EnemySnake.Grow();
                EnemySnake.ResetSpeedUpTimer();
            }

            if (EnemySnake.IntersectsWithHead()
                || !GetRectangle().Contains(EnemySnake.Head.GetRectangle())
                || PlayerSnake.Intersects(EnemySnake.Head.GetRectangle()))
            {
                EnemySnake.Die();
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

        if (EnemySnake.State == SnakeState.Dead)
        {
            // TODO: should we use observer pattern here?
            if (EnemySnake.Reduce(deltaTime))
            {
                if (EnemySnake.Segments.Count > 0)
                    EntitySpawner.SpawnSnakePart(EnemySnake.Segments[0].Location);
            }

            if (EnemySnake.Segments.Count == 0)
            {
                EnemySnake.Reset();
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