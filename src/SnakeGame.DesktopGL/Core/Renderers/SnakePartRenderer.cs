using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.DesktopGL.Core.Sprites;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class SnakePartRenderer : RendererBase
{
    private GameWorld _gameWorld;

    private TextureSprite _snakePartSprite;

    public SnakePartRenderer(GameWorld gameWorld)
    {
        _gameWorld = gameWorld;
    }

    public override void LoadContent(ContentManager content)
    {
        _snakePartSprite = TextureSprite
            .Create(new Rectangle(20, 60, 20, 20))
            .Load(content, "snake");
    }

    public override void Render(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, float deltaTime)
    {
        Offset = PlayFieldRenderer.GetPlayFieldOffset(graphicsDevice);

        foreach (var snakePart in _gameWorld.EntitySpawner.SnakeParts)
            Draw(spriteBatch, snakePart, _snakePartSprite);
    }
}