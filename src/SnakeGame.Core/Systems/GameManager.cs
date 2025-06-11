using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Events;

namespace SnakeGame.Core.Systems;

public class GameManager
{
    private enum GameWorldState
    {
        Running,
        Paused,
        Ended
    }
    
    private readonly EntitySpawner _entitySpawner;
    private float _timer = Constants.InitialTimer;
    private readonly AssetManager _assets;

    public IList<Snake> Snakes { get; } = [];
    public IList<Collectable> Collectables { get; } = [];
    
    public EventManager Events { get; } = new();

    private GameWorldState _state = GameWorldState.Running;
    
    public World World { get; }

    public GameManager(AssetManager assets)
    {
        _assets = assets;
        _entitySpawner = new EntitySpawner(this, _assets);
        
        World = new World(_assets);
    }

    public void Initialize()
    {
        // Let's start initially with player and one enemy
        var playerSnake = new PlayerSnake(
            _assets,
            new Vector2(7f * Constants.SegmentSize, 20f * Constants.SegmentSize),
            Constants.InitialSnakeSize,
            SnakeDirection.Up
            );
        Snakes.Add(playerSnake);
        World.PlayField.Add(playerSnake);
        
        var enemySnake = new EnemySnake(
            _assets,
            new Vector2(23f * Constants.SegmentSize, 20f * Constants.SegmentSize),
            Constants.InitialSnakeSize,
            SnakeDirection.Up, this
            );
        Snakes.Add(enemySnake);
        World.PlayField.Add(enemySnake);
        
        foreach (var snake in Snakes)
        {
            snake.Initialize();
        }
    }

    public void Update(GameTime gameTime)
    {
        World.IsPaused = _state == GameWorldState.Paused;
        
        UpdateEntities(World, Vector2.Zero, gameTime);
        
        if (_state != GameWorldState.Running)
            return;
        
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
        
        foreach (var snake in Snakes)
        {
            if (snake.State != SnakeState.Alive)
                continue;

            var headRectangle = snake.Head.GetRectangle();
            
            // Handle collectables
            
            var collectable = GetCollectableAt(headRectangle);

            if (collectable != null)
            {
                HandleCollectableBonus(collectable, snake);

                collectable.QueueRemove = true;
                Collectables.Remove(collectable);
                Events.Notify(new NotifyEvent(collectable, snake, NotifyEventType.CollectableRemoved));
            }
            
            // Handle collisions

            if (snake.CollidesWithSelf())
            {
                snake.Die();
                Events.Notify(new NotifyEvent(snake, snake, NotifyEventType.SnakeDied));
            }
            else if (!GetRectangle().Contains(headRectangle))
            {
                snake.Die();
                Events.Notify(new NotifyEvent(snake, snake, NotifyEventType.SnakeDied));
            }
            else
            {
                foreach (var otherSnake in Snakes)
                {
                    if (snake != otherSnake && otherSnake.CollidesWith(headRectangle))
                    {
                        snake.Die();
                        Events.Notify(new NotifyEvent(snake, snake, NotifyEventType.SnakeDied));
                    }
                }
            }
        }

        _entitySpawner.Update(deltaTime);
    }
    
    public void Faster()
    {
        if (_state != GameWorldState.Running)
            return;

        foreach (var snake in Snakes)
        {
            if (snake is PlayerSnake playerSnake && snake.State == SnakeState.Alive)
            {
                playerSnake.Faster();
            }
        }
    }

    public void Slower()
    {
        if (_state != GameWorldState.Running)
            return;

        foreach (var snake in Snakes)
        {
            if (snake is PlayerSnake playerSnake && snake.State == SnakeState.Alive)
            {
                playerSnake.Slower();
            }
        }
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
        if (_state != GameWorldState.Running)
            return;

        foreach (var snake in Snakes)
        {
            if (snake is PlayerSnake playerSnake && snake.State == SnakeState.Alive)
            {
                playerSnake.UpdateDirection(direction);
            }
        }
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

    private void HandleCollectableBonus(Collectable collectable, Snake snake)
    {
        if (collectable.Type == CollectableType.Diamond)
        {
            snake.Grow();
            if (snake is PlayerSnake)
            {
                var fadingText = new FadingText($"+{Constants.DiamondCollectScore}", _assets.MainFont)
                {
                    Position = snake.Head.Position
                };

                World.PlayField.Add(fadingText);
            }
        }
        else if (collectable.Type == CollectableType.SnakePart)
        {
            snake.Grow();
            if (snake is PlayerSnake)
            {
                var fadingText = new FadingText($"+{Constants.SnakePartCollectScore}", _assets.MainFont)
                {
                    Position = snake.Head.Position
                };

                World.PlayField.Add(fadingText);
            }
        }
        else if (collectable.Type == CollectableType.SpeedBoost)
        {
            snake.Grow();
            snake.SpeedBoost(Constants.SpeedUpTimer);
                        
            if (snake is PlayerSnake)
            {
                var fadingText = new FadingText($"+{Constants.SpeedBoostCollectScore} (Speed)", _assets.MainFont)
                {
                    Position = snake.Head.Position
                };

                World.PlayField.Add(fadingText);
            }
        }
        else if (collectable.Type == CollectableType.Clock)
        {
            snake.Grow();
            if (snake is PlayerSnake)
            {
                _timer += Constants.ClockBonus;
                _timer = Math.Min(_timer, Constants.MaxTimer);
                            
                var fadingText = new FadingText($"+{Constants.ClockCollectScore} (Time)", _assets.MainFont)
                {
                    Position = snake.Head.Position
                };

                World.PlayField.Add(fadingText);
            }
        }
    }

    private void UpdateEntities(Entity entity, Vector2 basePosition, GameTime gameTime)
    {
        if (entity.IsPaused) return;

        entity.IsUpdated = false;
        entity.Update(gameTime);
        entity.GlobalPosition = basePosition + entity.Position;
        entity.IsUpdated = true;

        foreach (var child in entity.Children)
        {
            if (child.QueueRemove)
            {
                entity.Remove(child);
                continue;
            }
            
            UpdateEntities(child, entity.GlobalPosition, gameTime);
        }
    }
    
    private Collectable GetCollectableAt(Rectangle targetRectangle)
    {
        foreach (var collectable in Collectables)
        {
            var rectangle = new Rectangle(
                (int)collectable.Position.X,
                (int)collectable.Position.Y,
                Constants.SegmentSize,
                Constants.SegmentSize);

            if (rectangle.Intersects(targetRectangle))
            {
                return collectable;
            }
        }

        return null;
    }
}