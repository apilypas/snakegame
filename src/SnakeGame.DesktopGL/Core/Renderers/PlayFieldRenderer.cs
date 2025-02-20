using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.DesktopGL.Core.Sprites;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class PlayFieldRenderer : RendererBase
{
    private Texture2D _texture;
    private GameWorld _gameWorld;

    private TextureSprite _backgroundSprite1;
    private TextureSprite _backgroundSprite2;
    private TextureSprite _gridSprite;
    private TextureSprite _frameSprite;
    private bool[] _hasGrass;

    public Vector2 Offset { get; set; }

    public PlayFieldRenderer(GameWorld gameWorld)
    {
        _gameWorld = gameWorld;
        _hasGrass = new bool[Constants.WallWidth * Constants.WallHeight];
    }

    public override void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("snake");

        LoadSprites();
        RandomizeBackground();
    }

    public override void Render(SpriteBatch spriteBatch, float deltaTime)
    {
        RenderBacground(spriteBatch);
        RenderGrid(spriteBatch);
        RenderFrame(spriteBatch);
    }

    private void RenderBacground(SpriteBatch spriteBatch)
    {
        for (var i = 0; i < Constants.WallHeight; i++)
        {
            var y = i * Constants.SegmentSize;
            for (var j = 0; j < Constants.WallWidth; j++)
            {
                var x = j * Constants.SegmentSize;

                var location = new Vector2(x, y) + Offset;

                if (_hasGrass[i * j])
                {
                    _backgroundSprite1.Location = location;
                    _backgroundSprite1.Draw(spriteBatch);
                }
                else
                {
                    _backgroundSprite2.Location = location;
                    _backgroundSprite2.Draw(spriteBatch);
                }
            }
        }
    }

    private void RenderGrid(SpriteBatch spriteBatch)
    {
        if (!_gameWorld.HasGrid)
            return;

        for (var i = 0; i < Constants.WallHeight; i++)
        {
            var y = i * Constants.SegmentSize;
            for (var j = 0; j < Constants.WallWidth; j++)
            {
                var x = j * Constants.SegmentSize;

                var location = new Vector2(x, y) + Offset;

                _gridSprite.Location = location;
                _gridSprite.Draw(spriteBatch);
            }
        }
    }

    private void RenderFrame(SpriteBatch spriteBatch)
    {
        for (var i = 0; i < Constants.WallWidth; i++)
        {
            _frameSprite.Rotation = 0f;
            _frameSprite.Location = new Vector2(i * Constants.SegmentSize, 0) + Offset;
            _frameSprite.Draw(spriteBatch);

            _frameSprite.Rotation = MathF.PI;
            _frameSprite.Location = new Vector2(i * Constants.SegmentSize, (Constants.WallHeight - 1) * Constants.SegmentSize) + Offset;
            _frameSprite.Draw(spriteBatch);
        }

        for (var i = 0; i < Constants.WallHeight; i++)
        {
            _frameSprite.Rotation = -MathF.PI / 2f;
            _frameSprite.Location = new Vector2(0, i * Constants.SegmentSize) + Offset;
            _frameSprite.Draw(spriteBatch);

            _frameSprite.Rotation = MathF.PI / 2f;
            _frameSprite.Location = new Vector2((Constants.WallWidth - 1) * Constants.SegmentSize, i * Constants.SegmentSize) + Offset;
            _frameSprite.Draw(spriteBatch);
        }
    }

    private void RandomizeBackground()
    {
        var random = new Random();

        for (var i = 0; i < Constants.WallHeight; i++)
        {
            for (var j = 0; j < Constants.WallWidth; j++)
            {
                _hasGrass[i * j] = random.Next() % 20 != 0;
            }
        }
    }

    private void LoadSprites()
    {
        _backgroundSprite1 = new TextureSprite(
            _texture,
            new Rectangle(Constants.SegmentSize, Constants.SegmentSize, Constants.SegmentSize, Constants.SegmentSize)
            );

        _backgroundSprite2 = new TextureSprite(
            _texture,
            new Rectangle(Constants.SegmentSize * 2, Constants.SegmentSize, Constants.SegmentSize, Constants.SegmentSize)
            );
        
        _gridSprite = new TextureSprite(
            _texture,
            new Rectangle(4 * Constants.SegmentSize, Constants.SegmentSize, Constants.SegmentSize, Constants.SegmentSize)
            );
        
        _frameSprite = new TextureSprite(
            _texture,
            new Rectangle(3 * Constants.SegmentSize, 0, Constants.SegmentSize, Constants.SegmentSize)
            );
    }
}