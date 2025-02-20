using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.DesktopGL.Core.Sprites;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class BugRenderer : RendererBase
{
    private Texture2D _texture;
    private GameWorld _gameWorld;

    private TextureSprite _bugSprite;

    public Vector2 Offset { get; set; }

    public BugRenderer(GameWorld gameWorld)
    {
        _gameWorld = gameWorld;
    }

    public override void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("snake");

        _bugSprite = new TextureSprite(
            _texture,
            new Rectangle(0, Constants.SegmentSize, Constants.SegmentSize, Constants.SegmentSize));
    }

    public override void Render(SpriteBatch spriteBatch, float deltaTime)
    {
        _bugSprite.Rotation = (_bugSprite.Rotation + deltaTime * 10f) % (MathF.PI * 2f); // TODO: remove

        foreach (var location in _gameWorld.BugSpawner.Locations)
        {
            _bugSprite.Location = location + Offset;
            _bugSprite.Draw(spriteBatch);
        }
    }
}