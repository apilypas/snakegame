using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Screens;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.ECS.Entities;
using SnakeGame.Core.Screens;

namespace SnakeGame.Core.ECS.Systems;

public class StartMenuButtonEventSystem : EntityProcessingSystem
{
    private readonly GameScreen _gameScreen;
    private readonly Game _game;
    private readonly EntityFactory _entityFactory;
    private ComponentMapper<ButtonEventComponent> _buttonEventMapper;
    private ComponentMapper<DialogComponent> _dialogMapper;

    public StartMenuButtonEventSystem(GameScreen gameScreen, Game game, EntityFactory entityFactory) 
        : base(Aspect.All(typeof(ButtonEventComponent)))
    {
        _gameScreen = gameScreen;
        _game = game;
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

        if (buttonEvent.Event == ButtonEvents.StartNew)
        {
            _gameScreen.ScreenManager.LoadScreen(new PlayScreen(_game));
        }

        if (buttonEvent.Event == ButtonEvents.Close)
        {
            var dialog = _dialogMapper.Get(buttonEvent.DialogEntityId);
            dialog.IsDestroyed = true;  
        }

        if (buttonEvent.Event == ButtonEvents.Exit)
        {
            _game.Exit();
        }

        if (buttonEvent.Event == ButtonEvents.ShowCredits)
        {
            _entityFactory.Dialog.CreateCreditsDialog();
        }
        
        if (buttonEvent.Event == ButtonEvents.ShowScoreBoard)
        {
            _entityFactory.Dialog.CreateScoreBoardDialog();
        }
        
        _buttonEventMapper.Delete(entityId);
    }
}