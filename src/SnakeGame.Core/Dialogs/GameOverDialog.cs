using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Screens;

namespace SnakeGame.Core.Dialogs;

public class GameOverDialog : Dialog
{
    private readonly Label _resultsLabel;
        
    public GameOverDialog(PlayScreen playScreen, Entity world)
        : base(playScreen.GraphicsDevice, world, new SizeF(230, 200))
    {
        AddChild(new Label
        {
            Text = "Game is over",
            Position = new Vector2(10f, 10f)
        });
            
        _resultsLabel = new Label
        {
            Text = "-",
            Position = new Vector2(10f, 30f)
        };
        AddChild(_resultsLabel);
            
        var exitButton = new Button
        {
            Input = playScreen.Inputs,
            Text = "Exit",
            Position = new Vector2(60f, 150f),
            Size = new Vector2(100, 40)
        };

        exitButton.OnClick += () =>
        {
            playScreen.ScreenManager.LoadScreen(new StartScreen(playScreen.Game));
        };

        AddChild(exitButton);
    }

    public void UpdateResults(int score, int deaths, int longest)
    {
        _resultsLabel.Text = new StringBuilder()
            .AppendLine("Your results:")
            .AppendLine($"Score: {score}")
            .AppendLine($"Deaths: {deaths}")
            .AppendLine($"Longest snake: {longest}")
            .ToString();
    }
}