using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SnakeGame.Core.Forms;

namespace SnakeGame.Core.Renderers;

public class FormsRenderer(FormsManager formsManager) : RendererBase
{
    private SpriteFont _font;
    
    public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _font = content.Load<SpriteFont>("MainFont");
    }

    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        var form = formsManager.GetVisibleForm();

        if (form != null)
        {
            RenderForm(spriteBatch, form);
        }
    }

    private void RenderForm(SpriteBatch spriteBatch, Form form)
    {
        var scale = Globals.IsMobileDevice ? new Vector2(2f, 2f) : Vector2.One;
        
        form.UpdateSize(spriteBatch, _font, scale);

        if (Globals.IsMobileDevice)
        {
            spriteBatch.FillRectangle(
                0f,
                0f,
                spriteBatch.GraphicsDevice.Viewport.Width,
                spriteBatch.GraphicsDevice.Viewport.Height,
                Colors.FormBackgroundColor);
        }
        else
        {
            spriteBatch.FillRectangle(form.Location, form.Size, Colors.FormBackgroundColor);
            spriteBatch.DrawRectangle(form.Location, form.Size, Colors.DefaultTextColor);
        }

        foreach (var element in form.Elements)
        {
            if (element is FormText formText)
            {
                spriteBatch.DrawString(
                    _font,
                    formText.Text,
                    formText.Location,
                    Colors.DefaultTextColor,
                    0f,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    0f);
            }
        }

        foreach (var action in form.Actions)
        {
            if (action.IsPressed)
            {
                spriteBatch.FillRectangle(
                    action.Location,
                    action.Size,
                    Colors.FormButtonSelectedColor
                    );
            }
            else if (action.IsHovered)
            {
                spriteBatch.FillRectangle(
                    action.Location,
                    action.Size,
                    Colors.FormButtonHoverColor
                    );
            }
            else
            {
                spriteBatch.FillRectangle(
                    action.Location,
                    action.Size,
                    Colors.FormButtonColor
                    );
            }

            if (action.IsFocused)
            {
                spriteBatch.DrawRectangle(
                    action.Location,
                    action.Size,
                    Colors.FormButtonSelectedColor,
                    3f
                    );
            }

            spriteBatch.DrawString(
                _font,
                action.Title,
                action.TitleLocation,
                Colors.DefaultTextColor,
                0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0f);
        }
    }

    public override void Update(GameTime gameTime)
    {
    }
}