using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Core.Entities;

public class Entity
{
    public int Id { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 GlobalPosition { get; set; }
    public float Rotation { get; set; }
    public bool QueueRemove { get; set; }
    public bool IsPaused { get; set; }
    public bool IsUpdated { get; set; }
    
    public HashSet<Entity> Children { get; } = [];

    public virtual void Update(GameTime gameTime) { }
    public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) { }
    public void Add(Entity entity) { Children.Add(entity); }
    public void Remove(Entity entity) { Children.Remove(entity); }
}