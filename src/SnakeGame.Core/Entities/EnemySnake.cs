using System;
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
        Color.RosyBrown,
        Color.Aquamarine,
        Color.Violet,
        Color.GreenYellow,
        Color.Beige,
        Color.Plum,
        Color.Gray,
        Color.DarkGray,
        Color.DarkBlue
    ];
    
    private readonly static Random Random = new();
    
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
        Color = EnemyColors[Random.NextInt64() % EnemyColors.Length];
        base.Initialize();
    }
}