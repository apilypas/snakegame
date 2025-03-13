using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using SnakeGame.Core.Commands;

namespace SnakeGame.Core.Forms;

public class FormAction(string title, ICommand command)
{
    public string Title { get; } = title;
    public bool IsHovered { get; set; }
    public ICommand Command { get; } = command;
    public Vector2 Location { get; set; }
    public Vector2 TitleLocation { get; set; }
    public SizeF Size { get; set; }

    public RectangleF Bounds => new(Location, Size);
}