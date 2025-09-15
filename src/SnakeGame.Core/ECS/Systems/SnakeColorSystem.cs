using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.ECS.Components;

namespace SnakeGame.Core.ECS.Systems;

public class SnakeColorSystem : EntityProcessingSystem
{
    private ComponentMapper<SnakeComponent> _snakeMapper;
    private ComponentMapper<InvincibleComponent> _invincibleMapper;

    public SnakeColorSystem() 
        : base(Aspect.All(typeof(SnakeComponent)))
    {
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _snakeMapper = mapperService.GetMapper<SnakeComponent>();
        _invincibleMapper = mapperService.GetMapper<InvincibleComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var snake = _snakeMapper.Get(entityId);

        if (snake.IsAlive)
        {
            UpdateSegmentColors(entityId, snake);
        }
    }

    private void UpdateSegmentColors(int entityId, SnakeComponent snake)
    {
        var isInvincible = _invincibleMapper.Has(entityId);
        
        snake.Head.Color = GetColor(snake.Color, 0, isInvincible);
        snake.Tail.Color = GetColor(snake.Color, snake.Segments.Count - 1, isInvincible);
        
        for (var i = 0; i < snake.Segments.Count; i++)
        {
            snake.Segments[i].Color = GetColor(snake.Color, i, isInvincible);
        }
    }
    
    private static Color GetColor(Color color, int index, bool isInvincible)
    {
        if (isInvincible)
        {
            index = Random.Shared.Next(0, 25);
        }
        
        var r = MathHelper.Clamp(color.R + 5*index, 0, 255);
        var g = MathHelper.Clamp(color.G + 5*index, 0, 255);
        var b = MathHelper.Clamp(color.B + 5*index, 0, 255);

        return new Color(r, g, b);
    }
}