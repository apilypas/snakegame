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
        Render(spriteBatch, _entity, gameTime);
    }

    private static void Render(SpriteBatch spriteBatch, Entity entity, GameTime gameTime)
    {
        if (!entity.IsUpdated)
            return;

        if (!entity.IsVisible)
            return;

        entity.Draw(spriteBatch, gameTime);

        foreach (var child in entity.GetChildren())
        {
            Render(spriteBatch, child, gameTime);
        }
    }
}