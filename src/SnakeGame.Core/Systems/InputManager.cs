using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Core.Inputs;

namespace SnakeGame.Core.Systems;

public class InputManager
{
    public KeyboardInputHandler Keyboard { get; } = new();
    public MouseInputHandler Mouse { get; } = new();
    public TouchInputHandler Touch { get; } = new();
    public GamePadInputHandler GamePad { get; } = new();
    
    private readonly Dictionary<string, List<Keys>> _bindings = new();

    public void BindKey(string actionName, params Keys[] keys)
    {
        if (keys.Length == 0)
            throw new ArgumentException($"{nameof(keys)} should have at least one value");
        
        if (!_bindings.ContainsKey(actionName))
            _bindings.Add(actionName, []);
        
        foreach (var key in keys)
            _bindings[actionName].Add(key);
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
        foreach (var key in _bindings[actionName])
        {
            if (Keyboard.GetIsKeyDown(key)) return true;
        }

        return false;
    }

    public bool IsActionPressed(string actionName)
    {
        foreach (var key in _bindings[actionName])
        {
            if (Keyboard.GetIsKeyPressed(key)) return true;
        }

        return false;
    }
    
    public bool IsActionReleased(string actionName)
    {
        foreach (var key in _bindings[actionName])
        {
            if (Keyboard.GetIsKeyReleased(key)) return true;
        }

        return false;
    }
}