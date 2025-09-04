using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Core.Inputs;

namespace SnakeGame.Core.Services;

public class InputManager
{
    private readonly List<InputBinding> _bindings = [];
    
    private struct InputBinding
    {
        public string ActionName;
        public Keys[] Keys;
        public Buttons Button;
    }

    public KeyboardInputHandler Keyboard { get; } = new();
    public MouseInputHandler Mouse { get; } = new();
    public TouchInputHandler Touch { get; } = new();
    public GamePadInputHandler GamePad { get; } = new();

    public void BindKey(string actionName, params Keys[] keys)
    {
        if (keys.Length is <= 0 or > 2)
            throw new ArgumentException($"{nameof(keys)} should have one or two values");
        
        var inputBinding = new InputBinding
        {
            ActionName = actionName,
            Keys = keys
        };
        
        _bindings.Add(inputBinding);
        
        SortBindings();
    }

    public void BindButton(string actionName, Buttons button)
    {
        var inputBinding = new InputBinding
        {
            ActionName = actionName,
            Button = button
        };
        
        _bindings.Add(inputBinding);
        
        SortBindings();
    }

    public void Update()
    {
        Keyboard.Update();
        Mouse.Update();
        Touch.Update();
        GamePad.Update();
    }

    public bool IsActionDown(string actionName)
    {
        foreach (var binding in _bindings)
        {
            if (binding.ActionName == actionName && GetIsDown(binding))
                return true;
        }

        return false;
    }

    public bool IsActionPressed(string actionName)
    {
        foreach (var binding in _bindings)
        {
            if (binding.ActionName == actionName && GetIsPressed(binding))
                return true;
        }

        return false;
    }

    public bool IsActionReleased(string actionName)
    {
        foreach (var binding in _bindings)
        {
            if (binding.ActionName == actionName && GetIsReleased(binding))
                return true;
        }

        return false;
    }

    private void SortBindings()
    {
        // Priority for multi key shortcuts with more keys
        _bindings.Sort((a, b) => 
            b.Keys != null && a.Keys != null
                ? b.Keys.Length.CompareTo(a.Keys.Length)
                : int.MaxValue);
    }

    private bool GetIsDown(InputBinding binding)
    {
        if (binding.Keys is { Length: 1 }
            && Keyboard.GetIsKeyDown(binding.Keys[0]))
        {
            return true;
        }

        if (binding.Keys is { Length: 2 }
            && Keyboard.GetIsKeyDown(binding.Keys[0])
            && Keyboard.GetIsKeyDown(binding.Keys[1]))
        {
            return true;
        }

        if (binding.Button != Buttons.None
            && GamePad.GetIsButtonDown(binding.Button))
        {
            return true;
        }
        
        return false;
    }
    
    private bool GetIsPressed(InputBinding binding)
    {
        if (binding.Keys is { Length: 1 }
            && Keyboard.GetIsKeyPressed(binding.Keys[0]))
        {
            return true;
        }

        if (binding.Keys is { Length: 2 }
            && Keyboard.GetIsKeyDown(binding.Keys[0])
            && Keyboard.GetIsKeyPressed(binding.Keys[1]))
        {
            return true;
        }

        if (binding.Button != Buttons.None
            && GamePad.GetIsButtonPressed(binding.Button))
        {
            return true;
        }

        return false;
    }
    
    private bool GetIsReleased(InputBinding binding)
    {
        if (binding.Keys is { Length: 1 }
            && Keyboard.GetIsKeyReleased(binding.Keys[0]))
        {
            return true;
        }

        if (binding.Keys is { Length: 2 }
            && Keyboard.GetIsKeyDown(binding.Keys[0])
            && Keyboard.GetIsKeyReleased(binding.Keys[1]))
        {
            return true;
        }

        if (binding.Button != Buttons.None
            && GamePad.GetIsButtonReleased(binding.Button))
        {
            return true;
        }

        return false;
    }
}