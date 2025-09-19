using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.ECS.Entities;
using SnakeGame.Core.Services;

namespace SnakeGame.Core.ECS.Systems;

public class GameTimerSystem : EntityProcessingSystem
{
    private readonly GameState _gameState;
    private readonly EntityFactory _entityFactory;
    private ComponentMapper<SoundEffectComponent> _soundEffectMapper;

    public GameTimerSystem(GameState gameState, EntityFactory entityFactory) 
        : base(Aspect.All(typeof(PlayFieldComponent)))
    {
        _gameState = gameState;
        _entityFactory = entityFactory;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _soundEffectMapper = mapperService.GetMapper<SoundEffectComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        if (_gameState.IsPaused) return;
        
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        _gameState.Timer -= deltaTime;
        _gameState.TotalTime += deltaTime;

        if (_gameState.Timer >= 0f)
        {
            if ((int)_gameState.Timer != _gameState.TimerRounded)
            {
                _gameState.TimerRounded = (int)_gameState.Timer;

                if (_gameState.TimerRounded <= 10)
                {
                    _soundEffectMapper.Put(entityId, new SoundEffectComponent
                    {
                        Type = SoundEffectTypes.Timer
                    });
                }
            }
        }
        else
        {
            _gameState.State = GameWorldState.Ended;
            _gameState.IsPaused = true;
            
            _soundEffectMapper.Put(entityId, new SoundEffectComponent
            {
                Type = SoundEffectTypes.GameEnd
            });

            _entityFactory.Dialog.CreateGameOverDialog(_gameState);
                
            var dataManager = new DataManager();
            dataManager.SaveScore(_gameState.Score, (int)_gameState.TotalTime);
        }
    }
}