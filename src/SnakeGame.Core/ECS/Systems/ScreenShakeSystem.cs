using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.ECS.Components;

namespace SnakeGame.Core.ECS.Systems;

public class ScreenShakeSystem : EntityUpdateSystem
{
    private const float ShakeTime = .3f;
    
    private ComponentMapper<ScreenShakeComponent> _screenShakeMapper;
    
    public ScreenShakeSystem() 
        : base(Aspect.All(typeof(ScreenShakeComponent)))
    {
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _screenShakeMapper = mapperService.GetMapper<ScreenShakeComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        foreach (var entityId in ActiveEntities)
        {
            var screenShake = _screenShakeMapper.Get(entityId);
            
            screenShake.Timer += deltaTime;

            screenShake.CameraOffset = new Vector2(
                Random.Shared.NextSingle(-5f, 5f),
                Random.Shared.NextSingle(-5f, 5f)
                );

            if (screenShake.Timer >= ShakeTime)
            {
                _screenShakeMapper.Delete(entityId);
            }
        }
    }
}