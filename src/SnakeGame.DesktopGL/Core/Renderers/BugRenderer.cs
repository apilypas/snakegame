using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.DesktopGL.Core.Sprites;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class BugRenderer : RendererBase
{
    private GameWorld _gameWorld;

    private TextureSprite _bugSprite;

    public BugRenderer(GameWorld gameWorld)
    {
        _gameWorld = gameWorld;
    }

    public override void LoadContent(ContentManager content)
    {
        _bugSprite = TextureSprite
            .Create(new Rectangle(40, 60, 20, 20))
            .Load(content, "snake");
    }

    public override void Render(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, float deltaTime)
    {
        Offset = PlayFieldRenderer.GetPlayFieldOffset(graphicsDevice);

        foreach (var bug in _gameWorld.EntitySpawner.Bugs)
            Draw(spriteBatch, bug, _bugSprite);
    }
}