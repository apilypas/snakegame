using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Core.Entities;

namespace SnakeGame.Core.Dialogs;

public class Dialog : Control
{
    protected Entity Content { get; set; }
    
    protected Dialog(Entity world, SizeF size)
    {
        Size = size;
        IsVisible = false;
        
        AddChild(new ColorRectangle
        {
            Size = new SizeF(Constants.ScreenWidth, Constants.ScreenHeight),
            FillColor = new Color(Color.Black, .6f)
        });

        Content = new Panel
        {
            Size = size,
            Position = new Vector2(
                (Constants.ScreenWidth - size.Width) / 2,
                (Constants.ScreenHeight - size.Height) / 2)
        };
        
        AddChild(Content);
            
        world.AddChild(this);
    }
}