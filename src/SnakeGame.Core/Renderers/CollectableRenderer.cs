using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
using SnakeGame.Core.Renderers.Animations;

namespace SnakeGame.Core.Renderers;

public class CollectableRenderer(GameWorld gameWorld) : RendererBase
{
    private readonly JumpingAnimation _jumpingAnimation = new();

    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        _jumpingAnimation.Update(gameTime);
        
        foreach (var collectable in gameWorld.Collectables)
        {
            collectable.Sprite.Draw(
                spriteBatch,
                collectable.Location + Globals.PlayFieldOffset + _jumpingAnimation.AdjustVector,
                collectable.Rotation,
                Vector2.One);
        }
    }
}