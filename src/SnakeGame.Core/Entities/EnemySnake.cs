using Microsoft.Xna.Framework;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Entities;

public class EnemySnake : Snake
{
    private readonly EnemySnakeBehavior _behavior;

    public EnemySnake(AssetManager assets, Vector2 location, int length, SnakeDirection direction, GameManager gameManager)
        : base(assets, location, length, direction)
    {
        _behavior = new EnemySnakeBehavior(gameManager, this);
    }

    public override void Update(float deltaTime)
    {
        var direction = _behavior.GetDirection();
        
        ChangeDirection(direction);

        base.Update(deltaTime);
    }
}