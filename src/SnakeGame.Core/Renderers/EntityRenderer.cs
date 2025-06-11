using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Entities;

namespace SnakeGame.Core.Renderers;

public class EntityRenderer : RendererBase
{
    private readonly Entity _entity;

    public EntityRenderer(Entity entity)
    {
        _entity = entity;
    }
    
    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        Render(spriteBatch, _entity.Position, _entity, gameTime);
    }

    private void Render(SpriteBatch spriteBatch, Vector2 position, Entity entity, GameTime gameTime)
    {
        entity.Draw(spriteBatch, position, gameTime);
        
        foreach (var child in entity.Children)
        {
            Render(spriteBatch, position + child.Position, child, gameTime);
        }
    }
}