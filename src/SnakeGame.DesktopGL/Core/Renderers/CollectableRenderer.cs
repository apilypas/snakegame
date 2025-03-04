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

    public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _texture = content.Load<Texture2D>("snake");
        
        _diamondSprite = new Sprite(
            new Texture2DRegion(
                _texture, 
                new Rectangle(40, 60, 20, 20)
                )
            );
        
        _snakePartSprite = new Sprite(
            new Texture2DRegion(
                _texture, 
                new Rectangle(20, 60, 20, 20)
                )
            );
        
        _speedBoostSprite = new Sprite(
            new Texture2DRegion(
                _texture, 
                new Rectangle(0, 60, 20, 20)
                )
            );
    }

    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        var offset = PlayFieldRenderer.GetPlayFieldOffset(spriteBatch.GraphicsDevice);

        foreach (var collectable in gameWorld.Collectables)
        {
            if (collectable.Type == CollectableType.Diamond)
            {
                _diamondSprite.Draw(spriteBatch, collectable.Location + offset, collectable.Rotation, Vector2.One);
            }

            if (collectable.Type == CollectableType.SnakePart)
            {
                _snakePartSprite.Draw(spriteBatch, collectable.Location + offset, collectable.Rotation, Vector2.One);
            }

            if (collectable.Type == CollectableType.SpeedBoost)
            {
                _speedBoostSprite.Draw(spriteBatch, collectable.Location + offset, collectable.Rotation, Vector2.One);
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
    }
}