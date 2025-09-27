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
        else if (buttonEvent.Event == ButtonEvents.ShowStartScreen)
        {
            _gameScreen.ScreenManager.LoadScreen(new StartScreen(_game));
        }
        else if (buttonEvent.Event == ButtonEvents.ShowScoreBoard)
        {
            _entityFactory.Dialog.CreateScoreBoardDialog();
        }
        else if (buttonEvent.Event == ButtonEvents.Close)
        {
            var dialog = _dialogMapper.Get(buttonEvent.DialogEntityId);
            dialog.IsDestroyed = true;
        }
        else if (buttonEvent.Event == ButtonEvents.AddTime)
        {
            _gameState.IsPaused = false;
            _dialogMapper.Get(buttonEvent.DialogEntityId).IsDestroyed = true;
            
            CreateEntity().Attach(new LevelBonusComponent
            {
                Type = LevelBonusComponent.LevelBonusType.AddTime
            });
        }
        else if (buttonEvent.Event == ButtonEvents.AddInvincibility)
        {
            _gameState.IsPaused = false;
            _dialogMapper.Get(buttonEvent.DialogEntityId).IsDestroyed = true;
            
            CreateEntity().Attach(new LevelBonusComponent
            {
                Type = LevelBonusComponent.LevelBonusType.AddInvincibility
            });
        }
        else if (buttonEvent.Event == ButtonEvents.DestroyEnemies)
        {
            _gameState.IsPaused = false;
            _dialogMapper.Get(buttonEvent.DialogEntityId).IsDestroyed = true;
            
            CreateEntity().Attach(new LevelBonusComponent
            {
                Type = LevelBonusComponent.LevelBonusType.DestroyEnemies
            });
        }
        else if (buttonEvent.Event == ButtonEvents.AddDiamondSpawnRate)
        {
            _gameState.IsPaused = false;
            _dialogMapper.Get(buttonEvent.DialogEntityId).IsDestroyed = true;
            
            CreateEntity().Attach(new LevelBonusComponent
            {
                Type = LevelBonusComponent.LevelBonusType.AddDiamondSpawnRate
            });
        }
        else if (buttonEvent.Event == ButtonEvents.AddScoreMultiplicator)
        {
            _gameState.IsPaused = false;
            _dialogMapper.Get(buttonEvent.DialogEntityId).IsDestroyed = true;
            
            CreateEntity().Attach(new LevelBonusComponent
            {
                Type = LevelBonusComponent.LevelBonusType.AddScoreMultiplicator
            });
        }
        else if (buttonEvent.Event == ButtonEvents.AddDiamonds)
        {
            _gameState.IsPaused = false;
            _dialogMapper.Get(buttonEvent.DialogEntityId).IsDestroyed = true;
            
            CreateEntity().Attach(new LevelBonusComponent
            {
                Type = LevelBonusComponent.LevelBonusType.AddDiamonds
            });
        }
        
        _buttonEventMapper.Delete(entityId);
    }
}