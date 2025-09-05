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

    public ScoreDisplaySystem(GameState gameState) 
        : base(Aspect.All(typeof(ScoreDisplayComponent)))
    {
        _gameState = gameState;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _scoreDisplayMapper = mapperService.GetMapper<ScoreDisplayComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var scoreDisplay = _scoreDisplayMapper.Get(entityId);
        
        GetEntity(scoreDisplay.ScoreLabelId).Get<HudLabelComponent>().Text = _gameState.Score.ToString(Constants.ScoreFormat);
        GetEntity(scoreDisplay.MultiplicatorLabelId).Get<HudLabelComponent>().Text = $"x{_gameState.ScoreMultiplicator}";
        GetEntity(scoreDisplay.TimeLabelId).Get<HudLabelComponent>().Text = $"{(int)_gameState.Timer / 60:00}:{(int)_gameState.Timer % 60:00}";
    }
}