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

    private SnakeBorderSprite _snakeBorderSprite;
    private SnakeColorSprite _snakeColorSprite;
    private SnakeFaceSprite _snakeFaceSprite;

    public SnakeRenderer(GameWorld gameWorld)
    {
        _gameWorld = gameWorld;
    }

    public override void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("snake");

        _snakeBorderSprite = new SnakeBorderSprite(_texture);
        _snakeColorSprite = new SnakeColorSprite(_texture);
        _snakeFaceSprite = new SnakeFaceSprite(_texture);
    }

    public void Render(SpriteBatch spriteBatch)
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
        _snakeColorSprite.Location = segment.Location;
        _snakeColorSprite.Draw(spriteBatch);

        _snakeBorderSprite.Location = new Vector2(segment.Location.X, segment.Location.Y);
        _snakeBorderSprite.Rotation = segment.GetRotation() - MathF.PI / 2f;
        _snakeBorderSprite.Draw(spriteBatch);

        _snakeBorderSprite.Location = new Vector2(segment.Location.X, segment.Location.Y);
        _snakeBorderSprite.Rotation = next.GetRotation() + MathF.PI / 2f;
        _snakeBorderSprite.Draw(spriteBatch);
    }
}