using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.ECS.Entities;
using SnakeGame.Core.Services;

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
            var dataManager = new DataManager();
            var scoreBoard = dataManager.LoadScoreBoard();

            var resultBuilder = new StringBuilder()
                .AppendFormat("{0,14}", "Date")
                .Append("|")
                .AppendFormat("{0,14}", "Score")
                .Append("|")
                .AppendFormat("{0,8}", "Time (s)")
                .AppendLine();

            foreach (var entry in scoreBoard.Entries)
            {
                resultBuilder
                    .AppendFormat("{0,14}", entry.CreatedAt.ToShortDateString())
                    .Append("|")
                    .AppendFormat("{0,14}", entry.Score.ToString(Constants.ScoreFormat))
                    .Append("|")
                    .AppendFormat("{0,8}", entry.TimePlayed.ToString())
                    .AppendLine();
            }

            var dialogId = _entityFactory.CreateDialog(
                "Score Board",
                resultBuilder.ToString(),
                new SizeF(300f, 400f),
                ("Back", () =>
                {
                    //
                }));
            
            _gameState.ScoreBoardDialogId = dialogId;
        }
        
        _buttonEventMapper.Delete(entityId);
    }
}