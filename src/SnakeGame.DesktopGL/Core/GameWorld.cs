using SnakeGame.DesktopGL.Core.Entities;

namespace SnakeGame.DesktopGL.Core;

public class GameWorld
{
    public Snake Snake { get; set; }
    public EntitySpawner EntitySpawner { get; set; }
    public int Score { get; set; }
    public bool IsPaused { get; private set; }
    public bool HasGrid { get; private set; }

    public GameWorld()
    {
        EntitySpawner = new EntitySpawner(this);
        Snake = new Snake();
        IsPaused = false;
        HasGrid = false;
        Score = 0;
    }

    public void Initialize()
    {
        Snake.Initialize();
    }

    public void Update(float deltaTime)
    {
        if (IsPaused)
            return;

        if (Snake.State == SnakeState.Alive)
        {
            Snake.Update(deltaTime);

            if (EntitySpawner.KillAt(Snake.Head.GetRectangle()))
            {
                Score += Constants.BugKillScore;
                Snake.Grow();
            }
            
            if (Snake.IntersectsWithHead() || Snake.IsOutOfBounds())
            {
                Snake.Die();
            }
        }

        if (Snake.State == SnakeState.Dead)
        {
            // TODO: should we use observer pattern here?
            if (Snake.Reduce(deltaTime))
            {
                if (Snake.Segments.Count > 0)
                    EntitySpawner.SpawnSnakePart(Snake.Segments[0].Location);
            }

            if (Snake.Segments.Count == 0)
            {
                Snake.Reset();
            }
        }

        EntitySpawner.UpdateLocations(deltaTime);
    }

    public void Pause()
    {
        IsPaused = !IsPaused;
    }

    public void ShowGrid()
    {
        HasGrid = !HasGrid;
    }
}