using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using SnakeGame.Core.Inputs;

namespace SnakeGame.Core.Systems;

public class VirtualGamePadManager(InputManager input) : GamePadInputHandler.IVirtualGamePad
{
    public class VirtualGamePadButton
    {
        public Vector2 Position { get; set; }
        public Buttons GamePadButton { get; set; }
        public bool IsPressed { get; set; }
    }
    
    public bool IsConnected { get; } = input.Touch.IsConnected;
    public bool IsVisible { get; set; } = true;
    
    public VirtualGamePadButton LeftButton { get; } = new() { GamePadButton = Buttons.DPadLeft };
    public VirtualGamePadButton RightButton { get; } = new() { GamePadButton = Buttons.DPadRight };
    public VirtualGamePadButton UpButton { get; } = new() { GamePadButton = Buttons.DPadUp };
    public VirtualGamePadButton DownButton { get; } = new() { GamePadButton = Buttons.DPadDown };
    public VirtualGamePadButton ActionButton { get; } = new() { GamePadButton = Buttons.A };
    public VirtualGamePadButton StartButton { get; } = new() { GamePadButton = Buttons.Start };
    
    public void Update()
    {
        if (!IsConnected || !IsVisible)
            return;
        
        LeftButton.Position = new Vector2(10, Globals.VirtualScreenHeight - 150);
        RightButton.Position = new Vector2(130, Globals.VirtualScreenHeight - 150);
        UpButton.Position = new Vector2(70, Globals.VirtualScreenHeight - 210);
        DownButton.Position = new Vector2(70, Globals.VirtualScreenHeight - 90);
        ActionButton.Position = new Vector2(Globals.VirtualScreenWidth - 180, Globals.VirtualScreenHeight - 180);
        StartButton.Position = new Vector2(70, 70);

        var pressedPoints = input.Touch.GetTouchedPoints().ToList();
        
        HandleButton(LeftButton, pressedPoints);
        HandleButton(RightButton, pressedPoints);
        HandleButton(UpButton, pressedPoints);
        HandleButton(DownButton, pressedPoints);
        HandleButton(ActionButton, pressedPoints);
        HandleButton(StartButton, pressedPoints);
    }

    public GamePadState GetState(GamePadState gamePadState)
    {
        var buttons = Buttons.None;

        buttons |= GetPressedButton(gamePadState, UpButton);
        buttons |= GetPressedButton(gamePadState, DownButton);
        buttons |= GetPressedButton(gamePadState, LeftButton);
        buttons |= GetPressedButton(gamePadState, RightButton);
        buttons |= GetPressedButton(gamePadState, ActionButton);
        buttons |= GetPressedButton(gamePadState, StartButton);

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

    private void HandleButton(VirtualGamePadButton button, IList<Vector2> touchPoints)
    {
        var isPressed = false;
        
        foreach (var touchPoint in touchPoints)
        {
            var rectangle = new RectangleF(button.Position.X, button.Position.Y, 64, 64);
            rectangle.Inflate(20f, 20f); // Give bigger space for button so player can press multiple of them

            if (rectangle.Contains(touchPoint))
            {
                isPressed = true;
                break;
            }
        }
        
        button.IsPressed = isPressed;
    }
}