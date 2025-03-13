using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Extended;
using SnakeGame.Core.Events;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Screens;

namespace SnakeGame.Core;

public class VirtualGamePad(ScreenBase screen, InputManager input) : IVirtualGamePad
{
    public class VirtualGamePadButton
    {
        public Vector2 Position { get; set; }
        public Buttons GamePadButton { get; set; }
        public bool IsPressed { get; set; }
    }
    
    public bool IsConnected { get; } = input.Touch.IsConnected;
    
    public VirtualGamePadButton LeftButton { get; } = new() { GamePadButton = Buttons.DPadLeft };
    public VirtualGamePadButton RightButton { get; } = new() { GamePadButton = Buttons.DPadRight };
    public VirtualGamePadButton UpButton { get; } = new() { GamePadButton = Buttons.DPadUp };
    public VirtualGamePadButton DownButton { get; } = new() { GamePadButton = Buttons.DPadDown };
    public VirtualGamePadButton ActionButton { get; } = new() { GamePadButton = Buttons.A };
    public VirtualGamePadButton PauseButton { get; } = new() { GamePadButton = Buttons.Start };
    
    public void Update()
    {
        if (!IsConnected)
            return;
        
        LeftButton.Position = new Vector2(10, screen.VirtualHeight - 150);
        RightButton.Position = new Vector2(130, screen.VirtualHeight - 150);
        UpButton.Position = new Vector2(70, screen.VirtualHeight - 210);
        DownButton.Position = new Vector2(70, screen.VirtualHeight - 90);
        ActionButton.Position = new Vector2(screen.VirtualWidth - 150, screen.VirtualHeight - 150);
        PauseButton.Position = new Vector2(70, 70); 
        
        foreach (var touch in input.Touch.Touches)
        {
            HandleButton(LeftButton, touch);
            HandleButton(RightButton, touch);
            HandleButton(UpButton, touch);
            HandleButton(DownButton, touch);
            HandleButton(ActionButton, touch);
            HandleButton(PauseButton, touch);
        }
    }

    public GamePadState GetState(GamePadState gamePadState)
    {
        var buttons = Buttons.None;

        buttons |= GetPressedButton(gamePadState, UpButton);
        buttons |= GetPressedButton(gamePadState, DownButton);
        buttons |= GetPressedButton(gamePadState, LeftButton);
        buttons |= GetPressedButton(gamePadState, RightButton);
        buttons |= GetPressedButton(gamePadState, ActionButton);
        buttons |= GetPressedButton(gamePadState, PauseButton);

        return new GamePadState(
            gamePadState.ThumbSticks,
            gamePadState.Triggers,
            new GamePadButtons(buttons),
            gamePadState.DPad);
    }

    private Buttons GetPressedButton(GamePadState gamePadState, VirtualGamePadButton button)
    {
        if (gamePadState.IsButtonDown(button.GamePadButton))
            return button.GamePadButton;
        
        if (IsConnected && button.IsPressed)
            return button.GamePadButton;
        
        return Buttons.None;
    }

    private void HandleButton(VirtualGamePadButton button, TouchLocation touch)
    {
        button.IsPressed = false;

        if (touch.State != TouchLocationState.Pressed && touch.State != TouchLocationState.Moved)
            return;

        var touchPoint = screen.TransformPoint(touch.Position);
        var rectangle = new RectangleF(button.Position.X, button.Position.Y, 64, 64);
        rectangle.Inflate(20f, 20f); // Give bigger space for button so player can press multiple of them

        button.IsPressed = rectangle.Contains(touchPoint);
    }
}