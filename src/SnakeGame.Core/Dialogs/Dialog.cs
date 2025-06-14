using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SnakeGame.Core.Entities;

namespace SnakeGame.Core.Dialogs;

public class Dialog : Control
{
    protected Dialog(GraphicsDevice graphics, Entity world, SizeF size)
    {
        Size = size;
        Position = new Vector2(
            (graphics.Viewport.Width - size.Width) / 2, 
            (graphics.Viewport.Height - size.Height) / 2);
        IsVisible = false;
            
        AddChild(new Panel
        {
            Size = size,
        });
            
        world.AddChild(this);
    }
}