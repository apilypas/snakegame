using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
using SnakeGame.Core.Renderers.Animations;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Renderers;

public class CollectableRenderer(GameManager gameManager) : RendererBase
{
    private readonly JumpingAnimation _jumpingAnimation = new();

    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        _jumpingAnimation.Update(gameTime);
        
        foreach (var collectable in gameManager.Collectables)
        {
            collectable.Sprite.Draw(
                spriteBatch,
                collectable.Position + Globals.PlayFieldOffset + _jumpingAnimation.AdjustVector,
                collectable.Rotation,
                Vector2.One);
        }
    }
}