using System;
using Microsoft.Xna.Framework;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Events;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.Systems;

public class GameManager
{
    private enum GameWorldState
    {
        Running,
        Paused,
        Ended
    }
    
    private readonly EntityManager _entities;

    private float _timer = Constants.InitialTimer;
    
    public EventManager Events { get; } = new();

    private GameWorldState _state = GameWorldState.Running;
    
    public World World { get; }

    public GameManager(AssetManager assets)
    {
        World = new World(assets);
        _entities = new EntityManager(World, assets);
        
        Events.AddObserver(World.Score);
    }

    public void Initialize()
    {
        // Let's start initially with player and one enemy

        var playerAt = new Vector2(7f * Constants.SegmentSize, 20f * Constants.SegmentSize);
        var enemyAt = new Vector2(23f * Constants.SegmentSize, 20f * Constants.SegmentSize);
        
        _entities.SpawnPlayerSnake(
            playerAt,
            Constants.InitialSnakeSize,
            SnakeDirection.Up);
        
        _entities.SpawnEnemySnake(
            enemyAt,
            Constants.InitialSnakeSize,
            SnakeDirection.Up);
    }

    public void Update(GameTime gameTime)
    {
        World.IsPaused = _state == GameWorldState.Paused;
        World.UpdateEntityTree(gameTime);
        
        if (_state != GameWorldState.Running)
            return;
        
        HandleGameTimer(gameTime);
        HandleCollectables();
        HandleCollisions();

        _entities.Update(gameTime);
    }
    
    public void Faster()
    {
        if (_state != GameWorldState.Running)
            return;

        if (World.PlayerSnake is { State: SnakeState.Alive })
            World.PlayerSnake.Faster();
    }

    public void Slower()
    {
        if (_state != GameWorldState.Running)
            return;

        if (World.PlayerSnake is { State: SnakeState.Alive })
            World.PlayerSnake.Slower();
    }

    public void ChangeDirection(SnakeDirection direction)
    {
        if (_state != GameWorldState.Running)
            return;
        
        if (World.PlayerSnake is { State: SnakeState.Alive })
            World.PlayerSnake.UpdateDirection(direction);
    }

    public void TogglePause()
    {
        if (_state == GameWorldState.Running)
        {
            _state = GameWorldState.Paused;
            Events.Notify(new NotifyEvent(null, null, NotifyEventType.Paused));
        }
        else if (_state == GameWorldState.Paused)
        {
            _state = GameWorldState.Running;
            Events.Notify(new NotifyEvent(null, null, NotifyEventType.Resume));
        }
    }
    
    private void HandleGameTimer(GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        _timer -= deltaTime;

        if (_timer >= 0f)
        {
            Events.Notify(new NotifyTimerChangedEvent(_timer));
        }
        else
        {
            _state = GameWorldState.Ended;
            Events.Notify(new NotifyEvent(null, null, NotifyEventType.GameEnded));
        }
    }

    private void HandleCollisions()
    {
        foreach (var snake in _entities.Snakes)
        {
            if (snake.State != SnakeState.Alive)
                continue;

            var isDead = false;
            var headRectangle = snake.Head.GetRectangle();

            if (snake.CollidesWithSelf() || !Globals.PlayFieldRectangle.Contains(headRectangle))
            {
                isDead = true;
            }
            else
            {
                foreach (var otherSnake in _entities.Snakes)
                {
                    if (snake != otherSnake && otherSnake.CollidesWith(headRectangle))
                    {
                        isDead = true;
                        break;
                    }
                }
            }

            if (isDead)
            {
                snake.Die();
                Events.Notify(new NotifyEvent(snake, snake, NotifyEventType.SnakeDied));
            }
        }
    }

    private void HandleCollectables()
    {
        foreach (var snake in _entities.Snakes)
        {
            if (snake.State != SnakeState.Alive)
                continue;

            var headRectangle = snake.Head.GetRectangle();
            var collectable = _entities.GetCollectableAt(headRectangle);

            if (collectable != null)
            {
                HandleCollectableBonus(collectable, snake);

                collectable.QueueRemove = true;
                _entities.Collectables.Remove(collectable);
                Events.Notify(new NotifyEvent(collectable, snake, NotifyEventType.CollectableRemoved));
            }
        }
    }

    private void HandleCollectableBonus(Collectable collectable, Snake snake)
    {
        if (collectable.Type == CollectableType.Diamond)
        {
            snake.Grow();
            if (snake is PlayerSnake)
            {
                _entities.SpawnFadingText(snake.Head.Position, $"+{Constants.DiamondCollectScore}");
            }
        }
        else if (collectable.Type == CollectableType.SnakePart)
        {
            snake.Grow();
            if (snake is PlayerSnake)
            {
                _entities.SpawnFadingText(snake.Head.Position, $"+{Constants.SnakePartCollectScore}");
            }
        }
        else if (collectable.Type == CollectableType.SpeedBoost)
        {
            snake.Grow();
            snake.SpeedBoost(Constants.SpeedUpTimer);
            if (snake is PlayerSnake)
            {
                _entities.SpawnFadingText(snake.Head.Position, $"+{Constants.SpeedBoostCollectScore} (Speed)");
            }
        }
        else if (collectable.Type == CollectableType.Clock)
        {
            snake.Grow();
            if (snake is PlayerSnake)
            {
                _timer += Constants.ClockBonus;
                _timer = Math.Min(_timer, Constants.MaxTimer);
                
                _entities.SpawnFadingText(snake.Head.Position, $"+{Constants.ClockCollectScore} (Time)");
            }
        }
    }
}