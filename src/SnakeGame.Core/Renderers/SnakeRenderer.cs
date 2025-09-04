using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Services;

namespace SnakeGame.Core.Renderers;

public class SnakeRenderer
{
    private readonly Texture2D _texture;
    
    private Rectangle _headRectangle;
    private Rectangle _faceRectangle;
    private Rectangle _tailRectangle;
    private Rectangle _segmentRectangle;
    private Rectangle _cornerRectangle;

    public SnakeRenderer(ContentManager contents)
    {
        _texture = contents.SnakeTexture;
    }

    public void Render(SpriteBatch spriteBatch, SnakeComponent snake, bool isPlayer)
    {
        var textureOffset = isPlayer ? 0 : 16;
        
        _headRectangle = new Rectangle(48, textureOffset, 16, 16);
        _faceRectangle = new Rectangle(32, textureOffset, 16, 16);
        _tailRectangle = new Rectangle(48, textureOffset, 16, 16);
        _segmentRectangle = new Rectangle(16, textureOffset, 16, 16);
        _cornerRectangle = new Rectangle(0, textureOffset, 16, 16);
        
        DrawSnake(spriteBatch, snake);
    }

    private void DrawSnake(SpriteBatch spriteBatch, SnakeComponent snake)
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

    private void DrawHead(SpriteBatch spriteBatch, SnakeComponent snake)
    {
        if (snake.IsAlive)
        {
            spriteBatch.Draw(_texture,
                snake.Head.Position + Globals.SnakeSegmentOrigin,
                _faceRectangle,
                Color.White,
                snake.Head.Rotation,
                Globals.SnakeSegmentOrigin,
                Vector2.One,
                SpriteEffects.None,
                1f);
        }
        
        spriteBatch.Draw(_texture,
            snake.Head.Position + Globals.SnakeSegmentOrigin,
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
                snake.Head.Position + Globals.SnakeSegmentOrigin,
                _tailRectangle,
                snake.Head.Color,
                snake.Head.Rotation,
                Globals.SnakeSegmentOrigin,
                Vector2.One,
                SpriteEffects.None,
                1f);
        }
    }

    private void DrawTail(SpriteBatch spriteBatch, SnakeComponent snake)
    {
        spriteBatch.Draw(_texture,
            snake.Tail.Position + Globals.SnakeSegmentOrigin,
            _tailRectangle,
            snake.Tail.Color,
            snake.Tail.Rotation,
            Globals.SnakeSegmentOrigin,
            Vector2.One,
            SpriteEffects.None,
            1f);
    }

    private void DrawBody(SpriteBatch spriteBatch, SnakeSegment segment, SnakeComponent snake)
    {
        if (segment.IsCorner)
            DrawCorner(spriteBatch, segment, snake);
        else
            DrawSegment(spriteBatch, segment, snake);
    }

    private void DrawSegment(SpriteBatch spriteBatch, SnakeSegment segment, SnakeComponent snake)
    {
        spriteBatch.Draw(_texture,
            segment.Position + Globals.SnakeSegmentOrigin,
            _segmentRectangle,
            segment.Color,
            segment.Rotation,
            Globals.SnakeSegmentOrigin,
            Vector2.One,
            SpriteEffects.None,
            1f);
    }

    private void DrawCorner(SpriteBatch spriteBatch, SnakeSegment segment, SnakeComponent snake)
    {
        spriteBatch.Draw(_texture,
            segment.Position + Globals.SnakeSegmentOrigin,
            _cornerRectangle,
            segment.Color,
            segment.Rotation,
            Globals.SnakeSegmentOrigin,
            Vector2.One,
            segment.IsClockwise ? SpriteEffects.None : SpriteEffects.FlipVertically,
            1f);
    }
}