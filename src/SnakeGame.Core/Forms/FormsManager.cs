using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Screens;

namespace SnakeGame.Core.Forms;

public class FormsManager(ScreenBase screen, InputManager inputs)
{
    private int _visibleFormId = -1;
    
    private readonly Dictionary<int, Form> _forms = [];
    
    public void Add(Form form)
    {
        _forms[form.Id] = form;
    }

    public Form GetVisibleForm()
    {
        if (_visibleFormId == -1)
            return null;
        
        return _forms.GetValueOrDefault(_visibleFormId);
    }

    public void Show(int formId)
    {
        _visibleFormId = formId;

        var form = GetVisibleForm();
        
        if (form == null)
            throw new ArgumentException($"Unknown form id {formId}", nameof(formId));
    }

    public void Close()
    {
        _visibleFormId = -1;
    }

    public void Update()
    {
        var form = GetVisibleForm();

        if (form != null)
        {
            HoverElement(form);
            ClickElement(form);
        }
    }

    private void HoverElement(Form form)
    {
        var position = screen.TransformPoint(new Vector2(inputs.Mouse.State.X, inputs.Mouse.State.Y));
        
        form.HoverElement(position.X, position.Y);
    }

    private void ClickElement(Form form)
    {
        if (inputs.Touch.IsConnected && inputs.Touch.IsTouched)
        {
            foreach (var state in inputs.Touch.Touches)
            {
                if (state.State == TouchLocationState.Pressed)
                {
                    var position = screen.TransformPoint(state.Position);
                    
                    if (ClickElement(form, position.X, position.Y))
                        return;
                }
            }
        }
        else if (inputs.Mouse.IsLeftButtonDown)
        {
            var position = screen.TransformPoint(new Vector2(inputs.Mouse.State.X, inputs.Mouse.State.Y));

            ClickElement(form, position.X, position.Y);
        }
    }

    private bool ClickElement(Form form, float x, float y)
    {
        foreach (var action in form.Actions)
        {
            if (action.Bounds.Contains(new Vector2(x, y)))
            {
                action.Command.Execute();
                return true;
            }
        }

        return false;
    }
}