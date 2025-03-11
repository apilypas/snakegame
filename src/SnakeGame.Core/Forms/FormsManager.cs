using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using SnakeGame.Core.Events;
using SnakeGame.Core.Screens;

namespace SnakeGame.Core.Forms;

public class FormsManager(ScreenBase screen, InputManager inputs)
{
    private int _visibleFormId = -1;
    
    private readonly Dictionary<int, Form> _forms = [];
    
    private KeyboardState _previousState;
    private KeyboardState _currentState;
    
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
        inputs.Keyboard.DisableBindings();
        
        _visibleFormId = formId;

        var form = GetVisibleForm();
        
        if (form == null)
            throw new ArgumentException($"Unknown form id {formId}", nameof(formId));
        
        _previousState = _currentState;
        _currentState = inputs.Keyboard.KeyboardState;
    }

    public void Close()
    {
        _visibleFormId = -1;
        inputs.Keyboard.EnableBindings();
    }

    public void Update()
    {
        var form = GetVisibleForm();

        if (form != null)
        {
            _previousState = _currentState;
            _currentState = inputs.Keyboard.KeyboardState;
            
            HoverElement(form);
            ClickElement(form);
            HandleKeyboardInput(form);
        }
    }

    private void HoverElement(Form form)
    {
        var position = CalculateWithScale(inputs.Mouse.State.X, inputs.Mouse.State.Y);
        
        form.HoverElement(position.X, position.Y);
    }

    private bool ClickElement(Form form)
    {
        if (inputs.Touch.IsConnected && inputs.Touch.IsTouched)
        {
            foreach (var state in inputs.Touch.State)
            {
                if (state.State == TouchLocationState.Pressed)
                {
                    var position = CalculateWithScale(state.Position.X, state.Position.Y);
                    
                    if (ClickElement(form, position.X, position.Y))
                    {
                        return true;
                    }
                }
            }
        }
        else if (inputs.Mouse.IsLeftButtonPressed)
        {
            var position = CalculateWithScale(inputs.Mouse.State.X, inputs.Mouse.State.Y);

            if (ClickElement(form, position.X, position.Y))
            {
                return true;
            }
        }

        return false;
    }

    private bool ClickElement(Form form, float x, float y)
    {
        foreach (var action in form.Actions)
        {
            if (action.Bounds.Contains(new Vector2(x, y)))
            {
                action.Command.Execute();
                Close();
                return true;
            }
        }

        return false;
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

    private Vector2 CalculateWithScale(float x, float y)
    {
        var screenScale = screen.GetScale();
        return new Vector2(x / screenScale.X, y / screenScale.Y);
    }
}