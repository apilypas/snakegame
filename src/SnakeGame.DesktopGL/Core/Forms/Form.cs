using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace SnakeGame.DesktopGL.Core.Forms;

public class Form(int id)
{
    private const float ContentMarginSize = 40f;
    private const float ButtonMarginSize = 10f;
    
    public IList<FormElement> Elements { get; } = [];
    public IList<FormAction> Actions { get; } = [];

    public int Id { get; private set; } = id;
    public Vector2 Location { get; private set; } = Vector2.Zero;
    public SizeF Size { get; private set; } = SizeF.Empty;

    private bool _shouldResize = true;

    protected void Add(FormElement element)
    {
        Elements.Add(element);
    }

    protected void Add(FormAction action)
    {
        Actions.Add(action);
    }

    public void CalculateSizes(SpriteBatch spriteBatch, SpriteFont font)
    {
        if (!_shouldResize)
            return;
        
        var contentSize = new SizeF(0f, 0f);
        var margin = new Vector2(ContentMarginSize, ContentMarginSize);

        foreach (var element in Elements)
        {
            var textSize = font.MeasureString(((FormText)element).Text);
            
            contentSize.Width = MathF.Max(contentSize.Width, textSize.X);
            contentSize.Height = textSize.Y;
            
            element.Location = margin;
            element.Size = new SizeF(textSize.X, textSize.Y);
        }

        var formSize = contentSize + (2f * margin).ToSize();
        
        Location = Vector2.Zero;
        Size = formSize;

        var screenWidth = spriteBatch.GraphicsDevice.Viewport.Width;
        var screenHeight = spriteBatch.GraphicsDevice.Viewport.Height;
        
        if (Actions.Count > 0)
        {
            var actionsHeight = 0f;
            
            foreach (var action in Actions)
            {
                var textSize = font.MeasureString(action.Title);
                actionsHeight = MathF.Max(actionsHeight, textSize.Y);
                
                action.Location = new Vector2(0, Size.Height + ButtonMarginSize);
                action.Size = textSize + new Vector2(ButtonMarginSize * 2f, ButtonMarginSize * 2f);
                action.TitleLocation = new Vector2(
                    action.Location.X + (action.Size.Width - textSize.X) / 2f,
                    action.Location.Y + (action.Size.Height - textSize.Y) / 2f
                    );
            }

            var totalButtonWidth = Actions.Sum(x => x.Size.Width) + (Actions.Count - 1) * ButtonMarginSize;
            var totalButtonOffset = new Vector2((Size.Width - totalButtonWidth) / 2f, 0f);
            
            for (var i = 0; i < Actions.Count; i++)
            {
                var action = Actions[i];
                
                if (i > 0)
                {
                    var previousAction = Actions[i - 1];
                    var buttonOffset = new Vector2(
                        ButtonMarginSize + previousAction.Location.X + previousAction.Size.Width,
                        0f);
                    action.Location += buttonOffset;
                    action.TitleLocation += buttonOffset;
                }
                else
                {
                    action.Location += totalButtonOffset;
                    action.TitleLocation += totalButtonOffset;
                }
            }

            Size += new SizeF(0f, actionsHeight + 4f * ButtonMarginSize);
        }

        // Center to screen
        var offset = new Vector2(screenWidth - Size.Width, screenHeight - Size.Height) / 2f;
        Location += offset;
        
        foreach (var element in Elements)
            element.Location += offset;

        foreach (var action in Actions)
        {
            action.Location += offset;
            action.TitleLocation += offset;
        }

        _shouldResize = false;
    }

    public void HoverElement(int x, int y)
    {
        foreach (var action in Actions)
        {
            action.IsHovered = action.IsMouseOver(x, y);
        }
    }
}