using System.Text;
using SnakeGame.Core.Screens;

namespace SnakeGame.Core.Forms;

public class PlayScreenForms(PlayScreen playScreen)
{
    public const int PauseFormId = 1;
    public const int GameOverFormId = 2;

    public class PauseForm : Form
    {
        public PauseForm(PlayScreen playScreen) : base(PauseFormId)
        {
            Add(new FormText("Game is paused"));
            
            Add(new FormAction("Resume", playScreen.Commands.Pause));
            Add(new FormAction("Exit", playScreen.GlobalCommands.OpenStartScreen));
        }
    }

    public class GameOverForm : Form
    {
        public GameOverForm(PlayScreen playScreen) : base(GameOverFormId)
        {
            Add(new FormText("Game is over"));
            Add(new FormText("[Placeholder]"));
            Add(new FormAction("Exit", playScreen.GlobalCommands.OpenStartScreen));
        }
        
        public void UpdateResultsText(int score, int deaths)
        {
            var formText = (FormText) Elements[1];
            formText.Text = new StringBuilder()
                .AppendLine($"Your score: {score}")
                .AppendLine($"Your deaths: {deaths}")
                .ToString();
        }
    }
    
    public PauseForm Pause { get; } = new(playScreen);
    public GameOverForm GameOver { get; } = new(playScreen);
}