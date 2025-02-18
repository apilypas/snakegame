using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.DesktopGL.Core.Sprites;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class BugRenderer : RendererBase
{
    private Texture2D _texture;
    private GameWorld _gameWorld;

    private BugSprite _bugSprite;

    public BugRenderer(GameWorld gameWorld)
    {
        _gameWorld = gameWorld;
    }

    public override void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("snake");

        _bugSprite = new BugSprite(_texture);
    }

    public override void Render(SpriteBatch spriteBatch, float deltaTime)
    {
        _bugSprite.Rotation = (_bugSprite.Rotation + deltaTime * 10f) % (MathF.PI * 2f); // TODO: remove

        foreach (var location in _gameWorld.BugSpawner.Locations)
        {
            _bugSprite.Location = location;
            _bugSprite.Draw(spriteBatch);
        }
    }
}