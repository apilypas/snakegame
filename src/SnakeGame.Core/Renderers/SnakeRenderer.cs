using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Renderers;

public class SnakeRenderer
{
    private readonly int _textureOffset;
    private readonly Texture2D _texture;
    private readonly Snake _snake;
    
    public SnakeRenderer(Snake snake, AssetManager assets)
    {
        _snake = snake;
        _texture = assets.SnakeTexture;
        _textureOffset = _snake is PlayerSnake ? 0 : 16;
    }

    public void Render(SpriteBatch spriteBatch)
    {
        DrawSnake(spriteBatch, _texture, _textureOffset, _snake);
    }

    private static void DrawSnake(SpriteBatch spriteBatch, Texture2D texture, int textureOffset, Snake snake)
    {
        if (snake.Segments.Count == 0)
            return;

        DrawSegment(spriteBatch, texture, textureOffset, snake.Head, snake);

        if (snake.Segments.Count > 1)
        {
            // Head & face drawing logic will cover this when length is equal to 1
            DrawSegment(spriteBatch, texture, textureOffset, snake.Tail, snake);
        }
        
        for (var i = 0; i < snake.Segments.Count - 1; i++)
        {
            DrawBody(spriteBatch, texture, textureOffset, snake.Segments[i], snake);
        }
        
        DrawHead(spriteBatch, texture, textureOffset, snake);

        if (snake.Segments.Count > 1)
        {
            // Head & face drawing logic will cover this when length is equal to 1
            DrawTail(spriteBatch, texture, textureOffset, snake);
        }
    }

    private static void DrawHead(SpriteBatch spriteBatch, Texture2D texture, int textureOffset, Snake snake)
    {
        if (snake.State == SnakeState.Alive)
        {
            spriteBatch.Draw(texture,
                snake.GlobalPosition + snake.Head.Position + Globals.SnakeSegmentOrigin,
                new Rectangle(32, textureOffset, 16, 16),
                snake.Head.Color,
                snake.Head.Rotation,
                Globals.SnakeSegmentOrigin,
                Vector2.One,
                SpriteEffects.None,
                1f);
        }
        
        spriteBatch.Draw(texture,
            snake.GlobalPosition + snake.Head.Position + Globals.SnakeSegmentOrigin,
            new Rectangle(48, textureOffset, 16, 16),
            snake.Head.Color,
            snake.Head.Rotation,
            Globals.SnakeSegmentOrigin,
            Vector2.One,
            SpriteEffects.FlipHorizontally,
            1f);

        if (snake.Segments.Count == 1)
        {
            // If snake size is equal to 1 - draw tail line on same segment too
            spriteBatch.Draw(texture,
                snake.GlobalPosition + snake.Head.Position + Globals.SnakeSegmentOrigin,
                new Rectangle(48, textureOffset, 16, 16),
                snake.Head.Color,
                snake.Head.Rotation,
                Globals.SnakeSegmentOrigin,
                Vector2.One,
                SpriteEffects.None,
                1f);
        }
    }

    private static void DrawTail(SpriteBatch spriteBatch, Texture2D texture, int textureOffset, Snake snake)
    {
        spriteBatch.Draw(texture,
            snake.GlobalPosition + snake.Tail.Position + Globals.SnakeSegmentOrigin,
            new Rectangle(48, textureOffset, 16, 16),
            snake.Tail.Color,
            snake.Tail.Rotation,
            Globals.SnakeSegmentOrigin,
            Vector2.One,
            SpriteEffects.None,
            1f);
    }

    private static void DrawBody(SpriteBatch spriteBatch, Texture2D texture, int textureOffset, SnakeSegment segment, Snake snake)
    {
        if (segment.IsCorner)
            DrawCorner(spriteBatch, texture, textureOffset, segment, snake);
        else
            DrawSegment(spriteBatch, texture, textureOffset, segment, snake);
    }

    private static void DrawSegment(SpriteBatch spriteBatch, Texture2D texture, int textureOffset, SnakeSegment segment, Snake snake)
    {
        spriteBatch.Draw(texture,
            snake.GlobalPosition + segment.Position + Globals.SnakeSegmentOrigin,
            new Rectangle(16, textureOffset, 16, 16),
            segment.Color,
            segment.Rotation,
            Globals.SnakeSegmentOrigin,
            Vector2.One,
            SpriteEffects.None,
            1f);
    }

    private static void DrawCorner(SpriteBatch spriteBatch, Texture2D texture, int textureOffset, SnakeSegment segment, Snake snake)
    {
        spriteBatch.Draw(texture,
            snake.GlobalPosition + segment.Position + Globals.SnakeSegmentOrigin,
            new Rectangle(0, textureOffset, 16, 16),
            segment.Color,
            segment.Rotation,
            Globals.SnakeSegmentOrigin,
            Vector2.One,
            segment.IsClockwise ? SpriteEffects.None : SpriteEffects.FlipVertically,
            1f);
    }
}