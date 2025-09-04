using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.ECS.Components;

namespace SnakeGame.Core.ECS.Systems;

public class DialogSystem : EntityProcessingSystem
{
    private ComponentMapper<DialogComponent> _dialogMapper;

    public DialogSystem() 
        : base(Aspect.All(typeof(DialogComponent)))
    {
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _dialogMapper = mapperService.GetMapper<DialogComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var dialog = _dialogMapper.Get(entityId);

        if (dialog.IsDestroyed)
        {
            foreach (var childEntityId in dialog.ChildrenEntities)
                DestroyEntity(childEntityId);
            
            DestroyEntity(entityId);
        }
    }
}