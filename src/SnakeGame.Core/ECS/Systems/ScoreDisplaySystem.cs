using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;

namespace SnakeGame.Core.ECS.Systems;

public class ScoreDisplaySystem : EntityProcessingSystem
{
    private readonly GameState _gameState;
    private ComponentMapper<ScoreDisplayComponent> _scoreDisplayMapper;
    private ComponentMapper<HudLabelComponent> _hudLabelMapper;

    public ScoreDisplaySystem(GameState gameState) 
        : base(Aspect.All(typeof(ScoreDisplayComponent)))
    {
        _gameState = gameState;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _scoreDisplayMapper = mapperService.GetMapper<ScoreDisplayComponent>();
        _hudLabelMapper = mapperService.GetMapper<HudLabelComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var scoreDisplay = _scoreDisplayMapper.Get(entityId);
        
        _hudLabelMapper.Get(scoreDisplay.ScoreLabelId).Text = _gameState.Score.ToString(Constants.ScoreFormat);
        _hudLabelMapper.Get(scoreDisplay.MultiplicatorLabelId).Text = $"x{_gameState.ScoreMultiplicator}";
        _hudLabelMapper.Get(scoreDisplay.TimeLabelId).Text = $"{(int)_gameState.Timer / 60:00}:{(int)_gameState.Timer % 60:00}";
    }
}