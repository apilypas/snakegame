using System.Text;
using Microsoft.Xna.Framework;
using SnakeGame.Core.Commands;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Screens;

namespace SnakeGame.Core.Forms;

public class PlayScreenForms(PlayScreen playScreen, World world, InputManager inputs)
{
    public class PauseForm : Entity
    {
        public PauseForm(PlayScreenCommands playScreenCommands, GlobalCommands globalCommands, World world, InputManager inputs)
        {
            Position = new Vector2(60, 60);
            IsVisible = false;
            
            var panel = new Panel
            {
                Size = new Vector2(230, 130),
            };
            AddChild(panel);
        
            var label = new Label
            {
                Text = "Game is paused",
                Position = new Vector2(10f, 10f)
            };
            panel.AddChild(label);
        
            var resumeButton = new Button
            {
                Input = inputs,
                Text = "Resume",
                Position = new Vector2(10f, 80f),
                Size = new Vector2(100, 40),
                Command = playScreenCommands.Pause
            };
            panel.AddChild(resumeButton);
            
            var exitButton = new Button
            {
                Input = inputs,
                Text = "Exit",
                Position = new Vector2(120f, 80f),
                Size = new Vector2(100, 40),
                Command = globalCommands.OpenStartScreen
            };
            panel.AddChild(exitButton);
            
            world.FormLayer.AddChild(this);
        }
    }

    public class GameOverForm : Entity
    {
        private readonly Label _resultsLabel;
        
        public GameOverForm(GlobalCommands globalCommands, World world, InputManager inputs)
        {
            Position = new Vector2(60, 60);
            IsVisible = false;
            
            var panel = new Panel
            {
                Size = new Vector2(230, 200),
            };
            AddChild(panel);
            
            var label = new Label
            {
                Text = "Game is over",
                Position = new Vector2(10f, 10f)
            };
            panel.AddChild(label);
            
            _resultsLabel = new Label
            {
                Text = "-",
                Position = new Vector2(10f, 30f)
            };
            panel.AddChild(_resultsLabel);
            
            var exitButton = new Button
            {
                Input = inputs,
                Text = "Exit",
                Position = new Vector2(60f, 150f),
                Size = new Vector2(100, 40),
                Command = globalCommands.OpenStartScreen
            };
            panel.AddChild(exitButton);
            
            world.FormLayer.AddChild(this);
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
    
    public PauseForm Pause { get; } = new(playScreen.Commands, playScreen.GlobalCommands, world, inputs);
    public GameOverForm GameOver { get; } = new(playScreen.GlobalCommands, world, inputs);
}