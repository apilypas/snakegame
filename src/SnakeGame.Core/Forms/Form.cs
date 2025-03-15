using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace SnakeGame.Core.Forms;

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

    public void UpdateSize(SpriteBatch spriteBatch, SpriteFont font)
    {
        if (!_shouldResize)
            return;
        
        UpdateContentSize(font);
        UpdateActionsSize(font);
        UpdateContentMarginSize();
        UpdateActionsMarginSize();
        UpdateContentAlignment();
        UpdateActionsAlignment();
        UpdateFormSize();
        UpdateActionCentering();
        UpdateAllToScreenCenter(spriteBatch);

        _shouldResize = false;
    }

    private void UpdateAllToScreenCenter(SpriteBatch spriteBatch)
    {
        var screenWidth = spriteBatch.GraphicsDevice.Viewport.Width;
        var screenHeight = spriteBatch.GraphicsDevice.Viewport.Height;
        var offset = new Vector2(screenWidth - Size.Width, screenHeight - Size.Height) / 2f;
        
        Location += offset;
        
        foreach (var element in Elements)
        {
            element.Location += offset;
        }

        foreach (var action in Actions)
        {
            action.Location += offset;
            action.TitleLocation += offset;
        }
    }

    private void UpdateActionCentering()
    {
        var actionButtonsWidth = Actions.Sum(x => x.Size.Width);
        actionButtonsWidth += Actions.Count * ButtonMarginSize;
        var actionButtonOffset = (Size.Width - actionButtonsWidth - ButtonMarginSize) / 2f;
        foreach (var action in Actions)
        {
            action.Location += new Vector2(actionButtonOffset, 0f);
            action.TitleLocation += new Vector2(actionButtonOffset, 0f);
        }
    }

    private void UpdateFormSize()
    {
        var formWidth = MathF.Max(
            Elements.Max(x => x.Size.Width),
            Actions.Sum(x => x.Size.Width + ButtonMarginSize) + ButtonMarginSize);
        var formHeight = Elements.Sum(x => x.Size.Height);
        formHeight += Actions.Max(x => x.Size.Height);
        formHeight += ButtonMarginSize * 2f;
        
        Location = Vector2.Zero;
        Size = new SizeF(formWidth, formHeight);
    }

    private void UpdateActionsAlignment()
    {
        var actionX = 0f;
        var actionY = Elements.Sum(x => x.Size.Height);
        foreach (var action in Actions)
        {
            action.Location += new Vector2(actionX + ButtonMarginSize, actionY + ButtonMarginSize);
            action.TitleLocation += new Vector2(actionX + ButtonMarginSize, actionY + ButtonMarginSize);
            actionX += action.Size.Width + ButtonMarginSize;
        }
    }

    private void UpdateContentAlignment()
    {
        var contentY = 0f;
        foreach (var element in Elements)
        {
            element.Location += new Vector2(0f, contentY);
            contentY += element.Size.Height;
        }
    }

    private void UpdateActionsMarginSize()
    {
        foreach (var action in Actions)
        {
            action.TitleLocation += new Vector2(ButtonMarginSize, ButtonMarginSize);
            action.Size += new SizeF(2f * ButtonMarginSize, 2f * ButtonMarginSize);
        }
    }

    private void UpdateContentMarginSize()
    {
        for (var i = 0; i < Elements.Count; i++)
        {
            var element = Elements[i];
            element.Location += new Vector2(ContentMarginSize, i == 0 ? ContentMarginSize : 0f);
            element.Size += new SizeF(2f * ContentMarginSize, i == 0 ? (2f * ContentMarginSize) : 0f);
        }
    }

    private void UpdateActionsSize(SpriteFont font)
    {
        foreach (var action in Actions)
        {
            var textSize = font.MeasureString(action.Title);
            action.Location = Vector2.Zero;
            action.Size = textSize;
            action.TitleLocation = Vector2.Zero;
        }
    }

    private void UpdateContentSize(SpriteFont font)
    {
        foreach (var element in Elements)
        {
            if (element is FormText formText)
            {
                var textSize = font.MeasureString(formText.Text);
                element.Location = Vector2.One;
                element.Size = new SizeF(textSize.X, textSize.Y);
            }
        }
    }

    public void HoverElement(float x, float y)
    {
        foreach (var action in Actions)
        {
            action.IsHovered = action.Bounds.Contains(new Vector2(x, y));
        }
    }
    
    public void PressElement(float x, float y)
    {
        foreach (var action in Actions)
        {
            action.IsPressed = action.Bounds.Contains(new Vector2(x, y));
        }
    }

    public int GetFocusedActionIndex()
    {
        var focusedIndex = -1;

        for (var i = 0; i < Actions.Count; i++)
        {
            if (Actions[i].IsFocused)
            {
                focusedIndex = i;
                break;
            }
        }

        return focusedIndex;
    }

    public void FocusByIndex(int focusedIndex)
    {
        if (focusedIndex < Actions.Count && focusedIndex >= 0)
        {
            foreach (var action in Actions)
            {
                action.IsFocused = false;
            }
                
            Actions[focusedIndex].IsFocused = true;
        }
    }

    public void Unfocus()
    {
        foreach (var action in Actions)
        {
            action.IsFocused = false;
        }
    }
}