using System;
using MonoGame.Extended;

namespace SnakeGame.Core.ECS.Components;

public class ButtonComponent
{
    public SizeF Size { get; set; }
    public string Text { get; set; }
    public bool IsHovered { get; set; }
    public bool IsPressed { get; set; }
    public Action Action { get; set; }
}