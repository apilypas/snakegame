using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SnakeGame.DesktopGL.Core.Entities;
using SnakeGame.DesktopGL.Core.Events;

namespace SnakeGame.DesktopGL.Core;

public class GameWorld
{
    public enum GameWorldState
    {
        Running,
        Paused,
        Ended
    }
    
    private readonly EntitySpawner _entitySpawner;
    private float _timer = Constants.InitialTimer;
    
    public IList<Snake> Snakes { get; } = [];
    public IList<Collectable> Collectables { get; } = [];
    
    public EventManager EventManager { get; } = new();

    public GameWorldState State { get; private set; } = GameWorldState.Running;
    
    public GameWorld()
    {
        _entitySpawner = new EntitySpawner(this);

        // Let's start initially with player and one enemy
        Snakes.Add(new PlayerSnake(
            new Vector2(7f * Constants.SegmentSize, 20f * Constants.SegmentSize),
            3,
            SnakeDirection.Up
            ));
        Snakes.Add(new EnemySnake(
            new Vector2(23f * Constants.SegmentSize, 20f * Constants.SegmentSize),
            3,
            SnakeDirection.Up, this
            ));
    }

    public void Initialize()
    {
        foreach (var snake in Snakes)
        {
            snake.Initialize();
        }
    }

    public void Update(GameTime gameTime)
    {
        if (State != GameWorldState.Running)
            return;
        
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        _timer -= deltaTime;

        if (_timer >= 0f)
        {
            EventManager.Notify(new NotifyTimerChangedEvent(_timer));
        }
        else
        {
            State = GameWorldState.Ended;
            EventManager.Notify(new NotifyEvent(null, null, NotifyEventType.GameEnded));
        }
        
        foreach (var snake in Snakes)
        {
            if (snake.State == SnakeState.Alive)
            {
                snake.Update(deltaTime);

                var headRectangle = snake.Head.GetRectangle();

                var collectable = _entitySpawner.RemoveCollectable(snake);

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
                    EventManager.Notify(new NotifyEvent(snake, snake, NotifyEventType.SnakeDied));
                }
            }
        }

        _entitySpawner.Update(deltaTime);
    }

    public void SpeedUp()
    {
        var playerSnake = Snakes.SingleOrDefault(x => x is PlayerSnake && x.State == SnakeState.Alive);
        playerSnake?.SpeedUp();
    }

    public void SpeedDown()
    {
        var playerSnake = Snakes.SingleOrDefault(x => x is PlayerSnake && x.State == SnakeState.Alive);
        playerSnake?.SpeedDown();
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
        var playerSnake = Snakes.SingleOrDefault(x => x is PlayerSnake && x.State == SnakeState.Alive);
        playerSnake?.ChangeDirection(direction);
    }

    public void Pause()
    {
        if (State == GameWorldState.Running)
        {
            State = GameWorldState.Paused;
            EventManager.Notify(new NotifyEvent(null, null, NotifyEventType.Paused));
        }
    }

    public void Unpause()
    {
        if (State == GameWorldState.Paused)
        {
            State = GameWorldState.Running;
        }
    }
}