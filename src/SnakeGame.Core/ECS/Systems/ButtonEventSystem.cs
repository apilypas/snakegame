using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Screens;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.ECS.Entities;
using SnakeGame.Core.Screens;

namespace SnakeGame.Core.ECS.Systems;

public class ButtonEventSystem : EntityProcessingSystem
{
    private readonly GameScreen _gameScreen;
    private readonly Game _game;
    private readonly GameState _gameState;
    private readonly EntityFactory _entityFactory;
    private ComponentMapper<ButtonEventComponent> _buttonEventMapper;
    private ComponentMapper<DialogComponent> _dialogMapper;

    public ButtonEventSystem(GameScreen gameScreen, Game game, GameState gameState, EntityFactory entityFactory)
        : base(Aspect.All(typeof(ButtonEventComponent)))
    {
        _gameScreen = gameScreen;
        _game = game;
        _gameState = gameState;
        _entityFactory = entityFactory;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _buttonEventMapper = mapperService.GetMapper<ButtonEventComponent>();
        _dialogMapper = mapperService.GetMapper<DialogComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var buttonEvent = _buttonEventMapper.Get(entityId);

        if (buttonEvent.Event == ButtonEvents.Resume)
        {
            _dialogMapper.Get(buttonEvent.DialogEntityId).IsDestroyed = true;
            _gameState.IsPaused = false;
        }

        if (buttonEvent.Event == ButtonEvents.ShowStartScreen)
        {
            _gameScreen.ScreenManager.LoadScreen(new StartScreen(_game));
        }

        if (buttonEvent.Event == ButtonEvents.ShowScoreBoard)
        {
            _entityFactory.Dialog.CreateScoreBoardDialog();
        }
        
        if (buttonEvent.Event == ButtonEvents.Close)
        {
            var dialog = _dialogMapper.Get(buttonEvent.DialogEntityId);
            dialog.IsDestroyed = true;
        }
        
        _buttonEventMapper.Delete(entityId);
    }
}