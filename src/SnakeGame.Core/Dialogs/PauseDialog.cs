using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Screens;

namespace SnakeGame.Core.Dialogs;

public class PauseDialog : Dialog
{
    public PauseDialog(PlayScreen playScreen, Entity world)
        : base(world, new SizeF(230, 130))
    {
        Content.AddChild(new Label
        {
            Text = "Game is paused",
            Position = new Vector2(10f, 10f),
            Size = new SizeF(210, 0),
            HorizontalAlignment = Label.HorizontalLabelAlignment.Center
        });
        
        var resumeButton = new Button
        {
            Text = "Resume",
            Position = new Vector2(10f, 80f),
            Size = new Vector2(100, 40)
        };

        resumeButton.OnClick += playScreen.TogglePause;

        Content.AddChild(resumeButton);
            
        var exitButton = new Button
        {
            Text = "Exit",
            Position = new Vector2(120f, 80f),
            Size = new Vector2(100, 40)
        };
            
        exitButton.OnClick += () =>
        {
            playScreen.ScreenManager.LoadScreen(new StartScreen(playScreen.Game));
        };
            
        Content.AddChild(exitButton);
    }
}