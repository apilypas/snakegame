using Microsoft.Xna.Framework.Input;
using SnakeGame.DesktopGL.Core.Commands;

namespace SnakeGame.DesktopGL.Core.Forms;

public class PlayScreenForms(PlayScreenCommands playScreenCommands, GlobalCommands globalCommands)
{
    public const int PauseFormId = 1;
    public const int GameOverFormId = 2;

    private class PauseForm : Form
    {
        public PauseForm(PlayScreenCommands playScreenCommands, GlobalCommands globalCommands)
            : base(PauseFormId)
        {
            Add(new FormText("Game is paused"));
            Add(new FormAction("Resume", Keys.Escape, playScreenCommands.Resume));
            Add(new FormAction("Quit", Keys.Q, globalCommands.Quit));
        }
    }

    private class GameOverForm : Form
    {
        public GameOverForm(GlobalCommands globalCommands)
            : base(GameOverFormId)
        {
            Add(new FormText("Game is over"));
            Add(new FormAction("Quit", Keys.Q, globalCommands.Quit));
        }
    }
    
    public Form Pause { get; } = new PauseForm(playScreenCommands, globalCommands);
    public Form GameOver { get; } = new GameOverForm(globalCommands);
}