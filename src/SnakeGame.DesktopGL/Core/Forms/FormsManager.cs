using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using SnakeGame.DesktopGL.Core.Events;

namespace SnakeGame.DesktopGL.Core.Forms;

public class FormsManager(InputManager inputs)
{
    private int _visibleFormId = -1;
    
    private readonly Dictionary<int, Form> _forms = [];
    
    private KeyboardState _previousState;
    private KeyboardState _currentState;
    private MouseState _mouseState;

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
        inputs.DisableBindings();
        
        _visibleFormId = formId;

        var form = GetVisibleForm();
        
        if (form == null)
            throw new ArgumentException($"Unknown form id {formId}", nameof(formId));
        
        _previousState = _currentState;
        _currentState = inputs.KeyboardState;
        _mouseState = inputs.MouseState;
    }

    public void Close()
    {
        _visibleFormId = -1;
        inputs.EnableBindings();
    }

    public void Update()
    {
        var form = GetVisibleForm();

        if (form != null)
        {
            _previousState = _currentState;
            _currentState = inputs.KeyboardState;
            _mouseState = inputs.MouseState;
            
            HoverElement(form);
            ClickElement(form);
            HandleKeyboardInput(form);
        }
    }

    private void HoverElement(Form form)
    {
        form.HoverElement(_mouseState.X, _mouseState.Y);
    }

    private void ClickElement(Form form)
    {
        if (_mouseState.LeftButton != ButtonState.Pressed)
            return;
        
        foreach (var action in form.Actions)
        {
            if (action.IsHovered)
            {
                action.Command.Execute();
                Close();
                break;
            }
        }
    }

    private void HandleKeyboardInput(Form form)
    {
        foreach (var action in form.Actions)
        {
            if (_currentState.IsKeyDown(action.Key) && _previousState.IsKeyUp(action.Key))
            {
                action.Command.Execute();
                Close();
                break;
            }
        }
    }
}