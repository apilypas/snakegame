using System;
using System.Collections.Generic;
using System.Linq;
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
    public IList<FadeOutText> FadeOutTexts { get; } = [];
    
    public EventManager Events { get; } = new();

    private GameWorldState _state = GameWorldState.Running;
    
    public GameManager(AssetManager assets)
    {
        _assets = assets;
        _entitySpawner = new EntitySpawner(this, _assets);

        // Let's start initially with player and one enemy
        Snakes.Add(new PlayerSnake(
            assets,
            new Vector2(7f * Constants.SegmentSize, 20f * Constants.SegmentSize),
            Constants.InitialSnakeSize,
            SnakeDirection.Up
            ));
        Snakes.Add(new EnemySnake(
            assets,
            new Vector2(23f * Constants.SegmentSize, 20f * Constants.SegmentSize),
            Constants.InitialSnakeSize,
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
                snake.Update(deltaTime);

                var headRectangle = snake.Head.GetRectangle();

                var collectable = _entitySpawner.RemoveCollectable(snake);

                if (collectable != null)
                {
                    if (collectable.Type == CollectableType.Diamond)
                    {
                        snake.Grow();
                        if (snake is PlayerSnake)
                        {
                            FadeOutTexts.Add(new FadeOutText(_assets, $"+{Constants.DiamondCollectScore}")
                            {
                                Position = snake.Head.Position,
                            });
                        }
                    }

                    if (collectable.Type == CollectableType.SnakePart)
                    {
                        snake.Grow();
                        if (snake is PlayerSnake)
                        {
                            FadeOutTexts.Add(new FadeOutText(_assets, $"+{Constants.SnakePartCollectScore}")
                            {
                                Position = snake.Head.Position,
                            });
                        }
                    }

                    if (collectable.Type == CollectableType.SpeedBoost)
                    {
                        snake.Grow();
                        snake.ResetSpeedUpTimer();
                        if (snake is PlayerSnake)
                        {
                            FadeOutTexts.Add(new FadeOutText(_assets, $"+{Constants.SpeedBoostCollectScore} (Speed)")
                            {
                                Position = snake.Head.Position,
                            });
                        }
                    }

                    if (collectable.Type == CollectableType.Clock)
                    {
                        snake.Grow();
                        if (snake is PlayerSnake)
                        {
                            _timer += 30;
                            _timer = Math.Min(_timer, Constants.MaxTimer);
                            
                            FadeOutTexts.Add(new FadeOutText(_assets, $"+{Constants.ClockCollectScore} (Time)")
                            {
                                Position = snake.Head.Position,
                            });
                        }
                    }
                }

                if (snake.IntersectsWithHead()
                    || !GetRectangle().Contains(headRectangle)
                    || Snakes.Any(x => x != snake && x.Intersects(headRectangle)))
                {
                    snake.Die();
                    Events.Notify(new NotifyEvent(snake, snake, NotifyEventType.SnakeDied));
                }
            }
        }
        
        UpdateFadeOutTexts(gameTime);

        _entitySpawner.Update(deltaTime);
    }

    public void SpeedUp()
    {
        if (_state != GameWorldState.Running)
            return;
        
        var playerSnake = Snakes.SingleOrDefault(x => x is PlayerSnake && x.State == SnakeState.Alive);
        playerSnake?.SpeedUp();
    }

    public void SpeedDown()
    {
        if (_state != GameWorldState.Running)
            return;
        
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
        if (_state != GameWorldState.Running)
            return;
        
        var playerSnake = Snakes.SingleOrDefault(x => x is PlayerSnake && x.State == SnakeState.Alive);
        playerSnake?.ChangeDirection(direction);
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
    
    private void UpdateFadeOutTexts(GameTime gameTime)
    {
        var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        foreach (var fadeOutText in FadeOutTexts)
        {
            fadeOutText.TimeToLive -= elapsed;
            fadeOutText.Position += new Vector2(0f, -elapsed * 30f);
        }
        
        var oldTexts = FadeOutTexts.Where(x => x.TimeToLive <= 0f).ToList();

        foreach (var fadeOutText in oldTexts)
        {
            FadeOutTexts.Remove(fadeOutText);
        }
    }
}