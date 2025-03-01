using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.DesktopGL.Core.Sprites;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class SpeedBugRenderer : RendererBase
{
    private GameWorld _gameWorld;

    private TextureSprite _speedBugSprite;

    public SpeedBugRenderer(GameWorld gameWorld)
    {
        _gameWorld = gameWorld;
    }

    public override void LoadContent(ContentManager content)
    {
        _speedBugSprite = TextureSprite
            .Create(new Rectangle(0, 60, 20, 20))
            .Load(content, "snake");
    }

    public override void Render(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, float deltaTime)
    {
        Offset = PlayFieldRenderer.GetPlayFieldOffset(graphicsDevice);

        foreach (var speedBug in _gameWorld.EntitySpawner.SpeedBugs)
            Draw(spriteBatch, speedBug, _speedBugSprite);
    }
}