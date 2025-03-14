namespace SnakeGame.Core.Forms;

public class FormText(string text) : FormElement
{
    public string Text { get; set; } = text;
}