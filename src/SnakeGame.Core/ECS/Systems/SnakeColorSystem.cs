using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.ECS.Components;

namespace SnakeGame.Core.ECS.Systems;

public class SnakeColorSystem : EntityProcessingSystem
{
    private ComponentMapper<SnakeComponent> _snakeMapper;

    public SnakeColorSystem() 
        : base(Aspect.All(typeof(SnakeComponent)))
    {
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _snakeMapper = mapperService.GetMapper<SnakeComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var snake = _snakeMapper.Get(entityId);

        if (snake.IsAlive)
        {
            UpdateSegmentColors(snake);
        }
    }

    private void UpdateSegmentColors(SnakeComponent snake)
    {
        snake.Head.Color = GetColor(snake.Color, 0, snake.IsInvincible);
        snake.Tail.Color = GetColor(snake.Color, snake.Segments.Count - 1, snake.IsInvincible);
        
        for (var i = 0; i < snake.Segments.Count; i++)
        {
            snake.Segments[i].Color = GetColor(snake.Color, i, snake.IsInvincible);
        }
    }
    
    private static Color GetColor(Color color, int index, bool isInvincible)
    {
        return isInvincible 
            ? GetInvincibleColor()
            : GetNormalStateColor(color, index);
    }

    private static Color GetNormalStateColor(Color color, int index)
    {
        var r = MathHelper.Clamp(color.R + 5*index, 0, 255);
        var g = MathHelper.Clamp(color.G + 5*index, 0, 255);
        var b = MathHelper.Clamp(color.B + 5*index, 0, 255);

        return new Color(r, g, b);
    }

    private static Color GetInvincibleColor()
    {
        var i = Random.Shared.Next(0, Constants.InvincibleColors.Length);
        return Constants.InvincibleColors[i];
    }
}