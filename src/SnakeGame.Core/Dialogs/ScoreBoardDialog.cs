using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Dialogs;

public class ScoreBoardDialog : Dialog
{
    private readonly Entity _dataLines;
    
    public ScoreBoardDialog(Entity world)
        : base(world, new SizeF(300, 400))
    {
        var titleLabel = new Label
        {
            Text = "Score Board",
            Position = new Vector2(100f, 0f)
        };
        Content.AddChild(titleLabel);

        var backButton = new Button
        {
            Text = "Back",
            Position = new Vector2(100f, 350f),
            Size = new SizeF(100f, 40f)
        };
        backButton.OnClick += Hide;
        Content.AddChild(backButton);
        
        _dataLines = new Entity
        {
            Position = new Vector2(0f, 10f)
        };
        Content.AddChild(_dataLines);
    }

    public override void OnShown(params object[] args)
    {
        var dataManager = new DataManager();
        var scoreBoard = dataManager.LoadScoreBoard();

        _dataLines.RemoveAllChildren();
        var y = 10f;
        
        foreach (var entry in scoreBoard.Entries)
        {
            var dateLabel = new Label
            {
                Text = entry.CreatedAt.ToShortDateString(),
                Position = new Vector2(10f, y)
            };
            _dataLines.AddChild(dateLabel);

            var scoreLabel = new Label
            {
                Text = entry.Score.ToString(),
                Position = new Vector2(110f, y)
            };
            _dataLines.AddChild(scoreLabel);
            
            var timeLabel = new Label
            {
                Text = entry.TimePlayed.ToString(),
                Position = new Vector2(240f, y)
            };
            _dataLines.AddChild(timeLabel);

            if (args.Length > 0 && entry.Id == (int)args[0])
            {
                dateLabel.Color = Color.Red;
                scoreLabel.Color = Color.Red;
                timeLabel.Color = Color.Red;
            }

            y += 20f;
        }
    }
}