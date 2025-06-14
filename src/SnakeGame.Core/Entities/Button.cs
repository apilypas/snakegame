using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Systems;
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
    public InputManager Input { get; set; }
    public bool IsHovered { get; set; }
    public bool IsPressed { get; set; }

    public delegate void OnButtonPressedEventHandler();
    public event OnButtonPressedEventHandler OnClick;

    public Button()
    {
        _label = new Label
        {
            Position = new Vector2(0f, 0f)
        };
        AddChild(_label);
    }

    public override void Update(GameTime gameTime)
    {
        _label.Text = Text;
        
        var textSize = _label.Font.MeasureString(_label.Text);
        _label.Position = new Vector2(
            (Size.Width - textSize.X) / 2f,
            (Size.Height - textSize.Y) / 2f);
        
        IsHovered = GetGlobalRectangle().Contains(Input.Mouse.Position);
        IsPressed = IsHovered && Input.Mouse.IsLeftButtonDown;
        
        if (IsHovered && Input.Mouse.IsLeftButtonReleased)
            OnClick?.Invoke();
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
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

    private Rectangle GetGlobalRectangle()
    {
        return new Rectangle((int)GlobalPosition.X, (int)GlobalPosition.Y, (int)Size.Width, (int)Size.Height);
    }
}