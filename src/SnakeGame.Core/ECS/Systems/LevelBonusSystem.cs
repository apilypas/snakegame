using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;

namespace SnakeGame.Core.ECS.Systems;

public class LevelBonusSystem : EntityUpdateSystem
{
    private readonly GameState _gameState;
    private ComponentMapper<LevelBonusComponent> _levelBonusMapper;
    private ComponentMapper<PlayerComponent> _playerMapper;
    private ComponentMapper<InvincibleComponent> _invincibleMapper;
    private ComponentMapper<SnakeComponent> _snakeMapper;
    private ComponentMapper<EnemyComponent> _enemyMapper;

    public LevelBonusSystem(GameState gameState) 
        : base(Aspect.One(typeof(LevelBonusComponent), typeof(PlayerComponent), typeof(SnakeComponent)))
    {
        _gameState = gameState;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _levelBonusMapper = mapperService.GetMapper<LevelBonusComponent>();
        _playerMapper = mapperService.GetMapper<PlayerComponent>();
        _invincibleMapper = mapperService.GetMapper<InvincibleComponent>();
        _snakeMapper = mapperService.GetMapper<SnakeComponent>();
        _enemyMapper = mapperService.GetMapper<EnemyComponent>();
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
                    _gameState.Timer += 30f;
                    if (_gameState.Timer > Constants.MaxTimer)
                        _gameState.Timer = Constants.MaxTimer;
                }
                else if (levelBonus.Type == LevelBonusComponent.LevelBonusType.AddInvincibility)
                {
                    var playerEntityId = ActiveEntities.Single(x => _playerMapper.Has(x));
                    _invincibleMapper.Put(playerEntityId, new InvincibleComponent
                    {
                        Timer = Constants.InvincibleTimer
                    });
                }
                else if (levelBonus.Type == LevelBonusComponent.LevelBonusType.DestroyEnemies)
                {
                    var enemyEntityIds = ActiveEntities.Where(x => _enemyMapper.Has(x)).ToList();

                    foreach (var enemyEntityId in enemyEntityIds)
                    {
                        _snakeMapper.Get(enemyEntityId).IsAlive = false;
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