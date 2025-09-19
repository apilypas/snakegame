using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.ECS.Entities;

namespace SnakeGame.Core.ECS.Systems;

public class LevelBonusSystem : EntityUpdateSystem
{
    private readonly GameState _gameState;
    private readonly EntityFactory _entityFactory;
    private ComponentMapper<LevelBonusComponent> _levelBonusMapper;
    private ComponentMapper<PlayerComponent> _playerMapper;
    private ComponentMapper<InvincibleComponent> _invincibleMapper;
    private ComponentMapper<SnakeComponent> _snakeMapper;
    private ComponentMapper<EnemyComponent> _enemyMapper;
    private ComponentMapper<SoundEffectComponent> _soundEffectMapper;
    private ComponentMapper<ScreenShakeComponent> _screenShakeMapper;

    public LevelBonusSystem(GameState gameState, EntityFactory entityFactory) 
        : base(Aspect.One(typeof(LevelBonusComponent), typeof(PlayerComponent), typeof(SnakeComponent)))
    {
        _gameState = gameState;
        _entityFactory = entityFactory;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _levelBonusMapper = mapperService.GetMapper<LevelBonusComponent>();
        _playerMapper = mapperService.GetMapper<PlayerComponent>();
        _invincibleMapper = mapperService.GetMapper<InvincibleComponent>();
        _snakeMapper = mapperService.GetMapper<SnakeComponent>();
        _enemyMapper = mapperService.GetMapper<EnemyComponent>();
        _soundEffectMapper = mapperService.GetMapper<SoundEffectComponent>();
        _screenShakeMapper = mapperService.GetMapper<ScreenShakeComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entityId in ActiveEntities)
        {
            var levelBonus = _levelBonusMapper.Get(entityId);

            if (levelBonus != null)
            {
                if (levelBonus.Type == LevelBonusComponent.LevelBonusType.AddTime)
                {
                    var playerEntityId = ActiveEntities.Single(x => _playerMapper.Has(x));
                    
                    _gameState.Timer += 30f;
                    
                    if (_gameState.Timer > Constants.MaxTimer)
                        _gameState.Timer = Constants.MaxTimer;
                    
                    _soundEffectMapper.Put(playerEntityId, new SoundEffectComponent
                    {
                        Type = SoundEffectTypes.AddTime
                    });
                }
                else if (levelBonus.Type == LevelBonusComponent.LevelBonusType.AddInvincibility)
                {
                    var playerEntityId = ActiveEntities.Single(x => _playerMapper.Has(x));
                    _invincibleMapper.Put(playerEntityId, new InvincibleComponent
                    {
                        Timer = Constants.InvincibleTimer
                    });
                    
                    _soundEffectMapper.Put(playerEntityId, new SoundEffectComponent
                    {
                        Type = SoundEffectTypes.PowerUp
                    });
                }
                else if (levelBonus.Type == LevelBonusComponent.LevelBonusType.DestroyEnemies)
                {
                    var enemyEntityIds = ActiveEntities.Where(x => _enemyMapper.Has(x)).ToList();
                    var totalScore = 0;

                    foreach (var enemyEntityId in enemyEntityIds)
                    {
                        var enemySnake = _snakeMapper.Get(enemyEntityId);
                        enemySnake.IsAlive = false;
                        totalScore += enemySnake.Segments.Count * _gameState.ScoreMultiplicator;
                    }

                    if (totalScore > 0)
                    {
                        var playerEntityId = ActiveEntities.Single(x => _playerMapper.Has(x));
                        var playerSnake = _snakeMapper.Get(playerEntityId);
                        
                        _soundEffectMapper.Put(playerEntityId, new SoundEffectComponent
                        {
                            Type = SoundEffectTypes.EnemyHit
                        });
            
                        _screenShakeMapper.Put(playerEntityId, new ScreenShakeComponent());
                        
                        _gameState.Score += totalScore;

                        var fadingTextEntity = _entityFactory.World.CreateFadingText($"+{totalScore}");
                        fadingTextEntity.Get<TransformComponent>().Position = playerSnake.Head.Position;
                    }
                }
                else if (levelBonus.Type == LevelBonusComponent.LevelBonusType.AddDiamondSpawnRate)
                {
                    _gameState.DiamondSpawnRate = MathF.Max(
                        1f,
                        _gameState.DiamondSpawnRate - _gameState.DiamondSpawnRate * .2f);
                }
                else if (levelBonus.Type == LevelBonusComponent.LevelBonusType.AddScoreMultiplicator)
                {
                    _gameState.ScoreMultiplicator *= 2;
                    _gameState.ScoreMultiplicatorTimer = 0f;
                }
                
                DestroyEntity(entityId);
            }
        }
    }
}