using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace SnakeGame.Core.Forms;

public abstract class FormElement
{
    public Vector2 Location { get; set; }
    public SizeF Size { get; set; }
}