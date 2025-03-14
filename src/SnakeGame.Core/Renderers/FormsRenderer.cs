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
            RenderForm(spriteBatch, form);
    }

    private void RenderForm(SpriteBatch spriteBatch, Form form)
    {
        form.CalculateSizes(spriteBatch, _font);
        
        spriteBatch.FillRectangle(form.Location, form.Size, Colors.FormBackgroundColor);
        
        spriteBatch.DrawRectangle(form.Location, form.Size, Colors.DefaultTextColor);

        if (form.Elements.Count > 0)
        {
            spriteBatch.DrawString(
                _font,
                ((FormText)form.Elements[0]).Text,
                form.Elements[0].Location,
                Colors.DefaultTextColor,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f);
        }

        if (form.Actions.Count > 0)
        {
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
                    1f,
                    SpriteEffects.None,
                    0f);
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
    }
}