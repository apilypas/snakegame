using Microsoft.Xna.Framework;
using SnakeGame.Core.Commands;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Screens;

namespace SnakeGame.Core.Forms;

public class StartScreenForms(StartScreen startScreen)
{
    public class MainMenuForm : Entity
    {
        public MainMenuForm(GlobalCommands globalCommands, Entity world, InputManager inputs)
        {
            Position = new Vector2(60, 60);
            
            var panel = new Panel
            {
                Size = new Vector2(340, 130),
            };
            AddChild(panel);
            
            var label = new Label
            {
                Text = "Snake Game",
                Position = new Vector2(10f, 10f)
            };
            panel.AddChild(label);
            
            var startButton = new Button
            {
                Input = inputs,
                Text = "Start",
                Position = new Vector2(10f, 80f),
                Size = new Vector2(100, 40),
                Command = globalCommands.OpenPlayScreen
            };
            panel.AddChild(startButton);
            
            var creditsButton = new Button
            {
                Input = inputs,
                Text = "Credits",
                Position = new Vector2(120f, 80f),
                Size = new Vector2(100, 40),
                Command = globalCommands.OpenCreditsScreen
            };
            panel.AddChild(creditsButton);
            
            var quitButton = new Button
            {
                Input = inputs,
                Text = "Quit",
                Position = new Vector2(230f, 80f),
                Size = new Vector2(100, 40),
                Command = globalCommands.Quit
            };
            panel.AddChild(quitButton);
            
            world.AddChild(this);
        }
    }

    public MainMenuForm MainMenu { get; } = new(startScreen.GlobalCommands, startScreen.World, startScreen.Input);
}