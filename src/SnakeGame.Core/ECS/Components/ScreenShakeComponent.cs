using Microsoft.Xna.Framework;

namespace SnakeGame.Core.ECS.Components;

public class ScreenShakeComponent
{
    public float Timer { get; set; }
    public Vector2 CameraOffset { get; set; }
}