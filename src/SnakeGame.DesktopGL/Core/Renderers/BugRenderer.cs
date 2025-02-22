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
        _bugSprite = new TextureSprite(new Rectangle(0, 20, 20, 20))
            .Load(content, "snake");
    }

    public override void Render(SpriteBatch spriteBatch, float deltaTime)
    {
        foreach (var bug in _gameWorld.BugSpawner.Bugs)
            Draw(spriteBatch, bug, _bugSprite);
    }
}