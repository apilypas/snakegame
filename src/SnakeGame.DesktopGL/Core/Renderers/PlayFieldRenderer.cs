using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.DesktopGL.Core.Sprites;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class PlayFieldRenderer : RendererBase
{
    private Texture2D _texture;
    private GameWorld _gameWorld;

    private Random _random;

    private IList<Sprite> _backgroundSprites;
    private IList<Sprite> _gridSprites;
    private IList<Sprite> _gridFrameSprites;

    public PlayFieldRenderer(GameWorld gameWorld)
    {
        _gameWorld = gameWorld;
        _random = new Random();
    }

    public override void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("snake");

        LoadBackgroundSprites();
        LoadGridSprites();
        LoadGridFrameSprites();
    }

    public override void Render(SpriteBatch spriteBatch, float deltaTime)
    {
        DrawBackground(spriteBatch);
        DrawGrid(spriteBatch);
        DrawFrame(spriteBatch);
    }

    private void DrawBackground(SpriteBatch spriteBatch)
    {
        foreach (var sprite in _backgroundSprites)
            sprite.Draw(spriteBatch);
    }

    private void DrawGrid(SpriteBatch spriteBatch)
    {
        if (!_gameWorld.HasGrid)
            return;

        foreach (var sprite in _gridSprites)
            sprite.Draw(spriteBatch);
    }

    private void DrawFrame(SpriteBatch spriteBatch)
    {
        foreach (var sprite in _gridFrameSprites)
            sprite.Draw(spriteBatch);
    }

    private void LoadBackgroundSprites()
    {
        _backgroundSprites = [];
        for (var i = 0; i < Constants.WallHeight; i++)
        {
            var y = i * Constants.SegmentSize;
            for (var j = 0; j < Constants.WallWidth; j++)
            {
                var x = j * Constants.SegmentSize;
                if (_random.Next() % 20 != 0)
                {
                    var sprite = new Sprite(
                        _texture,
                        new Rectangle(Constants.SegmentSize, Constants.SegmentSize, Constants.SegmentSize, Constants.SegmentSize));
                    sprite.Location = new Vector2(x, y);
                    _backgroundSprites.Add(sprite);
                }
                else
                {
                    var sprite = new Sprite(
                        _texture,
                        new Rectangle(Constants.SegmentSize * 2, Constants.SegmentSize, Constants.SegmentSize, Constants.SegmentSize));
                    sprite.Location = new Vector2(x, y);
                    _backgroundSprites.Add(sprite);
                }
            }
        }
    }

    private void LoadGridSprites()
    {
        _gridSprites = [];
        for (var i = 0; i < Constants.WallHeight; i++)
        {
            var y = i * Constants.SegmentSize;
            for (var j = 0; j < Constants.WallWidth; j++)
            {
                var x = j * Constants.SegmentSize;
                var sprite = new Sprite(
                    _texture,
                    new Rectangle(4 * Constants.SegmentSize, Constants.SegmentSize, Constants.SegmentSize, Constants.SegmentSize));
                sprite.Location = new Vector2(x, y);
                _gridSprites.Add(sprite);
            }
        }
    }

    private void LoadGridFrameSprites()
    {
        _gridFrameSprites = [];

        for (var i = 0; i < Constants.WallWidth; i++)
        {
            var sprite = new Sprite(
                _texture,
                new Rectangle(3 * Constants.SegmentSize, 0, Constants.SegmentSize, Constants.SegmentSize));
            sprite.Rotation = 0f;
            sprite.Location = new Vector2(i * Constants.SegmentSize, 0f);
            _gridFrameSprites.Add(sprite);
        }

        for (var i = 0; i < Constants.WallWidth; i++)
        {
            var sprite = new Sprite(
                _texture,
                new Rectangle(3 * Constants.SegmentSize, 0, Constants.SegmentSize, Constants.SegmentSize));
            sprite.Rotation = MathF.PI;
            sprite.Location = new Vector2(i * Constants.SegmentSize, (Constants.WallHeight - 1) * Constants.SegmentSize);
            _gridFrameSprites.Add(sprite);
        }

        for (var i = 0; i < Constants.WallWidth; i++)
        {
            var sprite = new Sprite(
                _texture,
                new Rectangle(3 * Constants.SegmentSize, 0, Constants.SegmentSize, Constants.SegmentSize));
            sprite.Rotation = - MathF.PI / 2f;
            sprite.Location = new Vector2(0f, i * Constants.SegmentSize);
            _gridFrameSprites.Add(sprite);
        }

        for (var i = 0; i < Constants.WallWidth; i++)
        {
            var sprite = new Sprite(
                _texture,
                new Rectangle(3 * Constants.SegmentSize, 0, Constants.SegmentSize, Constants.SegmentSize));
            sprite.Rotation = MathF.PI / 2f;
            sprite.Location = new Vector2((Constants.WallWidth - 1) * Constants.SegmentSize, i * Constants.SegmentSize);
            _gridFrameSprites.Add(sprite);
        }
    }
}