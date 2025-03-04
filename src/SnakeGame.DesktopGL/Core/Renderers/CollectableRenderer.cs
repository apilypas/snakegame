using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.DesktopGL.Core.Entities;
using SnakeGame.DesktopGL.Core.Sprites;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class CollectableRenderer : RendererBase
{
    private readonly GameWorld _gameWorld;

    private TextureSprite _diamondSprite;
    private TextureSprite _snakePartSprite;
    private TextureSprite _speedBoostSprite;

    public CollectableRenderer(GameWorld gameWorld)
    {
        _gameWorld = gameWorld;
    }

    public override void LoadContent(ContentManager content)
    {
        _diamondSprite = TextureSprite
            .Create(new Rectangle(40, 60, 20, 20))
            .Load(content, "snake");
        
        _snakePartSprite = TextureSprite
            .Create(new Rectangle(20, 60, 20, 20))
            .Load(content, "snake");
        
        _speedBoostSprite = TextureSprite
            .Create(new Rectangle(0, 60, 20, 20))
            .Load(content, "snake");
    }

    public override void Render(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, float deltaTime)
    {
        Offset = PlayFieldRenderer.GetPlayFieldOffset(graphicsDevice);

        foreach (var collectable in _gameWorld.Collectables)
        {
            if (collectable.Type == CollectableType.Diamond)
                Draw(spriteBatch, collectable, _diamondSprite);
            
            if (collectable.Type == CollectableType.SnakePart)
                Draw(spriteBatch, collectable, _snakePartSprite);
            
            if (collectable.Type == CollectableType.SpeedBoost)
                Draw(spriteBatch, collectable, _speedBoostSprite);
        }
    }
}