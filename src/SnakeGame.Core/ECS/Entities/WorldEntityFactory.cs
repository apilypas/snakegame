using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Graphics;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Enums;
using SnakeGame.Core.Services;

namespace SnakeGame.Core.ECS.Entities;

public class WorldEntityFactory(World world, GameContentManager contents)
{
    public void CreatePlayField()
    {
        var entity = world.CreateEntity();
        
        entity.Attach(new PlayFieldComponent
        {
            TilesTexture = contents.TilesTexture
        });
    }
    
    public void CreatePlayerSnake(Vector2 location, int length, SnakeDirection direction)
    {
        var entity = world.CreateEntity();
        
        entity.Attach(new PlayerComponent());
        
        entity.Attach(new SnakeComponent
        {
            Color = Color.Orange,
            DefaultLocation = location,
            DefaultLength = length,
            DefaultDirection = direction
        });
        
        entity.Attach(new SpeedUpComponent());
    }

    public void CreateEnemySnake(Vector2 location, int length, SnakeDirection direction)
    {
        var entity = world.CreateEntity();
        
        entity.Attach(new EnemyComponent());

        var rgb = new[]
        {
            Random.Shared.NextSingle(),
            Random.Shared.NextSingle(),
            Random.Shared.NextSingle()
        };

        if (rgb.All(x => x < .1f))
        {
            rgb[Random.Shared.Next(rgb.Length - 1)] = (.1f + Random.Shared.NextSingle()) % 1f;
        }
        
        entity.Attach(new SnakeComponent
        {
            DefaultLocation = location,
            DefaultLength = length,
            DefaultDirection = direction,
            Color = new Color(rgb[0], rgb[1], rgb[2], 1f)
        });
        
        entity.Attach(new SpeedUpComponent());
    }

    public Entity CreateCollectable(CollectableType type)
    {
        var entity = world.CreateEntity();
        
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
                    new Texture2DRegion(contents.CollectableTexture, new Rectangle(0, 32, 16, 16))),
                CollectableType.SnakePart => new Sprite(
                    new Texture2DRegion(contents.CollectableTexture, new Rectangle(0, 16, 16, 16))),
                CollectableType.SpeedBoost => new Sprite(
                    new Texture2DRegion(contents.CollectableTexture, new Rectangle(0, 0, 16, 16))),
                CollectableType.Clock => new Sprite(
                    new Texture2DRegion(contents.CollectableTexture, new Rectangle(16, 0, 16, 16))),
                CollectableType.Crown => new Sprite(
                    new Texture2DRegion(contents.CollectableTexture, new Rectangle(16, 16, 16, 16))),
                CollectableType.SnackCake => new Sprite(
                    new Texture2DRegion(contents.CollectableTexture, new Rectangle(16, 32, 16, 16))),
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            }
        });
        
        return entity;
    }

    public Entity CreateFadingText(string text, float timeToLive = 1f)
    {
        var entity = world.CreateEntity();
        
        entity.Attach(new FadingTextComponent
        {
            Text = text,
            TimeToLive = timeToLive
        });
        
        entity.Attach(new TransformComponent());
        
        return entity;
    }
}