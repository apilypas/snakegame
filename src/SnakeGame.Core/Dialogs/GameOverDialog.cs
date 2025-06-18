using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Screens;

namespace SnakeGame.Core.Dialogs;

public class GameOverDialog : Dialog
{
    private readonly Label _resultsLabel;
    private readonly PlayScreen _playScreen;

    public GameOverDialog(PlayScreen playScreen, Entity world)
        : base(world, new SizeF(270, 200))
    {
        _playScreen = playScreen;
        
        Content.AddChild(new Label
        {
            Text = "Game is over",
            Position = new Vector2(10f, 10f)
        });
            
        _resultsLabel = new Label
        {
            Text = "-",
            Position = new Vector2(10f, 30f)
        };
        Content.AddChild(_resultsLabel);
        
        var scoreBoardButton = new Button
        {
            Text = "Score Board",
            Position = new Vector2(10f, 150f),
            Size = new Vector2(120, 40)
        };

        scoreBoardButton.OnClick += playScreen.ShowScoreBoardDialog;
        
        Content.AddChild(scoreBoardButton);
            
        var exitButton = new Button
        {
            Text = "Exit",
            Position = new Vector2(140f, 150f),
            Size = new Vector2(120, 40)
        };

        exitButton.OnClick += () =>
        {
            playScreen.ScreenManager.LoadScreen(new StartScreen(playScreen.Game));
        };

        Content.AddChild(exitButton);
    }

    public override void OnShown(params object[] args)
    {
        _resultsLabel.Text = new StringBuilder()
            .AppendLine("Your results:")
            .AppendLine($"Score: {_playScreen.GameManager.Score}")
            .AppendLine($"Deaths: {_playScreen.GameManager.Deaths}")
            .AppendLine($"Longest snake: {_playScreen.GameManager.LongestSnake}")
            .AppendLine($"Time played: {(int)_playScreen.GameManager.TotalTime}s")
            .ToString();
    }
}