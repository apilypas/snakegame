using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
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
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var buttonEvent = _buttonEventMapper.Get(entityId);

        if (buttonEvent.Event == ButtonEvents.StartNew)
        {
            _gameScreen.ScreenManager.LoadScreen(new PlayScreen(_game));
        }

        if (buttonEvent.Event == ButtonEvents.ShowCredits)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var content = new StringBuilder()
                .AppendLine($"Yet another Snake Game ({version})")
                .AppendLine()
                .AppendLine("Developed by: g1ngercat.itch.io")
                .AppendLine("Music and art too...")
                .AppendLine()
                .AppendLine("Font: Pixel Operator")
                .AppendLine("Game engine based on MonoGame")
                .ToString();
            
            var dialogId = _entityFactory.CreateDialog(
                "Credits",
                content,
                new SizeF(310f, 260f),
                ("Back", () =>
                {
                    //
                }));
        }
        
        _buttonEventMapper.Delete(entityId);
    }
}