using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Renderers;

public class SnakeRenderer : RendererBase
{
    private readonly Snake _snake;
    private readonly AssetManager _assets;
    private readonly int _textureOffset;

    public SnakeRenderer(Snake snake, AssetManager assets)
    {
        _snake = snake;
        _assets = assets;
        _textureOffset = _snake is PlayerSnake ? 0 : 16 * (_snake.Id % 4 + 1);
    }

    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        RenderSnake(spriteBatch);
    }

    private void RenderSnake(SpriteBatch spriteBatch)
    {
        if (_snake.Segments.Count == 0)
            return;

        DrawSegment(spriteBatch, _snake.Head);

        if (_snake.Segments.Count > 1)
        {
            // Head & face drawing logic will cover this when length is equal to 1
            DrawSegment(spriteBatch, _snake.Tail);
        }
        
        for (var i = 0; i < _snake.Segments.Count - 1; i++)
        {
            DrawBody(spriteBatch, _snake.Segments[i]);
        }
        
        DrawHead(spriteBatch);

        if (_snake.Segments.Count > 1)
        {
            // Head & face drawing logic will cover this when length is equal to 1
            DrawTail(spriteBatch);
        }
    }

    private void DrawHead(SpriteBatch spriteBatch)
    {
        if (_snake.State == SnakeState.Alive)
        {
            spriteBatch.Draw(_assets.SnakeTexture,
                _snake.GlobalPosition + _snake.Head.Position + _snake.SegmentOrigin,
                new Rectangle(32, _textureOffset, 16, 16),
                Color.White,
                _snake.Head.Rotation,
                _snake.SegmentOrigin,
                Vector2.One,
                SpriteEffects.None,
                1f);
        }
        
        spriteBatch.Draw(_assets.SnakeTexture,
            _snake.GlobalPosition + _snake.Head.Position + _snake.SegmentOrigin,
            new Rectangle(48, _textureOffset, 16, 16),
            Color.White,
            _snake.Head.Rotation,
            _snake.SegmentOrigin,
            Vector2.One,
            SpriteEffects.FlipHorizontally,
            1f);

        if (_snake.Segments.Count == 1)
        {
            // If snake size is equal to 1 - draw tail line on same segment too
            spriteBatch.Draw(_assets.SnakeTexture,
                _snake.GlobalPosition + _snake.Head.Position + _snake.SegmentOrigin,
                new Rectangle(48, _textureOffset, 16, 16),
                Color.White,
                _snake.Head.Rotation,
                _snake.SegmentOrigin,
                Vector2.One,
                SpriteEffects.None,
                1f);
        }
    }

    private void DrawTail(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_assets.SnakeTexture,
            _snake.GlobalPosition + _snake.Tail.Position + _snake.SegmentOrigin,
            new Rectangle(48, _textureOffset, 16, 16),
            Color.White,
            _snake.Tail.Rotation,
            _snake.SegmentOrigin,
            Vector2.One,
            SpriteEffects.None,
            1f);
    }

    private void DrawBody(SpriteBatch spriteBatch, SnakeSegment segment)
    {
        if (segment.IsCorner)
            DrawCorner(spriteBatch, segment);
        else
            DrawSegment(spriteBatch, segment);
    }

    private void DrawSegment(SpriteBatch spriteBatch, SnakeSegment segment)
    {
        spriteBatch.Draw(_assets.SnakeTexture,
            _snake.GlobalPosition + segment.Position + _snake.SegmentOrigin,
            new Rectangle(16, _textureOffset, 16, 16),
            Color.White,
            segment.Rotation,
            _snake.SegmentOrigin,
            Vector2.One,
            SpriteEffects.None,
            1f);
    }

    private void DrawCorner(SpriteBatch spriteBatch, SnakeSegment segment)
    {
        spriteBatch.Draw(_assets.SnakeTexture,
            _snake.GlobalPosition + segment.Position + _snake.SegmentOrigin,
            new Rectangle(0, _textureOffset, 16, 16),
            Color.White,
            segment.Rotation,
            _snake.SegmentOrigin,
            Vector2.One,
            segment.IsClockwise ? SpriteEffects.None : SpriteEffects.FlipVertically,
            1f);
    }
}