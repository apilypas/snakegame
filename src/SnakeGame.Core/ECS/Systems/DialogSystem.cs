using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.ECS.Components;

namespace SnakeGame.Core.ECS.Systems;

public class DialogSystem : EntityProcessingSystem
{
    private readonly List<int> _entityIds = [];
    
    private ComponentMapper<DialogComponent> _dialogMapper;
    private ComponentMapper<ButtonComponent> _buttonMapper;

    public DialogSystem() 
        : base(Aspect.All(typeof(DialogComponent)))
    {
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _dialogMapper = mapperService.GetMapper<DialogComponent>();
        _buttonMapper = mapperService.GetMapper<ButtonComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var dialog = _dialogMapper.Get(entityId);

        var canHandleInput = _entityIds.Count == 0 || entityId == _entityIds[^1];
        
        foreach (var childEntityId in dialog.ChildrenEntities)
        {
            var button = _buttonMapper.Get(childEntityId);
            if (button != null)
            {
                button.IsHandlingInput = canHandleInput;
            }
        }
        
        if (dialog.IsDestroyed)
        {
            foreach (var childEntityId in dialog.ChildrenEntities)
                DestroyEntity(childEntityId);
            
            DestroyEntity(entityId);
        }
    }

    protected override void OnEntityAdded(int entityId)
    {
        var dialog = _dialogMapper.Get(entityId);

        if (dialog != null)
        {
            _entityIds.Add(entityId);
            MediaPlayer.Volume = .1f;
        }
    }

    protected override void OnEntityRemoved(int entityId)
    {
        var dialog = _dialogMapper.Get(entityId);

        if (dialog != null)
        {
            _entityIds.Remove(entityId);
            
            if (_entityIds.Count == 0)
                MediaPlayer.Volume = .3f;
        }
    }
}