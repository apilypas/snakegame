using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Core.Core.Systems;
using SnakeGame.Core.Inputs;

namespace SnakeGame.Core.Forms;

public class FormsManager(InputManager inputs, VirtualGamePadManager virtualGamePadManager)
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
        
        if (virtualGamePadManager != null)
            virtualGamePadManager.IsVisible = false;
    }

    public void Close()
    {
        var form = GetVisibleForm();

        if (form != null)
        {
            form.Unfocus();
        }
        
        _visibleFormId = -1;
        
        if (virtualGamePadManager != null)
            virtualGamePadManager.IsVisible = true;
    }

    public void Update()
    {
        var form = GetVisibleForm();

        if (form != null)
        {
            HandleHover(form);
            HandlePress(form);
            HandleFocus(form);
            HandleClick(form);
        }
    }

    private void HandleFocus(Form form)
    {
        if (inputs.Keyboard.GetIsKeyPressed(Keys.Right) 
            || inputs.Keyboard.GetIsKeyPressed(Keys.Down)
            || (inputs.GamePad.IsConnected && inputs.GamePad.GetIsButtonPressed(Buttons.DPadRight))
            || (inputs.GamePad.IsConnected && inputs.GamePad.GetIsButtonPressed(Buttons.DPadDown)))
        {
            if (form.Actions.Count > 0)
            {
                var focusedIndex = form.GetFocusedActionIndex();
                focusedIndex++;
                form.FocusByIndex(focusedIndex);
            }
        }
        
        if (inputs.Keyboard.GetIsKeyPressed(Keys.Left)
            || inputs.Keyboard.GetIsKeyPressed(Keys.Up)
            || (inputs.GamePad.IsConnected && inputs.GamePad.GetIsButtonPressed(Buttons.DPadLeft))
            || (inputs.GamePad.IsConnected && inputs.GamePad.GetIsButtonPressed(Buttons.DPadUp)))
        {
            if (form.Actions.Count > 0)
            {
                var focusedIndex = form.GetFocusedActionIndex();
                focusedIndex--;
                form.FocusByIndex(focusedIndex);
            }
        }

        if (inputs.Keyboard.GetIsKeyPressed(Keys.Space)
            || inputs.Keyboard.GetIsKeyPressed(Keys.Enter)
            || (inputs.GamePad.IsConnected && inputs.GamePad.GetIsButtonPressed(Buttons.Start))
            || (inputs.GamePad.IsConnected && inputs.GamePad.GetIsButtonPressed(Buttons.A)))
        {
            if (form.Actions.Count > 0)
            {
                var focusedIndex = form.GetFocusedActionIndex();
                if (focusedIndex >= 0 && focusedIndex < form.Actions.Count)
                {
                    form.Actions[focusedIndex].Command.Execute();
                }
            }
        }
    }

    private void HandleHover(Form form)
    {
        form.HoverElement(inputs.Mouse.Position.X, inputs.Mouse.Position.Y);
    }
    
    private void HandlePress(Form form)
    {
        if (inputs.Touch.IsConnected)
        {
            foreach (var touchPoint in inputs.Touch.GetTouchedPoints())
            {
                form.PressElement(touchPoint.X, touchPoint.Y);
            }
        }

        if (inputs.Mouse.IsLeftButtonDown)
        {
            form.PressElement(inputs.Mouse.Position.X, inputs.Mouse.Position.Y);
        }
    }

    private void HandleClick(Form form)
    {
        if (inputs.Touch.IsConnected)
        {
            foreach (var touchPoint in inputs.Touch.GetReleasedPoints())
            {
                DoClick(form, touchPoint);
            }
        }
        
        if (inputs.Mouse.IsLeftButtonReleased)
        {
            DoClick(form, inputs.Mouse.Position);
        }
    }

    private void DoClick(Form form, Vector2 position)
    {
        foreach (var action in form.Actions)
        {
            if (action.Bounds.Contains(position))
            {
                action.Command.Execute();
                break;
            }
        }
    }
}