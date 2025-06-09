using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SnakeGame.Core.Forms;
using SnakeGame.Core.Systems;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.Renderers;

public class FormsRenderer(AssetManager assets, FormsManager formsManager) : RendererBase
{
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
        var font = Globals.IsMobileDevice ? assets.BigFont : assets.MainFont;
        
        form.UpdateSize(spriteBatch, font);

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
                spriteBatch.DrawStringWithShadow(
                    font,
                    formText.Text,
                    formText.Location,
                    Colors.DefaultTextColor,
                    0f,
                    Vector2.Zero);
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

            spriteBatch.DrawStringWithShadow(
                font,
                action.Title,
                action.TitleLocation,
                Colors.DefaultTextColor,
                0f,
                Vector2.Zero);
        }
    }
}