using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Core.Entities;

namespace SnakeGame.Core.Dialogs;

public class Dialog : Control
{
    protected Dialog(Entity world, SizeF size)
    {
        Size = size;
        Position = new Vector2(
            (Constants.ScreenWidth - size.Width) / 2, 
            (Constants.ScreenHeight - size.Height) / 2);
        IsVisible = false;
            
        AddChild(new Panel
        {
            Size = size,
        });
            
        world.AddChild(this);
    }
}