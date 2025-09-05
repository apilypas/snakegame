using System.Collections.Generic;
using MonoGame.Extended;

namespace SnakeGame.Core.ECS.Components;

public class DialogComponent
{
    public string Title { get; set; }
    public string Content { get; set; }
    public bool IsTransparent { get; set; }
    public SizeF Size { get; set; }
    public List<int> ChildrenEntities { get; set; } = [];
    public bool IsDestroyed { get; set; }
}