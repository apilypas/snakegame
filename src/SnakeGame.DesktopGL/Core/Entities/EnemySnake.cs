using Microsoft.Xna.Framework;

namespace SnakeGame.DesktopGL.Core.Entities;

public class EnemySnake : Snake
{
    private readonly GameWorld _gameWorld;
    private readonly EnemySnakeBehavior _behavior;
    
    public EnemySnake(GameWorld gameWorld)
        : base(GetInitialLocation())
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

    private static Vector2 GetInitialLocation()
    {
        return new Vector2(100f, 20f);
    }
}