using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace SnakeGame.Core.Services;

public struct InputBinding
{
    public string ActionName;
    public Keys[] Keys;
    public Buttons Button;
    
    public static InputBinding Create(string actionName, params Keys[] keys)
    {
        return new InputBinding
        {
            ActionName = actionName,
            Keys = keys
        };
    }

    public static InputBinding Create(string actionName, Buttons button)
    {
        return new InputBinding
        {
            ActionName = actionName,
            Button = button
        };
    }
}

public class InputManager
{
    private readonly List<InputBinding> _bindings = [];

    public KeyboardInputHandler KeyboardInput { get; } = new();
    public MouseInputHandler MouseInput { get; } = new();
    public GamePadInputHandler GamePadInput { get; } = new(PlayerIndex.One);

    public InputManager(IEnumerable<InputBinding> bindings)
    {
        foreach (var binding in bindings)
            _bindings.Add(binding);
        
        SortBindings();
    }

    public void Update()
    {
        KeyboardInput.Update();
        MouseInput.Update();
        GamePadInput.Update();
    }

    public bool IsActionDown(string actionName)
    {
        foreach (var binding in _bindings)
        {
            if (binding.ActionName == actionName && IsActionDown(binding))
                return true;
        }

        return false;
    }

    public bool WasActionPressed(string actionName)
    {
        foreach (var binding in _bindings)
        {
            if (binding.ActionName == actionName && WasActionPressed(binding))
                return true;
        }

        return false;
    }

    public bool WasActionReleased(string actionName)
    {
        foreach (var binding in _bindings)
        {
            if (binding.ActionName == actionName && WasActionReleased(binding))
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

    private bool IsActionDown(InputBinding binding)
    {
        if (binding.Keys is { Length: 1 }
            && KeyboardInput.IsKeyDown(binding.Keys[0]))
        {
            return true;
        }

        if (binding.Keys is { Length: 2 }
            && KeyboardInput.IsKeyDown(binding.Keys[0])
            && KeyboardInput.IsKeyDown(binding.Keys[1]))
        {
            return true;
        }

        if (binding.Button != Buttons.None && GamePadInput.IsButtonDown(binding.Button))
            return true;
        
        return false;
    }
    
    private bool WasActionPressed(InputBinding binding)
    {
        if (binding.Keys is { Length: 1 }
            && KeyboardInput.WasKeyPressed(binding.Keys[0]))
        {
            return true;
        }

        if (binding.Keys is { Length: 2 }
            && KeyboardInput.IsKeyDown(binding.Keys[0])
            && KeyboardInput.WasKeyPressed(binding.Keys[1]))
        {
            return true;
        }

        if (binding.Button != Buttons.None && GamePadInput.WasButtonPressed(binding.Button))
            return true;

        return false;
    }
    
    private bool WasActionReleased(InputBinding binding)
    {
        if (binding.Keys is { Length: 1 }
            && KeyboardInput.WasKeyReleased(binding.Keys[0]))
        {
            return true;
        }

        if (binding.Keys is { Length: 2 }
            && KeyboardInput.IsKeyDown(binding.Keys[0])
            && KeyboardInput.WasKeyReleased(binding.Keys[1]))
        {
            return true;
        }

        if (binding.Button != Buttons.None && GamePadInput.WasButtonReleased(binding.Button))
            return true;

        return false;
    }
    
    public class KeyboardInputHandler
    {
        private KeyboardStateExtended _keyboardState;
        
        public void Update()
        {
            KeyboardExtended.Update();
            _keyboardState = KeyboardExtended.GetState();
        }

        public bool IsKeyDown(Keys key)
        {
            return _keyboardState.IsKeyDown(key);
        }

        public bool WasKeyReleased(Keys key)
        {
            return _keyboardState.WasKeyReleased(key);
        }

        public bool WasKeyPressed(Keys key)
        {
            return _keyboardState.WasKeyPressed(key);
        }
    }

    public class MouseInputHandler
    {
        private MouseStateExtended _mouseState;
        
        public Vector2 MousePosition => new(_mouseState.X, _mouseState.Y);

        public void Update()
        {
            MouseExtended.Update();
            _mouseState = MouseExtended.GetState();
        }
        
        public bool IsButtonDown(MouseButton button)
        {
            return _mouseState.IsButtonDown(button);
        }

        public bool WasButtonPressed(MouseButton button)
        {
            return _mouseState.WasButtonPressed(button);
        }

        public bool WasButtonReleased(MouseButton button)
        {
            return _mouseState.WasButtonReleased(button);
        }
    }

    public class GamePadInputHandler(PlayerIndex playerIndex)
    {
        private GamePadState _previousState = GamePad.GetState(playerIndex);
        private GamePadState _currentState = GamePad.GetState(playerIndex);

        public bool IsButtonDown(Buttons button)
        {
            return _currentState.IsButtonDown(button);
        }
        
        public bool WasButtonPressed(Buttons button)
        {
            return _currentState.IsButtonDown(button)
                   && _previousState.IsButtonUp(button);
        }

        public bool WasButtonReleased(Buttons button)
        {
            return _currentState.IsButtonUp(button)
                   && _previousState.IsButtonDown(button);
        }

        public void Update()
        {
            _previousState = _currentState;
            _currentState = GamePad.GetState(playerIndex);
        }
    }
}