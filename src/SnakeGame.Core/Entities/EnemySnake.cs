using Microsoft.Xna.Framework;
using SnakeGame.Core.StateMachines;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Entities;

public class EnemySnake : Snake
{
    public EnemySnake(
        AssetManager assets,
        Vector2 location,
        int length,
        SnakeDirection direction,
        EntityManager entities)
        : base(assets, location, length, direction)
    {
        ChangeState(new EnemySnakeState(entities, this));
    }
}