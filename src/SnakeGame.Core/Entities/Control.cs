using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Entities;

public class Control : Entity
{
    private SizeF _previousSize;
    
    public SizeF Size { get; set; }
    public InputManager Inputs { get; set; }
    public bool IsHovered { get; set; }

    public override void Update(GameTime gameTime)
    {
        HandleSizeChanged();

        IsHovered = Inputs != null && GetGlobalRectangle().Contains(Inputs.Mouse.Position);
    }

    protected virtual void OnSizeChanged() { }

    private Rectangle GetGlobalRectangle()
    {
        return new Rectangle((int)GlobalPosition.X, (int)GlobalPosition.Y, (int)Size.Width, (int)Size.Height);
    }

    private void HandleSizeChanged()
    {
        if (MathF.Abs(Size.Width - _previousSize.Width) >= 1f 
            || MathF.Abs(Size.Height - _previousSize.Height) >= 1f)
        {
            _previousSize = Size;
            OnSizeChanged();
        }
    }
}