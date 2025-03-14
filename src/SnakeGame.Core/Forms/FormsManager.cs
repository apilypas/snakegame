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
            HandleHover(form);
            HandleSelection(form);
            HandleClick(form);
        }
    }

    private void HandleHover(Form form)
    {
        var position = screen.TransformPoint(inputs.Mouse.Position);
        
        form.HoverElement(position.X, position.Y);
    }
    
    private void HandleSelection(Form form)
    {
        var position = screen.TransformPoint(inputs.Mouse.Position);
        
        form.SelectElement(position.X, position.Y, inputs.Mouse.IsLeftButtonDown);
    }

    private void HandleClick(Form form)
    {
        if (inputs.Touch.IsConnected && inputs.Touch.IsTouched)
        {
            foreach (var state in inputs.Touch.Touches)
            {
                if (state.State == TouchLocationState.Pressed)
                {
                    var position = screen.TransformPoint(state.Position);
                    
                    if (DoClick(form, position))
                        return;
                }
            }
        }
        
        if (inputs.Mouse.IsLeftButtonReleased)
        {
            var position = screen.TransformPoint(inputs.Mouse.Position);
            DoClick(form, position);
        }
    }

    private bool DoClick(Form form, Vector2 position)
    {
        foreach (var action in form.Actions)
        {
            if (action.Bounds.Contains(position))
            {
                action.Command.Execute();
                return true;
            }
        }

        return false;
    }
}