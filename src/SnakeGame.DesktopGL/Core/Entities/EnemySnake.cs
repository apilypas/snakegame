using Microsoft.Xna.Framework;

namespace SnakeGame.DesktopGL.Core.Entities;

public class EnemySnake : Snake
{
    private readonly EnemySnakeBehavior _behavior;

    public EnemySnake(Vector2 location, int length, SnakeDirection direction, GameWorld gameWorld)
        : base(location, length, direction)
    {
        _behavior = new EnemySnakeBehavior(gameWorld, this);
    }

    public override void Update(float deltaTime)
    {
        var direction = _behavior.GetDirection();
        
        ChangeDirection(direction);

        base.Update(deltaTime);
    }
}