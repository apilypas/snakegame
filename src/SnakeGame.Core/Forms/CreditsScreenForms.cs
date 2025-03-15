using System.Text;
using SnakeGame.Core.Screens;

namespace SnakeGame.Core.Forms;

public class CreditsScreenForms(CreditsScreen creditsScreen)
{
    public const int CreditsFormId = 1;
    
    public class CreditsForm : Form
    {
        public CreditsForm(CreditsScreen creditsScreen) : base(CreditsFormId)
        {
            Add(new FormText("Credits"));
            Add(new FormText(new StringBuilder()
                .AppendLine("Yet another implementation of Snake Game")
                .AppendLine("Created by: Andrius Pilypas")
                .ToString()));
            Add(new FormAction("Back", creditsScreen.GlobalCommands.OpenStartScreen));
        }
    }
    
    public CreditsForm Credits { get; } = new(creditsScreen);
}