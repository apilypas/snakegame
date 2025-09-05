using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.ECS.Entities;

namespace SnakeGame.Core.ECS.Systems;

public class ButtonEventSystem : EntityProcessingSystem
{
    private readonly GameState _gameState;
    private readonly EntityFactory _entityFactory;
    private ComponentMapper<ButtonEventComponent> _buttonEventMapper;

    public ButtonEventSystem(GameState gameState, EntityFactory entityFactory)
        : base(Aspect.All(typeof(ButtonEventComponent)))
    {
        _gameState = gameState;
        _entityFactory = entityFactory;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _buttonEventMapper = mapperService.GetMapper<ButtonEventComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var buttonEvent = _buttonEventMapper.Get(entityId);

        if (buttonEvent.Event == ButtonEvents.Resume)
        {
            var pauseDialogEntity = GetEntity(_gameState.PausedDialogId);
            pauseDialogEntity.Get<DialogComponent>().IsDestroyed = true;
            _gameState.IsPaused = false;
        }

        if (buttonEvent.Event == ButtonEvents.ShowScoreBoard)
        {
            _entityFactory.Dialogs.CreateScoreBoardDialog();
        }
        
        _buttonEventMapper.Delete(entityId);
    }
}