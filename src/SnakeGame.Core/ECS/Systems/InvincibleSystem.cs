using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.ECS.Components;

namespace SnakeGame.Core.ECS.Systems;

public class InvincibleSystem : EntityProcessingSystem
{
    private ComponentMapper<InvincibleComponent> _invincibleMapper;

    public InvincibleSystem() 
        : base(Aspect.All(typeof(InvincibleComponent)))
    {
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _invincibleMapper = mapperService.GetMapper<InvincibleComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var invincible = _invincibleMapper.Get(entityId);

        invincible.Timer -= deltaTime;

        if (invincible.Timer <= 0)
        {
            _invincibleMapper.Delete(entityId);
        }
    }
}