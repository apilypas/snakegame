using Microsoft.Xna.Framework;
using SnakeGame.Core.Enums;
using SnakeGame.Core.StateMachines;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Entities;

public class EnemySnake : Snake
{
    private readonly static Color[] EnemyColors =
    [
        Color.Red,
        Color.Green,
        Color.Blue,
        Color.Brown,
        Color.Chocolate,
        Color.Purple,
        Color.Tomato,
        Color.Purple
    ];
    
    public int EnemySnakeId { get; set; }
    
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

    public override void Initialize()
    {
        Color = EnemyColors[EnemySnakeId % EnemyColors.Length];
        base.Initialize();
    }
}