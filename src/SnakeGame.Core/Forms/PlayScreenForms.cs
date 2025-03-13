using SnakeGame.Core.Screens;

namespace SnakeGame.Core.Forms;

public class PlayScreenForms(PlayScreen playScreen)
{
    public const int PauseFormId = 1;
    public const int GameOverFormId = 2;

    private class PauseForm : Form
    {
        public PauseForm(PlayScreen playScreen) : base(PauseFormId)
        {
            Add(new FormText("Game is paused"));
            Add(new FormAction("Resume", playScreen.Commands.Pause));
            Add(new FormAction("Quit", playScreen.GlobalCommands.OpenStartScreen));
        }
    }

    private class GameOverForm : Form
    {
        public GameOverForm(PlayScreen playScreen) : base(GameOverFormId)
        {
            Add(new FormText("Game is over"));
            Add(new FormAction("Quit", playScreen.GlobalCommands.OpenStartScreen));
        }
    }
    
    public Form Pause { get; } = new PauseForm(playScreen);
    public Form GameOver { get; } = new GameOverForm(playScreen);
}