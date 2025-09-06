using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;

namespace SnakeGame.Core.ECS.Systems;

public class ScoreMultiplicatorSystem : EntityProcessingSystem
{
    private readonly GameState _gameState;
    private ComponentMapper<SnakeComponent> _snakeMapper;

    public ScoreMultiplicatorSystem(GameState gameState) 
        : base(Aspect.All(typeof(SnakeComponent), typeof(PlayerComponent)))
    {
        _gameState = gameState;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _snakeMapper = mapperService.GetMapper<SnakeComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        if (_gameState.IsPaused) return;
        
        var snake = _snakeMapper.Get(entityId);

        if (snake.IsAlive)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
            _gameState.ScoreMultiplicatorTimer += deltaTime;

            if (_gameState.ScoreMultiplicatorTimer >= Constants.ScoreMultiplicatorTimer)
            {
                _gameState.ScoreMultiplicatorTimer -= Constants.ScoreMultiplicatorTimer;
                _gameState.ScoreMultiplicator = MathHelper.Clamp(_gameState.ScoreMultiplicator + 1, 1, Constants.MaxScoreMultiplier);
            }
        }
        else
        {
            _gameState.ScoreMultiplicator = 1;
            _gameState.ScoreMultiplicatorTimer = 0f;
        }
    }
}