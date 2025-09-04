using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Graphics;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Enums;
using SnakeGame.Core.StateMachines;

namespace SnakeGame.Core.ECS.Entities;

public class EntityFactory
{
    private World _world;

    public void Initialize(World world)
    {
        _world = world;
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

    public int CreateSprite(Texture2D texture, Rectangle source)
    {
        var entity = _world.CreateEntity();
        
        entity.Attach(new SpriteComponent
        {
            Sprite = new Sprite(new Texture2DRegion(texture, source))
        });
        
        entity.Attach(new TransformComponent());
        
        return entity.Id;
    }

    public int CreateLabel(SpriteFont spriteFont, string text, Color color)
    {
        var entity = _world.CreateEntity();

        entity.Attach(new LabelComponent
        {
            Font = spriteFont,
            Text = text,
            Color = color
        });
        
        entity.Attach(new TransformComponent());
        
        return entity.Id;
    }

    public int CreateButton(string text, Vector2 position, SizeF size, Action action)
    {
        var entity = _world.CreateEntity();
        
        entity.Attach(new ButtonComponent
        {
            Text = text,
            Size = size,
            Action = action
        });
        
        entity.Attach(new TransformComponent
        {
            Position = position
        });
        
        return entity.Id;
    }

    public int CreateDialog(string title, string content, SizeF size, params (string, Action)[] buttons)
    {
        var colorRectangleEntity = _world.CreateEntity();
        colorRectangleEntity.Attach(new ColorRectangleComponent
        {
            FillColor = new Color(Color.Black, .6f),
            Size = new SizeF(Constants.VirtualScreenWidth, Constants.VirtualScreenHeight)
        });
        colorRectangleEntity.Attach(new TransformComponent());
        
        var dialogEntity = _world.CreateEntity();

        var dialog = new DialogComponent
        {
            Title = title,
            Content = content,
            Size = size
        };
        dialogEntity.Attach(dialog);

        var transform = new TransformComponent
        {
            Position = new Vector2(
                (Constants.VirtualScreenWidth - size.Width) / 2,
                (Constants.VirtualScreenHeight - size.Height) / 2)
        };
        dialogEntity.Attach(transform);

        dialog.ChildrenEntities.Add(colorRectangleEntity.Id);   
        
        var totalButtonWidth = buttons.Length * 100f + (buttons.Length - 1) * 4f;
            
        var buttonPositionX = (dialog.Size.Width - totalButtonWidth) / 2f;
        var buttonPositionY = dialog.Size.Height - 46f;

        foreach (var button in buttons)
        {
            var buttonEntity = _world.CreateEntity();
            
            buttonEntity.Attach(new ButtonComponent
            {
                Text = button.Item1,
                Size = new SizeF(100f, 40f),
                Action = button.Item2
            });
            
            buttonEntity.Attach(new TransformComponent
            {
                Position = transform.Position + new Vector2(buttonPositionX, buttonPositionY)
            });

            buttonPositionX += 100f;
            buttonPositionX += 4f;
            
            dialog.ChildrenEntities.Add(buttonEntity.Id);
        }

        return dialogEntity.Id;
    }
}