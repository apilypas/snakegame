using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;

namespace SnakeGame.Core.ECS.Systems;

public class ScoreDisplaySystem : EntityProcessingSystem
{
    private ComponentMapper<GameState> _gameStateMapper;

    public ScoreDisplaySystem() 
        : base(Aspect.All(typeof(GameState)))
    {
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _gameStateMapper = mapperService.GetMapper<GameState>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var gameState = _gameStateMapper.Get(entityId);

        GetEntity(gameState.ScoreLabelId).Get<LabelComponent>().Text = gameState.Score.ToString(Constants.ScoreFormat);
        GetEntity(gameState.MultiplicatorLabelId).Get<LabelComponent>().Text = $"x{gameState.ScoreMultiplicator}";
        GetEntity(gameState.TimeLabelId).Get<LabelComponent>().Text = $"{(int)gameState.Timer / 60:00}:{(int)gameState.Timer % 60:00}";
    }
}