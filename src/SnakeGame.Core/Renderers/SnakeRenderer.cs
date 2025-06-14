using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Enums;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Renderers;

public class SnakeRenderer
{
    private readonly Texture2D _texture;
    private readonly Snake _snake;
    private readonly Rectangle _headRectangle;
    private readonly Rectangle _faceRectangle;
    private readonly Rectangle _tailRectangle;
    private readonly Rectangle _segmentRectangle;
    private readonly Rectangle _cornerRectangle;

    public SnakeRenderer(Snake snake, AssetManager assets)
    {
        _snake = snake;
        _texture = assets.SnakeTexture;
        
        var textureOffset = _snake is PlayerSnake ? 0 : 16;
        
        _headRectangle = new Rectangle(48, textureOffset, 16, 16);
        _faceRectangle = new Rectangle(32, textureOffset, 16, 16);
        _tailRectangle = new Rectangle(48, textureOffset, 16, 16);
        _segmentRectangle = new Rectangle(16, textureOffset, 16, 16);
        _cornerRectangle = new Rectangle(0, textureOffset, 16, 16);
    }

    public void Render(SpriteBatch spriteBatch)
    {
        DrawSnake(spriteBatch, _snake);
    }

    private void DrawSnake(SpriteBatch spriteBatch, Snake snake)
    {
        if (snake.Segments.Count == 0)
            return;

        DrawSegment(spriteBatch, snake.Head, snake);

        if (snake.Segments.Count > 1)
        {
            // Head & face drawing logic will cover this when length is equal to 1
            DrawSegment(spriteBatch, snake.Tail, snake);
        }
        
        for (var i = 0; i < snake.Segments.Count - 1; i++)
        {
            DrawBody(spriteBatch, snake.Segments[i], snake);
        }
        
        DrawHead(spriteBatch, snake);

        if (snake.Segments.Count > 1)
        {
            // Head & face drawing logic will cover this when length is equal to 1
            DrawTail(spriteBatch, snake);
        }
    }

    private void DrawHead(SpriteBatch spriteBatch, Snake snake)
    {
        if (snake.State == SnakeState.Alive)
        {
            spriteBatch.Draw(_texture,
                snake.GlobalPosition + snake.Head.Position + Globals.SnakeSegmentOrigin,
                _faceRectangle,
                Color.White,
                snake.Head.Rotation,
                Globals.SnakeSegmentOrigin,
                Vector2.One,
                SpriteEffects.None,
                1f);
        }
        
        spriteBatch.Draw(_texture,
            snake.GlobalPosition + snake.Head.Position + Globals.SnakeSegmentOrigin,
            _headRectangle,
            snake.Head.Color,
            snake.Head.Rotation,
            Globals.SnakeSegmentOrigin,
            Vector2.One,
            SpriteEffects.FlipHorizontally,
            1f);

        if (snake.Segments.Count == 1)
        {
            // If snake size is equal to 1 - draw tail line on same segment too
            spriteBatch.Draw(_texture,
                snake.GlobalPosition + snake.Head.Position + Globals.SnakeSegmentOrigin,
                _tailRectangle,
                snake.Head.Color,
                snake.Head.Rotation,
                Globals.SnakeSegmentOrigin,
                Vector2.One,
                SpriteEffects.None,
                1f);
        }
    }

    private void DrawTail(SpriteBatch spriteBatch, Snake snake)
    {
        spriteBatch.Draw(_texture,
            snake.GlobalPosition + snake.Tail.Position + Globals.SnakeSegmentOrigin,
            _tailRectangle,
            snake.Tail.Color,
            snake.Tail.Rotation,
            Globals.SnakeSegmentOrigin,
            Vector2.One,
            SpriteEffects.None,
            1f);
    }

    private void DrawBody(SpriteBatch spriteBatch, SnakeSegment segment, Snake snake)
    {
        if (segment.IsCorner)
            DrawCorner(spriteBatch, segment, snake);
        else
            DrawSegment(spriteBatch, segment, snake);
    }

    private void DrawSegment(SpriteBatch spriteBatch, SnakeSegment segment, Snake snake)
    {
        spriteBatch.Draw(_texture,
            snake.GlobalPosition + segment.Position + Globals.SnakeSegmentOrigin,
            _segmentRectangle,
            segment.Color,
            segment.Rotation,
            Globals.SnakeSegmentOrigin,
            Vector2.One,
            SpriteEffects.None,
            1f);
    }

    private void DrawCorner(SpriteBatch spriteBatch, SnakeSegment segment, Snake snake)
    {
        spriteBatch.Draw(_texture,
            snake.GlobalPosition + segment.Position + Globals.SnakeSegmentOrigin,
            _cornerRectangle,
            segment.Color,
            segment.Rotation,
            Globals.SnakeSegmentOrigin,
            Vector2.One,
            segment.IsClockwise ? SpriteEffects.None : SpriteEffects.FlipVertically,
            1f);
    }
}