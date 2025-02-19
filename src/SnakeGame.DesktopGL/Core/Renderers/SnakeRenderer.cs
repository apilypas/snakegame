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

    private Sprite _snakeBorderSprite;
    private Sprite _snakeColorSprite;
    private Sprite _snakeFaceSprite;
    private Sprite _snakeCornerSprite;

    public SnakeRenderer(GameWorld gameWorld)
    {
        _gameWorld = gameWorld;
    }

    public override void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("snake");

        _snakeBorderSprite = new Sprite(
            _texture,
            new Rectangle(0, 0, Constants.SegmentSize, Constants.SegmentSize));

        _snakeColorSprite = new Sprite(
            _texture,
            new Rectangle(4 * Constants.SegmentSize, 0, Constants.SegmentSize, Constants.SegmentSize));
        
        _snakeFaceSprite = new Sprite(
            _texture,
            new Rectangle(Constants.SegmentSize, 0, Constants.SegmentSize, Constants.SegmentSize));

        _snakeCornerSprite = new Sprite(
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

        _snakeBorderSprite.Location = segment.Location;
        _snakeBorderSprite.Rotation = rotation + MathF.PI / 2f;
        _snakeBorderSprite.Draw(spriteBatch);

        _snakeFaceSprite.Location = segment.Location;
        _snakeFaceSprite.Rotation = rotation;
        _snakeFaceSprite.Draw(spriteBatch);
    }

    private void DrawTail(SpriteBatch spriteBatch, SnakeSegment segment)
    {
        var rotation = segment.GetRotation();

        _snakeBorderSprite.Location = segment.Location;
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

        _snakeColorSprite.Location = segment.Location;
        _snakeColorSprite.Draw(spriteBatch);

        _snakeBorderSprite.Location = segment.Location;
        _snakeBorderSprite.Rotation = rotation;
        _snakeBorderSprite.Draw(spriteBatch);

        _snakeBorderSprite.Location = segment.Location;
        _snakeBorderSprite.Rotation = rotation + MathF.PI;
        _snakeBorderSprite.Draw(spriteBatch);
    }

    private void DrawCorner(SpriteBatch spriteBatch, SnakeSegment next, SnakeSegment segment)
    {
        // Draw segment color

        _snakeColorSprite.Location = segment.Location;
        _snakeColorSprite.Draw(spriteBatch);

        // Draw corner lines

        _snakeBorderSprite.Location = segment.Location;
        _snakeBorderSprite.Rotation = segment.GetRotation() - MathF.PI / 2f;
        _snakeBorderSprite.Draw(spriteBatch);

        _snakeBorderSprite.Location = segment.Location;
        _snakeBorderSprite.Rotation = next.GetRotation() + MathF.PI / 2f;
        _snakeBorderSprite.Draw(spriteBatch);

        // Draw corner dots

        _snakeCornerSprite.Location = segment.Location;
        _snakeCornerSprite.Rotation = 0f;
        _snakeCornerSprite.Draw(spriteBatch);

        _snakeCornerSprite.Location = segment.Location;
        _snakeCornerSprite.Rotation = MathF.PI / 2f;
        _snakeCornerSprite.Draw(spriteBatch);

        _snakeCornerSprite.Location = segment.Location;
        _snakeCornerSprite.Rotation = -MathF.PI / 2f;
        _snakeCornerSprite.Draw(spriteBatch);

        _snakeCornerSprite.Location = segment.Location;
        _snakeCornerSprite.Rotation = MathF.PI;
        _snakeCornerSprite.Draw(spriteBatch);
    }
}