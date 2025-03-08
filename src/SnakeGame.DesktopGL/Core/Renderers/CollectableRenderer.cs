using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
using SnakeGame.DesktopGL.Core.Entities;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class CollectableRenderer(GameWorld gameWorld) : RendererBase
{
    private Sprite _diamondSprite;
    private Sprite _snakePartSprite;
    private Sprite _speedBoostSprite;
    private Texture2D _texture;
    private Vector2 _offset;

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

        _offset = RendererUtils.GetPlayFieldOffset(graphicsDevice);
    }

    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        foreach (var collectable in gameWorld.Collectables)
        {
            if (collectable.Type == CollectableType.Diamond)
            {
                _diamondSprite.Draw(
                    spriteBatch,
                    collectable.Location + _offset,
                    collectable.Rotation,
                    Vector2.One);
            }

            if (collectable.Type == CollectableType.SnakePart)
            {
                _snakePartSprite.Draw(
                    spriteBatch,
                    collectable.Location + _offset,
                    collectable.Rotation,
                    Vector2.One);
            }

            if (collectable.Type == CollectableType.SpeedBoost)
            {
                _speedBoostSprite.Draw(
                    spriteBatch,
                    collectable.Location + _offset,
                    collectable.Rotation,
                    Vector2.One);
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
    }
}