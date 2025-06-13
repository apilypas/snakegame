using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Entities;

public class Entity
{
    private readonly HashSet<Entity> _children = [];
    
    public int Id { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 GlobalPosition { get; set; }
    public float Rotation { get; set; }
    public bool QueueRemove { get; set; }
    public bool IsPaused { get; set; }
    public bool IsUpdated { get; set; }
    public bool IsVisible { get; set; } = true;
    public ThemeManager Theme { get; set; }

    public virtual void Update(GameTime gameTime) { }
    public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) { }

    public void AddChild(Entity entity)
    {
        if (Theme != null)
        {
            Theme.Apply(entity);
            entity.Theme = Theme;
        }

        _children.Add(entity);
    }

    public void RemoveChild(Entity entity)
    {
        _children.Remove(entity);
    }
    
    public IEnumerable<Entity> GetChildren() => _children;
}