using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Inputs;

namespace SnakeGame.Core.Systems;

public class InputManager
{
    private readonly List<InputBinding> _bindings = [];
    private readonly Entity _baseEntity;

    private struct InputBinding
    {
        public string ActionName;
        public Keys[] Keys;
    }

    public KeyboardInputHandler Keyboard { get; } = new();
    public MouseInputHandler Mouse { get; } = new();
    public TouchInputHandler Touch { get; } = new();
    public GamePadInputHandler GamePad { get; } = new();


    public InputManager(Entity baseEntity)
    {
        _baseEntity = baseEntity;
    }

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
        
        // Priority for multi key shortcuts with more keys
        _bindings.Sort((a, b) => b.Keys.Length.CompareTo(a.Keys.Length));
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
            if (binding.ActionName == actionName)
            {
                if (binding.Keys.Length == 1)
                    return Keyboard.GetIsKeyDown(binding.Keys[0]);
                
                if (binding.Keys.Length == 2)
                    return Keyboard.GetIsKeyDown(binding.Keys[0])
                        && Keyboard.GetIsKeyDown(binding.Keys[1]);
            }
        }

        return false;
    }

    public bool IsActionPressed(string actionName)
    {
        foreach (var binding in _bindings)
        {
            if (binding.ActionName == actionName)
            {
                if (binding.Keys.Length == 1
                    && Keyboard.GetIsKeyPressed(binding.Keys[0]))
                {
                    return true;
                }

                if (binding.Keys.Length == 2
                    && Keyboard.GetIsKeyDown(binding.Keys[0])
                    && Keyboard.GetIsKeyPressed(binding.Keys[1]))
                {
                    return true;
                }
            }
        }

        return false;
    }
    
    public bool IsActionReleased(string actionName)
    {
        foreach (var binding in _bindings)
        {
            if (binding.ActionName == actionName)
            {
                if (binding.Keys.Length == 1
                    && Keyboard.GetIsKeyReleased(binding.Keys[0]))
                {
                    return true;
                }

                if (binding.Keys.Length == 2 
                    && Keyboard.GetIsKeyDown(binding.Keys[0])
                    && Keyboard.GetIsKeyReleased(binding.Keys[1]))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void Apply()
    {
        ApplyTo(_baseEntity);
    }

    public void Remove()
    {
        RemoveFrom(_baseEntity);
    }

    public void ApplyTo(Entity entity)
    {
        if (entity is Control control)
            control.Inputs = this;
        
        foreach (var child in entity.GetChildren())
            ApplyTo(child);
    }

    public void RemoveFrom(Entity entity)
    {
        if (entity is Control control)
            control.Inputs = null;
        
        foreach (var child in entity.GetChildren())
            RemoveFrom(child);
    }
}