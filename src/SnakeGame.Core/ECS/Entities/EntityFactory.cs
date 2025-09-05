using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Graphics;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Enums;
using SnakeGame.Core.Services;
using SnakeGame.Core.StateMachines;

namespace SnakeGame.Core.ECS.Entities;

public class EntityFactory
{
    private World _world;
    
    public DialogEntityFactory Dialog { get; private set; }
    public HudEntityFactory Hud { get; private set; }

    public void Initialize(World world, ContentManager contents)
    {
        _world = world;
        Dialog = new DialogEntityFactory(world, contents);
        Hud = new HudEntityFactory(world, contents);
    }

    public Entity CreatePlayerSnake(Vector2 location, int length, SnakeDirection direction)
    {
        var entity = _world.CreateEntity();
        
        entity.Attach(new PlayerComponent());
        
        entity.Attach(new SnakeComponent
        {
            Color = Color.Orange,
            DefaultLocation = location,
            DefaultLength = length,
            DefaultDirection = direction
        });
        
        return entity;
    }

    public Entity CreateEnemySnake(GameState gameState, Vector2 location, int length, SnakeDirection direction)
    {
        Color[] enemyColors = [
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
        
        var entity = _world.CreateEntity();
        
        entity.Attach(new EnemyComponent());
        
        entity.Attach(new SnakeComponent
        {
            DefaultLocation = location,
            DefaultLength = length,
            DefaultDirection = direction,
            State = new EnemySnakeState(gameState, entity),
            Color = enemyColors[Random.Shared.NextInt64() % enemyColors.Length]
        });
        
        return entity;
    }

    public Entity CreateCollectable(Texture2D texture, CollectableType type)
    {
        var entity = _world.CreateEntity();
        
        entity.Attach(new CollectableComponent
        {
            CollectableType = type
        });
        
        entity.Attach(new TransformComponent());
        
        entity.Attach(new SpriteComponent
        {
            Sprite = type switch
            {
                CollectableType.Diamond => new Sprite(
                    new Texture2DRegion(texture, new Rectangle(0, 32, 16, 16))),
                CollectableType.SnakePart => new Sprite(
                    new Texture2DRegion(texture, new Rectangle(0, 16, 16, 16))),
                CollectableType.SpeedBoost => new Sprite(
                    new Texture2DRegion(texture, new Rectangle(0, 0, 16, 16))),
                CollectableType.Clock => new Sprite(
                    new Texture2DRegion(texture, new Rectangle(16, 0, 16, 16))),
                CollectableType.Crown => new Sprite(
                    new Texture2DRegion(texture, new Rectangle(16, 16, 16, 16))),
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            }
        });
        
        return entity;
    }

    public Entity CreateFadingText(string text, float timeToLive = 1f)
    {
        var entity = _world.CreateEntity();
        
        entity.Attach(new FadingTextComponent
        {
            Text = text,
            TimeToLive = timeToLive
        });
        
        entity.Attach(new TransformComponent());
        
        return entity;
    }

    public Entity CreatePlayField(Texture2D texture)
    {
        var entity = _world.CreateEntity();
        
        entity.Attach(new PlayFieldComponent
        {
            TilesTexture = texture
        });
        
        return entity;
    }
}