using System;
using Microsoft.Xna.Framework;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Enums;
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
    private int _timerRounded = (int)Constants.InitialTimer;
    private float _scoreMultiplicatorTimer;
    private float _invincibilityTimer;
    private int _scoreMultiplicator = 1;

    public EventBus EventBus { get; } = new();

    private GameWorldState _state = GameWorldState.Running;

    public World World { get; }
    public int Score { get; private set; }
    public int Deaths { get; private set; }
    public int LongestSnake { get; private set; } = 3;
    public float TotalTime { get; private set; }

    public GameManager(AssetManager assets)
    {
        World = new World(assets, EventBus);
        _entities = new EntityManager(World, assets);
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
        World.PlayField.IsPaused = _state != GameWorldState.Running;
        World.UpdateEntityTree(gameTime);
        
        if (_state != GameWorldState.Running)
            return;
        
        HandleGameTimer(gameTime);
        HandleScoreMultiplicator(gameTime);
        HandleInvincibility(gameTime);
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
            EventBus.Publish(new PausedEvent());
        }
        else if (_state == GameWorldState.Paused)
        {
            _state = GameWorldState.Running;
            EventBus.Publish(new ResumeEvent());
        }
    }
    
    private void HandleGameTimer(GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        _timer -= deltaTime;
        TotalTime += deltaTime;

        if (_timer >= 0f)
        {
            if ((int)_timer != _timerRounded)
            {
                _timerRounded = (int)_timer;
                EventBus.Publish(new TimerChangedEvent { Timer = _timerRounded });
            }
        }
        else
        {
            _state = GameWorldState.Ended;
            EventBus.Publish(new GameEndedEvent());
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
                        if (snake.IsInvincible)
                            otherSnake.Die();
                        else
                            isDead = true;
                        break;
                    }
                }
            }

            if (isDead)
            {
                snake.Die();

                if (snake is PlayerSnake)
                {
                    Deaths++;
                    _scoreMultiplicator = 1;
                    EventBus.Publish(new PlayerDiedEvent { TotalDeaths = Deaths });
                    EventBus.Publish(new ScoreMultiplicatorChangedEvent { ScoreMultiplicator = _scoreMultiplicator });
                }
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

                if (snake is PlayerSnake)
                {
                    EventBus.Publish(new ScoreChangedEvent { Score = Score });
                }
            }
        }

        if (World.PlayerSnake != null && World.PlayerSnake.Segments.Count > LongestSnake)
        {
            LongestSnake = World.PlayerSnake.Segments.Count;
            EventBus.Publish(new LongestSnakeChanged { Length = LongestSnake });
        }
    }

    private void HandleCollectableBonus(Collectable collectable, Snake snake)
    {
        if (collectable.Type == CollectableType.Diamond)
        {
            snake.Grow();
            if (snake is PlayerSnake)
            {
                var score = _scoreMultiplicator * Constants.DiamondCollectScore;
                Score += score;
                _entities.SpawnFadingText(snake.Head.Position, $"+{score}");
            }
        }
        else if (collectable.Type == CollectableType.SnakePart)
        {
            snake.Grow();
            if (snake is PlayerSnake)
            {
                var score = _scoreMultiplicator * Constants.SnakePartCollectScore;
                Score += score;
                _entities.SpawnFadingText(snake.Head.Position, $"+{score}");
            }
        }
        else if (collectable.Type == CollectableType.SpeedBoost)
        {
            snake.Grow();
            snake.SpeedBoost(Constants.SpeedUpTimer);
            if (snake is PlayerSnake)
            {
                var score = _scoreMultiplicator * Constants.SpeedBoostCollectScore;
                Score += score;
                _entities.SpawnFadingText(snake.Head.Position, $"+{score} (+Speed)");
            }
        }
        else if (collectable.Type == CollectableType.Crown)
        {
            if (snake is PlayerSnake)
            {
                snake.IsInvincible = true;
                _invincibilityTimer = Constants.InvincibleTimer;
                var score = _scoreMultiplicator * Constants.CrownCollectScore;
                Score += score;
                _entities.SpawnFadingText(snake.Head.Position, $"+{score} (+Invincible)");
            }
        }
        else if (collectable.Type == CollectableType.Clock)
        {
            snake.Grow();
            if (snake is PlayerSnake)
            {
                _timer += Constants.ClockBonus;
                _timer = Math.Min(_timer, Constants.MaxTimer);
                
                var score = _scoreMultiplicator * Constants.ClockCollectScore;
                Score += score;
                _entities.SpawnFadingText(snake.Head.Position, $"+{score} (+Time)");
            }
        }
    }
    
    private void HandleScoreMultiplicator(GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        _scoreMultiplicatorTimer += deltaTime;

        if (_scoreMultiplicatorTimer >= Constants.ScoreMultiplicatorTimer)
        {
            _scoreMultiplicatorTimer -= Constants.ScoreMultiplicatorTimer;
            _scoreMultiplicator = MathHelper.Clamp(_scoreMultiplicator + 1, 1, Constants.MaxScoreMultiplier);
            EventBus.Publish(new ScoreMultiplicatorChangedEvent { ScoreMultiplicator = _scoreMultiplicator });
        }
    }
    
    
    private void HandleInvincibility(GameTime gameTime)
    {
        if (_invincibilityTimer <= 0)
            return;
        
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        _invincibilityTimer -= deltaTime;

        if (_invincibilityTimer <= 0)
            World.PlayerSnake.IsInvincible = false;
    }
}