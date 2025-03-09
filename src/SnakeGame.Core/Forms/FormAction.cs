using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using SnakeGame.Core.Commands;

namespace SnakeGame.Core.Forms;

public class FormAction(string title, Keys key, ICommand command)
{
    public string Title { get; } = title;
    public bool IsHovered { get; set; }
    public ICommand Command { get; } = command;
    public Vector2 Location { get; set; }
    public Vector2 TitleLocation { get; set; }
    public SizeF Size { get; set; }
    public Keys Key { get; } = key;

    public bool IsMouseOver(int x, int y)
    {
        var point = new Vector2(x, y);
        var rectangle = new RectangleF(Location, Size);
        return rectangle.Contains(point);
    }
}