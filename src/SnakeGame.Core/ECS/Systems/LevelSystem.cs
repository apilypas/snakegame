using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;

namespace SnakeGame.Core.ECS.Systems;

public class LevelSystem : EntityProcessingSystem
{
    private readonly GameState _gameState;
    private ComponentMapper<HudLevelDisplayComponent> _hudLevelDisplayMapper;

    public LevelSystem(GameState gameState)
        : base(Aspect.All(typeof(HudLevelDisplayComponent)))
    {
        _gameState = gameState;
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
            _gameState.Experience = 0;
            _gameState.MaxExperience += (int)MathF.Round((float)(_gameState.MaxExperience * .5));
        }
        
        hudLevelDisplay.Level = $"Level {_gameState.Level}";
        hudLevelDisplay.Progress = _gameState.Experience / (float)_gameState.MaxExperience;
    }
}