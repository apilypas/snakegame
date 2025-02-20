using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.DesktopGL.Core.Entities;
using SnakeGame.DesktopGL.Core.Sprites;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class SnakeRenderer : RendererBase
{
    private Texture2D _texture;

    private GameWorld _gameWorld;

    private TextureSprite _snakeBorderSprite;
    private TextureSprite _snakeColorSprite;
    private TextureSprite _snakeFaceSprite;
    private TextureSprite _snakeCornerSprite;

    public Vector2 Offset { get; set; }

    public SnakeRenderer(GameWorld gameWorld)
    {
        _gameWorld = gameWorld;
    }

    public override void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("snake");

        _snakeBorderSprite = new TextureSprite(
            _texture,
            new Rectangle(0, 0, Constants.SegmentSize, Constants.SegmentSize));

        _snakeColorSprite = new TextureSprite(
            _texture,
            new Rectangle(4 * Constants.SegmentSize, 0, Constants.SegmentSize, Constants.SegmentSize));
        
        _snakeFaceSprite = new TextureSprite(
            _texture,
            new Rectangle(Constants.SegmentSize, 0, Constants.SegmentSize, Constants.SegmentSize));

        _snakeCornerSprite = new TextureSprite(
            _texture,
            new Rectangle(2 * Constants.SegmentSize, 0, Constants.SegmentSize, Constants.SegmentSize));
    }

    public override void Render(SpriteBatch spriteBatch, float deltaTime)
    {
        DrawSegment(spriteBatch, _gameWorld.Snake.Head);
        DrawSegment(spriteBatch, _gameWorld.Snake.Tail);

        for (var i = 0; i < _gameWorld.Snake.Segments.Count - 1; i++)
        {
            var segment = _gameWorld.Snake.Segments[i];
            var nextSegment = _gameWorld.Snake.Segments[i + 1];
            DrawBody(spriteBatch, segment, nextSegment);
        }

        DrawHead(spriteBatch, _gameWorld.Snake.Head);
        DrawTail(spriteBatch, _gameWorld.Snake.Tail);
    }

    private void DrawHead(SpriteBatch spriteBatch, SnakeSegment segment)
    {
        var rotation = segment.GetRotation();

        _snakeBorderSprite.Location = segment.Location + Offset;
        _snakeBorderSprite.Rotation = rotation + MathF.PI / 2f;
        _snakeBorderSprite.Draw(spriteBatch);

        _snakeFaceSprite.Location = segment.Location + Offset;
        _snakeFaceSprite.Rotation = rotation;
        _snakeFaceSprite.Draw(spriteBatch);
    }

    private void DrawTail(SpriteBatch spriteBatch, SnakeSegment segment)
    {
        var rotation = segment.GetRotation();

        _snakeBorderSprite.Location = segment.Location + Offset;
        _snakeBorderSprite.Rotation = rotation - MathF.PI / 2f;
        _snakeBorderSprite.Draw(spriteBatch);
    }

    private void DrawBody(SpriteBatch spriteBatch, SnakeSegment segment, SnakeSegment next)
    {
        if (next.Direction != segment.Direction)
            DrawCorner(spriteBatch, next, segment);
        else
            DrawSegment(spriteBatch, segment);
    }

    private void DrawSegment(SpriteBatch spriteBatch, SnakeSegment segment)
    {
        var rotation = segment.GetRotation();

        _snakeColorSprite.Location = segment.Location + Offset;
        _snakeColorSprite.Draw(spriteBatch);

        _snakeBorderSprite.Location = segment.Location + Offset;
        _snakeBorderSprite.Rotation = rotation;
        _snakeBorderSprite.Draw(spriteBatch);

        _snakeBorderSprite.Location = segment.Location + Offset;
        _snakeBorderSprite.Rotation = rotation + MathF.PI;
        _snakeBorderSprite.Draw(spriteBatch);
    }

    private void DrawCorner(SpriteBatch spriteBatch, SnakeSegment next, SnakeSegment segment)
    {
        // Draw segment color

        _snakeColorSprite.Location = segment.Location + Offset;
        _snakeColorSprite.Draw(spriteBatch);

        // Draw corner lines

        _snakeBorderSprite.Location = segment.Location + Offset;
        _snakeBorderSprite.Rotation = segment.GetRotation() - MathF.PI / 2f;
        _snakeBorderSprite.Draw(spriteBatch);

        _snakeBorderSprite.Location = segment.Location + Offset;
        _snakeBorderSprite.Rotation = next.GetRotation() + MathF.PI / 2f;
        _snakeBorderSprite.Draw(spriteBatch);

        // Draw corner dots

        _snakeCornerSprite.Location = segment.Location + Offset;
        _snakeCornerSprite.Rotation = 0f;
        _snakeCornerSprite.Draw(spriteBatch);

        _snakeCornerSprite.Location = segment.Location + Offset;
        _snakeCornerSprite.Rotation = MathF.PI / 2f;
        _snakeCornerSprite.Draw(spriteBatch);

        _snakeCornerSprite.Location = segment.Location + Offset;
        _snakeCornerSprite.Rotation = -MathF.PI / 2f;
        _snakeCornerSprite.Draw(spriteBatch);

        _snakeCornerSprite.Location = segment.Location + Offset;
        _snakeCornerSprite.Rotation = MathF.PI;
        _snakeCornerSprite.Draw(spriteBatch);
    }
}