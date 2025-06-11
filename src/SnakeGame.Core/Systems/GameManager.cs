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

        // Let's start initially with player and one enemy
        var playerSnake = new PlayerSnake(
            assets,
            new Vector2(7f * Constants.SegmentSize, 20f * Constants.SegmentSize),
            Constants.InitialSnakeSize,
            SnakeDirection.Up
            );
        Snakes.Add(playerSnake);
        World.PlayField.Add(playerSnake);
        
        var enemySnake = new EnemySnake(
            assets,
            new Vector2(23f * Constants.SegmentSize, 20f * Constants.SegmentSize),
            Constants.InitialSnakeSize,
            SnakeDirection.Up, this
            );
        Snakes.Add(enemySnake);
        World.PlayField.Add(enemySnake);
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
        UpdateEntities(World, gameTime);
        
        World.IsPaused = _state == GameWorldState.Paused;
        
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
            if (snake.State == SnakeState.Alive)
            {
                var collectable = _entitySpawner.RemoveCollectable(snake);

                if (collectable != null)
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

                    if (collectable.Type == CollectableType.SnakePart)
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

                    if (collectable.Type == CollectableType.SpeedBoost)
                    {
                        snake.Grow();
                        snake.ResetSpeedUpTimer();
                        if (snake is PlayerSnake)
                        {
                            var fadingText = new FadingText($"+{Constants.SpeedBoostCollectScore} (Speed)", _assets.MainFont)
                            {
                                Position = snake.Head.Position
                            };

                            World.PlayField.Add(fadingText);
                        }
                    }

                    if (collectable.Type == CollectableType.Clock)
                    {
                        snake.Grow();
                        if (snake is PlayerSnake)
                        {
                            _timer += 30;
                            _timer = Math.Min(_timer, Constants.MaxTimer);
                            
                            var fadingText = new FadingText($"+{Constants.ClockCollectScore} (Time)", _assets.MainFont)
                            {
                                Position = snake.Head.Position
                            };

                            World.PlayField.Add(fadingText);
                        }
                    }
                }

                var headRectangle = snake.Head.GetRectangle();

                if (snake.IntersectsWithHead())
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
                        if (snake != otherSnake && otherSnake.Intersects(headRectangle))
                        {
                            snake.Die();
                            Events.Notify(new NotifyEvent(snake, snake, NotifyEventType.SnakeDied));
                        }
                    }
                }
            }
        }

        _entitySpawner.Update(deltaTime);
    }

    private void UpdateEntities(Entity entity, GameTime gameTime)
    {
        if (entity.IsPaused) return;
        
        entity.Update(gameTime);

        foreach (var child in entity.Children)
        {
            if (child.QueueRemove)
            {
                entity.Remove(child);
                continue;
            }
            
            UpdateEntities(child, gameTime);
        }
    }

    public void SpeedUp()
    {
        if (_state != GameWorldState.Running)
            return;

        foreach (var snake in Snakes)
        {
            if (snake is PlayerSnake playerSnake && snake.State == SnakeState.Alive)
            {
                playerSnake.SpeedUp();
            }
        }
    }

    public void SpeedDown()
    {
        if (_state != GameWorldState.Running)
            return;

        foreach (var snake in Snakes)
        {
            if (snake is PlayerSnake playerSnake && snake.State == SnakeState.Alive)
            {
                playerSnake.SpeedDown();
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
                playerSnake.ChangeDirection(direction);
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
}