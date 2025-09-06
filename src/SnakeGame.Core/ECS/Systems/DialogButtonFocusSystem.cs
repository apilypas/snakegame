using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.ECS.Components;

namespace SnakeGame.Core.ECS.Systems;

public class DialogButtonFocusSystem : EntityUpdateSystem
{
    private ComponentMapper<DialogComponent> _dialogMapper;
    private ComponentMapper<ButtonComponent> _buttonMapper;
    private ComponentMapper<NavigationIntentComponent> _navigationIntent;

    public DialogButtonFocusSystem() 
        : base(Aspect.One(typeof(NavigationIntentComponent), typeof(DialogComponent)))
    {
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _navigationIntent = mapperService.GetMapper<NavigationIntentComponent>();
        _dialogMapper = mapperService.GetMapper<DialogComponent>();
        _buttonMapper = mapperService.GetMapper<ButtonComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        NavigationIntentComponent navigationIntent = null;
        
        foreach (var entityId in ActiveEntities)
        {
            if (_navigationIntent.Has(entityId))
                navigationIntent = _navigationIntent.Get(entityId);
        }

        if (navigationIntent != null && navigationIntent.Event != NavigationEvent.None)
        {
            foreach (var entityId in ActiveEntities)
            {
                var dialog = _dialogMapper.Get(entityId);
                
                if (dialog == null)
                    continue;
                
                var buttons = dialog.ChildrenEntities
                    .Select(x => _buttonMapper.Get(x))
                    .Where(x => x is { IsHandlingInput: true })
                    .OrderBy(x => x.FocusOrderId)
                    .ToList();
                
                if (buttons.Any())
                {
                    if (navigationIntent.Event is NavigationEvent.FocusNext or NavigationEvent.FocusPrevious)
                    {
                        if (navigationIntent.Event == NavigationEvent.FocusPrevious)
                            buttons.Reverse();
                        
                        var selectNext = false;
                        foreach (var button in buttons)
                        {
                            if (button.IsFocused)
                            {
                                selectNext = true;
                                button.IsFocused = false;
                            }
                            else if (selectNext)
                            {
                                button.IsFocused = true;
                                break;
                            }
                        }
                        if (!selectNext)
                            buttons[0].IsFocused = true;
                    }

                    if (navigationIntent.Event == NavigationEvent.Select)
                    {
                        foreach (var button in buttons)
                        {
                            if (button.IsFocused)
                            {
                                button.Action();
                                break;
                            }
                        }
                    }
                    
                    break;
                }
            }
            
            navigationIntent.Event = NavigationEvent.None;
        }
    }
}