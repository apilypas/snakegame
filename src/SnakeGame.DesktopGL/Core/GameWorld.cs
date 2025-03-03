using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SnakeGame.DesktopGL.Core.Entities;
using SnakeGame.DesktopGL.Core.Events;

namespace SnakeGame.DesktopGL.Core;

public class GameWorld
{
    public IList<Snake> Snakes { get; private set; } = [];
    public EntitySpawner EntitySpawner { get; private set; }
    
    public EventManager EventManager { get; } = new EventManager();
    
    public GameWorld()
    {
        EntitySpawner = new EntitySpawner(this);

        Snakes.Add(new PlayerSnake());
        Snakes.Add(new EnemySnake(this, new Vector2(100f, 20f)));
        Snakes.Add(new EnemySnake(this, new Vector2(100f, 60f)));
        Snakes.Add(new EnemySnake(this, new Vector2(100f, 100f)));
        Snakes.Add(new EnemySnake(this, new Vector2(100f, 140f)));
        Snakes.Add(new EnemySnake(this, new Vector2(100f, 180f)));
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
        foreach (var snake in Snakes)
        {
            if (snake.State == SnakeState.Alive)
            {
                snake.Update(deltaTime);

                var headRectangle = snake.Head.GetRectangle();

                var collectable = EntitySpawner.RemoveCollectable(snake);

                if (collectable != null)
                {
                    if (collectable.Type == CollectableType.Diamond)
                    {
                        snake.Grow();
                    }

                    if (collectable.Type == CollectableType.SnakePart)
                    {
                        snake.Grow();
                    }

                    if (collectable.Type == CollectableType.SpeedBoost)
                    {
                        snake.Grow();
                        snake.ResetSpeedUpTimer();
                    }
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
                if (snake.Reduce(deltaTime) && snake.Segments.Count > 0)
                    EntitySpawner.SpawnSnakePart(snake.Segments[0].Location);

                if (snake.Segments.Count == 0)
                    snake.Reset();
            }
        }

        EntitySpawner.Update(deltaTime);
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