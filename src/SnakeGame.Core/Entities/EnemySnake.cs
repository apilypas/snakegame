using System;
using Microsoft.Xna.Framework;
using SnakeGame.Core.Enums;
using SnakeGame.Core.StateMachines;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Entities;

public class EnemySnake : Snake
{
    private readonly static Color[] EnemyColors;

    static EnemySnake()
    {
        EnemyColors = [
            Color.FromNonPremultiplied(0x8E, 0xCA, 0xE6, 0xFF),
            Color.FromNonPremultiplied(0x36, 0x81, 0xA3, 0xFF),
            Color.FromNonPremultiplied(0x06, 0x4E, 0x70, 0xFF),
            Color.FromNonPremultiplied(0x21, 0x9E, 0xBC, 0xFF),
            Color.FromNonPremultiplied(0x25, 0x57, 0x64, 0xFF),
            Color.FromNonPremultiplied(0x08, 0x45, 0x54, 0xFF),
            Color.FromNonPremultiplied(0x02, 0x30, 0x47, 0xFF),
            Color.FromNonPremultiplied(0x0D, 0x64, 0x8F, 0xFF),
            Color.FromNonPremultiplied(0x25, 0x44, 0x54, 0xFF),
            Color.FromNonPremultiplied(0x05, 0x22, 0x30, 0xFF),
            Color.FromNonPremultiplied(0xFB, 0x85, 0x00, 0xFF),
            Color.FromNonPremultiplied(0xEF, 0x9F, 0x45, 0xFF),
            Color.FromNonPremultiplied(0x93, 0x57, 0x14, 0xFF),
            Color.FromNonPremultiplied(0x50, 0x2B, 0x02, 0xFF),
            Color.FromNonPremultiplied(0xAB, 0x5B, 0x00, 0xFF),
            Color.FromNonPremultiplied(0x50, 0x2A, 0x00, 0xFF),
            Color.FromNonPremultiplied(0x93, 0x6D, 0x41, 0xFF),
            Color.FromNonPremultiplied(0x6C, 0x63, 0x59, 0xFF)
        ];
    }
    
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