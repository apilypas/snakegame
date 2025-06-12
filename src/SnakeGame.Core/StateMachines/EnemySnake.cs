using Microsoft.Xna.Framework;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.StateMachines;

public class EnemySnake : Snake
{
    public EnemySnake(
        AssetManager assets,
        Vector2 location,
        int length,
        SnakeDirection direction,
        GameManager gameManager)
        : base(assets, location, length, direction)
    {
        ChangeState(new EnemySnakeState(gameManager, this));
    }
}