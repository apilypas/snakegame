using System.Text;
using Microsoft.Xna.Framework;
using SnakeGame.Core.Commands;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Screens;

namespace SnakeGame.Core.Forms;

public class CreditsScreenForms(CreditsScreen creditsScreen)
{
    public class CreditsForm : Entity
    {
        public CreditsForm(GlobalCommands globalCommands, InputManager inputs, Entity world)
        {
            Position = new Vector2(60, 60);
            
            var panel = new Panel
            {
                Size = new Vector2(350, 130),
            };
            AddChild(panel);
            
            var label = new Label
            {
                Text = new StringBuilder()
                    .AppendLine("Yet another implementation of Snake Game")
                    .AppendLine("Created by: Andrius Pilypas")
                    .ToString(),
                Position = new Vector2(10f, 10f)
            };
            panel.AddChild(label);
            
            var backButton = new Button
            {
                Input = inputs,
                Text = "Back",
                Position = new Vector2(240f, 80f),
                Size = new Vector2(100, 40),
                Command = globalCommands.OpenStartScreen
            };
            panel.AddChild(backButton);
            
            world.AddChild(this);
        }
    }
    
    public CreditsForm Credits { get; } = new(creditsScreen.GlobalCommands, creditsScreen.Inputs, creditsScreen.World);
}