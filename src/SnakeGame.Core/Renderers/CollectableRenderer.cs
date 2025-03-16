using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Renderers.Animations;

namespace SnakeGame.Core.Renderers;

public class CollectableRenderer(GameWorld gameWorld) : RendererBase
{
    private Sprite _diamondSprite;
    private Sprite _snakePartSprite;
    private Sprite _speedBoostSprite;
    private Sprite _clockSprite;
    private Texture2D _texture;
    private readonly JumpingAnimation _jumpingAnimation = new();
    
    public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _texture = content.Load<Texture2D>("Collectables");
        
        _diamondSprite = new Sprite(
            new Texture2DRegion(
                _texture, 
                new Rectangle(0, 32, 16, 16)
                )
            );
        
        _snakePartSprite = new Sprite(
            new Texture2DRegion(
                _texture, 
                new Rectangle(0, 16, 16, 16)
                )
            );
        
        _speedBoostSprite = new Sprite(
            new Texture2DRegion(
                _texture, 
                new Rectangle(0, 0, 16, 16)
                )
            );
        
        _clockSprite = new Sprite(
            new Texture2DRegion(
                _texture, 
                new Rectangle(16, 0, 16, 16)
                )
            );
    }

    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        foreach (var collectable in gameWorld.Collectables)
        {
            if (collectable.Type == CollectableType.Diamond)
            {
                _diamondSprite.Draw(
                    spriteBatch,
                    collectable.Location + Globals.PlayFieldOffset + _jumpingAnimation.AdjustVector,
                    collectable.Rotation,
                    Vector2.One);
            }

            if (collectable.Type == CollectableType.SnakePart)
            {
                _snakePartSprite.Draw(
                    spriteBatch,
                    collectable.Location + Globals.PlayFieldOffset + _jumpingAnimation.AdjustVector,
                    collectable.Rotation,
                    Vector2.One);
            }

            if (collectable.Type == CollectableType.SpeedBoost)
            {
                _speedBoostSprite.Draw(
                    spriteBatch,
                    collectable.Location + Globals.PlayFieldOffset + _jumpingAnimation.AdjustVector,
                    collectable.Rotation,
                    Vector2.One);
            }
            
            if (collectable.Type == CollectableType.Clock)
            {
                _clockSprite.Draw(
                    spriteBatch,
                    collectable.Location + Globals.PlayFieldOffset + _jumpingAnimation.AdjustVector,
                    collectable.Rotation,
                    Vector2.One);
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        _jumpingAnimation.Update(gameTime);
    }
}