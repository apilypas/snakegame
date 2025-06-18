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
            Position = new Vector2(0f, 20f)
        };
        Content.AddChild(_dataLines);
    }

    public override void OnShown(params object[] args)
    {
        var dataManager = new DataManager();
        var scoreBoard = dataManager.LoadScoreBoard();

        _dataLines.RemoveAllChildren();
        
        var columnPositions = new[] { 10f, 110f, 220f };
        var columnWidths = new[] { 100, 100f, 70f };
        
        _dataLines.AddChild(new Label
        {
            Text = "Date",
            Position = new Vector2(columnPositions[0], 0f),
            Color = Color.Silver,
            Size = new SizeF(columnWidths[0], 0f)
        });
        
        _dataLines.AddChild(new Label
        {
            Text = "Score",
            Position = new Vector2(columnPositions[1], 0f),
            Color = Color.Silver,
            Size = new SizeF(columnWidths[1], 0f),
            HorizontalAlignment = Label.HorizontalLabelAlignment.Center
        });
        
        _dataLines.AddChild(new Label
        {
            Text = "Time (s)",
            Position = new Vector2(columnPositions[2], 0f),
            Color = Color.Silver,
            Size = new SizeF(columnWidths[2], 0f),
            HorizontalAlignment = Label.HorizontalLabelAlignment.Right
        });
        
        var rowY = 20f;
        
        foreach (var entry in scoreBoard.Entries)
        {
            var dateLabel = new Label
            {
                Text = entry.CreatedAt.ToShortDateString(),
                Position = new Vector2(columnPositions[0], rowY),
                Size = new SizeF(columnWidths[0], 0f)
            };
            _dataLines.AddChild(dateLabel);

            var scoreLabel = new Label
            {
                Text = entry.Score.ToString("0000000000"),
                Position = new Vector2(columnPositions[1], rowY),
                HorizontalAlignment = Label.HorizontalLabelAlignment.Center,
                Size = new SizeF(columnWidths[1], 0f)
            };
            _dataLines.AddChild(scoreLabel);
            
            var timeLabel = new Label
            {
                Text = entry.TimePlayed.ToString(),
                Position = new Vector2(columnPositions[2], rowY),
                HorizontalAlignment = Label.HorizontalLabelAlignment.Right,
                Size = new SizeF(columnWidths[2], 0f)
            };
            _dataLines.AddChild(timeLabel);

            if (args.Length > 0 && entry.Id == (int)args[0])
            {
                dateLabel.Color = Color.Red;
                scoreLabel.Color = Color.Red;
                timeLabel.Color = Color.Red;
            }

            rowY += 20f;
        }
    }
}