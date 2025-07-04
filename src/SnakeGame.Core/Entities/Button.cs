using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.Entities;

public class Button : Control
{
    private readonly Label _label;
    
    public Texture2D Texture { get; set; }
    public Rectangle TextureNormalRectangle { get; set; }
    public Rectangle TextureHoveredRectangle { get; set; }
    public Rectangle TexturePressedRectangle { get; set; }
    public string Text { get; set; }
    public bool IsPressed { get; set; }

    public delegate void OnButtonPressedEventHandler();
    public event OnButtonPressedEventHandler OnClick;

    public Button()
    {
        _label = new Label
        {
            Position = new Vector2(0f, 0f),
            Size = Size,
            HorizontalAlignment = Label.HorizontalLabelAlignment.Center,
            VerticalAlignment = Label.VerticalLabelAlignment.Center
        };
        
        AddChild(_label);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        _label.Text = Text;
        _label.Size = Size;

        if (Inputs != null)
        {
            IsPressed = IsHovered && Inputs.Mouse.IsLeftButtonDown;

            if (IsHovered && Inputs.Mouse.IsLeftButtonReleased)
                OnClick?.Invoke();
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (IsPressed)
        {
            spriteBatch.DrawFromNinePatch(
                GlobalPosition,
                Size,
                Texture,
                TexturePressedRectangle,
                Color.White);
        }
        else if (IsHovered)
        {
            spriteBatch.DrawFromNinePatch(
                GlobalPosition,
                Size,
                Texture,
                TextureHoveredRectangle,
                Color.White);
        }
        else
        {
            spriteBatch.DrawFromNinePatch(
                GlobalPosition,
                Size,
                Texture,
                TextureNormalRectangle,
                Color.White);
        }
    }
}