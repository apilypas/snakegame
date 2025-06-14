using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Screens;

namespace SnakeGame.Core.Systems;

public class DialogManager(PlayScreen playScreen, Entity world)
{
    public abstract class Dialog : Control
    {
        protected Dialog(GraphicsDevice graphics, Entity world, SizeF size)
        {
            Size = size;
            Position = new Vector2(
                (graphics.Viewport.Width - size.Width) / 2, 
                (graphics.Viewport.Height - size.Height) / 2);
            IsVisible = false;
            
            AddChild(new Panel
            {
                Size = size,
            });
            
            world.AddChild(this);
        }
    }
    
    public class PauseDialog : Dialog
    {
        public PauseDialog(PlayScreen playScreen, Entity world)
            : base(playScreen.GraphicsDevice, world, new SizeF(230, 130))
        {
            AddChild(new Label
            {
                Text = "Game is paused",
                Position = new Vector2(10f, 10f)
            });
        
            var resumeButton = new Button
            {
                Input = playScreen.Inputs,
                Text = "Resume",
                Position = new Vector2(10f, 80f),
                Size = new Vector2(100, 40)
            };

            resumeButton.OnClick += playScreen.TogglePause;

            AddChild(resumeButton);
            
            var exitButton = new Button
            {
                Input = playScreen.Inputs,
                Text = "Exit",
                Position = new Vector2(120f, 80f),
                Size = new Vector2(100, 40)
            };
            
            exitButton.OnClick += () =>
            {
                playScreen.ScreenManager.LoadScreen(new StartScreen(playScreen.Game));
            };
            
            AddChild(exitButton);
        }
    }

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
    
    public PauseDialog Pause { get; } = new(playScreen, world);
    public GameOverDialog GameOver { get; } = new(playScreen, world);
}