using Microsoft.Xna.Framework;

namespace SnakeGame.DesktopGL.Core.Entities;

public class EnemySnake : Snake
{
    private readonly GameWorld _gameWorld;
    private readonly EnemySnakeBehavior _behavior;
    
    public EnemySnake(GameWorld gameWorld, Vector2 location)
        : base(location)
    {
        _gameWorld = gameWorld;
        _behavior = new EnemySnakeBehavior(_gameWorld, this);
    }

    public override void Update(float deltaTime)
    {
        var direction = _behavior.GetDirection();
        
        ChangeDirection(direction);

        base.Update(deltaTime);
    }
}