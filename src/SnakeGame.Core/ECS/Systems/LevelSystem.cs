using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.ECS.Entities;

namespace SnakeGame.Core.ECS.Systems;

public class LevelSystem : EntityProcessingSystem
{
    private readonly GameState _gameState;
    private readonly EntityFactory _entities;
    private ComponentMapper<HudLevelDisplayComponent> _hudLevelDisplayMapper;

    public LevelSystem(GameState gameState, EntityFactory entities)
        : base(Aspect.All(typeof(HudLevelDisplayComponent)))
    {
        _gameState = gameState;
        _entities = entities;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _hudLevelDisplayMapper = mapperService.GetMapper<HudLevelDisplayComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var hudLevelDisplay = _hudLevelDisplayMapper.Get(entityId);

        if (_gameState.Experience >= _gameState.MaxExperience)
        {
            _gameState.Level++;
            _gameState.Experience -= _gameState.MaxExperience;
            _gameState.MaxExperience += (int)MathF.Round((float)(_gameState.MaxExperience * .1));

            _gameState.IsPaused = true;
                    
            _entities.Dialog.CreateLevelBonusDialog();
        }

        hudLevelDisplay.Level = $"Level {_gameState.Level}";
        hudLevelDisplay.Progress = _gameState.Experience / (float)_gameState.MaxExperience;
    }
}