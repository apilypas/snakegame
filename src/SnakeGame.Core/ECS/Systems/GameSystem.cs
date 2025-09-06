using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.ECS.Entities;
using SnakeGame.Core.Enums;

namespace SnakeGame.Core.ECS.Systems;

public class GameSystem : EntityProcessingSystem
{
    private float _invincibilityTimer;
    
    private readonly GameState _gameState;
    private readonly EntityFactory _entityFactory;
    
    private ComponentMapper<PlayerComponent> _playerMapper;

    public GameSystem(GameState gameState,
        EntityFactory entityFactory)
        : base(Aspect.All(typeof(PlayFieldComponent)))
    {
        _gameState = gameState;
        _entityFactory = entityFactory;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _playerMapper = mapperService.GetMapper<PlayerComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        if (_gameState.IsPaused) return;
        
        HandleScoreMultiplicator(gameTime);
        HandleInvincibility(gameTime);
        HandleCollectables(entityId);
        HandleCollisions(entityId);
    }

    private static bool CollidesWith(SnakeComponent snake, Rectangle rectangle)
    {
        if (snake.Head.GetRectangle().Intersects(rectangle))
            return true;

        if (snake.Tail.GetRectangle().Intersects(rectangle))
            return true;

        foreach (var segment in snake.Segments)
        {
            if (segment.GetRectangle().Intersects(rectangle))
                return true;
        }

        return false;
    }

    private void SpawnFadingText(Vector2 at, string text)
    {
        var fadingTextEntity = _entityFactory.World.CreateFadingText(text);
        fadingTextEntity.Get<TransformComponent>().Position = at;
    }

    private Entity GetCollectableAt(Rectangle targetRectangle)
    {
        foreach (var collectableEntity in _gameState.Collectables)
        {
            if (new Rectangle(
                    (int)collectableEntity.Get<TransformComponent>().Position.X,
                    (int)collectableEntity.Get<TransformComponent>().Position.Y,
                    Constants.SegmentSize,
                    Constants.SegmentSize)
                .Intersects(targetRectangle))
            {
                return collectableEntity;
            }
        }

        return null;
    }

    private void HandleCollisions(int entityId)
    {
        foreach (var snakeEntity in _gameState.Snakes)
        {
            var snake = snakeEntity.Get<SnakeComponent>();

            if (!snake.IsInitialized)
                continue;
            
            if (!snake.IsAlive)
                continue;

            var isDead = false;
            var headRectangle = snake.Head.GetRectangle();

            if (CollidesWithSelf(snake) || !Globals.PlayFieldRectangle.Contains(headRectangle))
            {   
                isDead = true;
            }
            else
            {
                foreach (var otherSnakeEntity in _gameState.Snakes)
                {
                    var otherSnake = otherSnakeEntity.Get<SnakeComponent>();
                    
                    if (!otherSnake.IsInitialized)
                        continue;
                    
                    if (snake != otherSnake && CollidesWith(otherSnake, headRectangle))
                    {
                        if (snake.IsInvincible)
                            otherSnake.IsAlive = false;
                        else
                            isDead = true;
                        break;
                    }
                }
            }

            if (isDead)
            {
                snake.IsAlive = false;

                if (_playerMapper.Has(snakeEntity.Id))
                {
                    _gameState.Deaths++;
                    _gameState.ScoreMultiplicator = 1;
                    
                    GetEntity(entityId).Attach(new SoundEffectComponent
                    {
                        Type = SoundEffectTypes.PlayerDied
                    });
                }
            }
        }
    }

    private void HandleCollectables(int entityId)
    {
        foreach (var snakeEntity in _gameState.Snakes)
        {
            var snake = snakeEntity.Get<SnakeComponent>();
            
            if (!snake.IsAlive)
                continue;
            
            var headRectangle = snake.Head.GetRectangle();
            var collectableEntity = GetCollectableAt(headRectangle);

            if (collectableEntity != null)
            {
                HandleCollectableBonus(collectableEntity, snakeEntity);
                
                _gameState.Collectables.Remove(collectableEntity);

                if (_playerMapper.Has(snakeEntity.Id))
                {
                    GetEntity(entityId).Attach(new SoundEffectComponent
                    {
                        Type = SoundEffectTypes.ScoreChanged
                    });
                }
                
                collectableEntity.Destroy();
            }
        }

        if (_gameState.PlayerSnake != null && _gameState.PlayerSnake.Get<SnakeComponent>().Segments.Count > _gameState.LongestSnake)
        {
            _gameState.LongestSnake = _gameState.PlayerSnake.Get<SnakeComponent>().Segments.Count;
        }
    }

    private void HandleCollectableBonus(Entity collectableEntity, Entity snakeEntity)
    {
        var snake = snakeEntity.Get<SnakeComponent>();
        
        if (collectableEntity.Get<CollectableComponent>().CollectableType == CollectableType.Diamond)
        {
            snake.SegmentsToGrow++;
            if (_playerMapper.Has(snakeEntity.Id))
            {
                var score = _gameState.ScoreMultiplicator * Constants.DiamondCollectScore;
                _gameState.Score += score;
                SpawnFadingText(snake.Head.Position, $"+{score}");
            }
        }
        else if (collectableEntity.Get<CollectableComponent>().CollectableType == CollectableType.SnakePart)
        {
            snake.SegmentsToGrow++;
            if (_playerMapper.Has(snakeEntity.Id))
            {
                var score = _gameState.ScoreMultiplicator * Constants.SnakePartCollectScore;
                _gameState.Score += score;
                SpawnFadingText(snake.Head.Position, $"+{score}");
            }
        }
        else if (collectableEntity.Get<CollectableComponent>().CollectableType == CollectableType.SpeedBoost)
        {
            snake.SegmentsToGrow++;
            snake.SpeedTimer = Constants.SpeedUpTimer;
            if (_playerMapper.Has(snakeEntity.Id))
            {
                var score = _gameState.ScoreMultiplicator * Constants.SpeedBoostCollectScore;
                _gameState.Score += score;
                SpawnFadingText(snake.Head.Position, $"+{score} (+Speed)");
            }
        }
        else if (collectableEntity.Get<CollectableComponent>().CollectableType == CollectableType.Crown)
        {
            if (_playerMapper.Has(snakeEntity.Id))
            {
                snake.IsInvincible = true;
                _invincibilityTimer = Constants.InvincibleTimer;
                var score = _gameState.ScoreMultiplicator * Constants.CrownCollectScore;
                _gameState.Score += score;
                SpawnFadingText(snake.Head.Position, $"+{score} (+Invincible)");
            }
        }
        else if (collectableEntity.Get<CollectableComponent>().CollectableType == CollectableType.Clock)
        {
            snake.SegmentsToGrow++;
            if (_playerMapper.Has(snakeEntity.Id))
            {
                _gameState.Timer += Constants.ClockBonus;
                _gameState.Timer = Math.Min(_gameState.Timer, Constants.MaxTimer);
                
                var score = _gameState.ScoreMultiplicator * Constants.ClockCollectScore;
                _gameState.Score += score;
                SpawnFadingText(snake.Head.Position, $"+{score} (+Time)");
            }
        }
    }

    private void HandleInvincibility(GameTime gameTime)
    {
        if (_invincibilityTimer <= 0)
            return;
        
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        _invincibilityTimer -= deltaTime;

        if (_invincibilityTimer <= 0)
            _gameState.PlayerSnake.Get<SnakeComponent>().IsInvincible = false;
    }
    
    private static bool CollidesWithSelf(SnakeComponent snake)
    {
        var headRectangle = snake.Head.GetRectangle();

        for (var i = 1; i < snake.Segments.Count; i++)
        {
            if (snake.Segments[i].GetRectangle().Intersects(headRectangle))
                return true;
        }

        return false;
    }
    
    private void HandleScoreMultiplicator(GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        _gameState.ScoreMultiplicatorTimer += deltaTime;

        if (_gameState.ScoreMultiplicatorTimer >= Constants.ScoreMultiplicatorTimer)
        {
            _gameState.ScoreMultiplicatorTimer -= Constants.ScoreMultiplicatorTimer;
            _gameState.ScoreMultiplicator = MathHelper.Clamp(_gameState.ScoreMultiplicator + 1, 1, Constants.MaxScoreMultiplier);
        }
    }
}