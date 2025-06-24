using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Core.Entities;

namespace SnakeGame.Core.Dialogs;

public class CreditsDialog : Dialog
{
    public CreditsDialog(Entity world)
        : base(world, new SizeF(310f, 260f))
    {
        var label = new Label
        {
            Text = new StringBuilder()
                .AppendLine("Yet another Snake Game")
                .AppendLine()
                .AppendLine("Created by: Andrius Pilypas")
                .AppendLine("Font: Pixel Operator")
                .AppendLine("Game engine created using MonoGame")
                .ToString(),
            Size = new SizeF(220f, 0f),
            Position = new Vector2(10f, 10f)
        };
        Content.AddChild(label);
            
        var backButton = new Button
        {
            Text = "Back",
            Position = new Vector2(100, 210),
            Size = new SizeF(100, 40),
        };

        backButton.OnClick += Hide;
        
        Content.AddChild(backButton);
    }
}