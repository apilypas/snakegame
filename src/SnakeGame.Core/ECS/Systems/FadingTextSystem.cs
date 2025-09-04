using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.ECS.Components;

namespace SnakeGame.Core.ECS.Systems;

public class FadingTextSystem : EntityUpdateSystem
{
    private ComponentMapper<FadingTextComponent> _fadingTextMapper;
    private ComponentMapper<TransformComponent> _transformMapper;

    public FadingTextSystem()
        : base(Aspect.All(typeof(FadingTextComponent)))
    {
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _fadingTextMapper = mapperService.GetMapper<FadingTextComponent>();
        _transformMapper = mapperService.GetMapper<TransformComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entityId in ActiveEntities)
        {
            var fadingText = _fadingTextMapper.Get(entityId);
            var transform = _transformMapper.Get(entityId);
            
            var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
            fadingText.TimeToLive -= elapsed;
            transform.Position += new Vector2(0f, -elapsed * 30f);

            if (fadingText.TimeToLive <= 0f)
            {
                DestroyEntity(entityId);
            }
        }
    }
}