using SnakeGame.Core.Screens;

namespace SnakeGame.Core.Forms;

public class StartScreenForms(StartScreen startScreen)
{
    public const int MainMenuFormId = 1;

    public class MainMenuForm : Form
    {
        public MainMenuForm(StartScreen startScreen) : base(MainMenuFormId)
        {
            Add(new FormText("Snake Game"));
            Add(new FormAction("Start", startScreen.GlobalCommands.OpenPlayScreen));
            Add(new FormAction("Credits", startScreen.GlobalCommands.OpenPlayScreen));
            Add(new FormAction("Quit", startScreen.GlobalCommands.Quit));
        }
    }
    
    public MainMenuForm MainMenu { get; } = new(startScreen);
}