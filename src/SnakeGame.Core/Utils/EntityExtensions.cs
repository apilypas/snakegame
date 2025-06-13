using Microsoft.Xna.Framework;
using SnakeGame.Core.Entities;

namespace SnakeGame.Core.Utils;

public static class EntityExtensions
{
    public static void UpdateEntityTree(this Entity entity, GameTime gameTime, Vector2? basePosition = null)
    {
        if (entity.IsPaused) return;

        if (!entity.IsVisible) return;

        if (null == basePosition)
            basePosition = Vector2.Zero;

        entity.IsUpdated = false;
        entity.Update(gameTime);
        entity.GlobalPosition = basePosition.Value + entity.Position;
        entity.IsUpdated = true;

        foreach (var child in entity.GetChildren())
        {
            if (child.QueueRemove)
            {
                entity.RemoveChild(child);
                continue;
            }
            
            UpdateEntityTree(child, gameTime, entity.GlobalPosition);
        }
    }
}