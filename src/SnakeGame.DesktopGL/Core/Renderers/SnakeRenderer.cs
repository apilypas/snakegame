using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.DesktopGL.Core.Entities;
using SnakeGame.DesktopGL.Core.Sprites;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class SnakeRenderer : RendererBase
{
    private Snake _snake;

    private TextureSprite _snakeSegmentSprite;
    private TextureSprite[] _snakeCornerSprites = new TextureSprite[2];
    private TextureSprite _snakeFaceSprite;
    private TextureSprite _snakeHeadSprite;
    private TextureSprite _snakeTailSprite;

    public SnakeRenderer(Snake snake)
    {
        _snake = snake;
    }

    public override void LoadContent(ContentManager content)
    {
        // Segment
        _snakeSegmentSprite = TextureSprite
            .Create(new Rectangle(20, 40, 20, 20))
            .Load(content, "snake");

        // Corner
        _snakeCornerSprites[0] = TextureSprite
            .Create(new Rectangle(0, 40, 20, 20))
            .Load(content, "snake");
        _snakeCornerSprites[1] = TextureSprite
            .Create(new Rectangle(0, 40, 20, 20))
            .WithEffects(SpriteEffects.FlipVertically)
            .Load(content, "snake");

        // Head
        _snakeFaceSprite = TextureSprite
            .Create(new Rectangle(40, 40, 20, 20))
            .Load(content, "snake");
        _snakeHeadSprite = TextureSprite
            .Create(new Rectangle(60, 40, 20, 20))
            .WithEffects(SpriteEffects.FlipHorizontally)
            .Load(content, "snake");

        // Tail
        _snakeTailSprite = TextureSprite
            .Create(new Rectangle(60, 40, 20, 20))
            .Load(content, "snake");
    }

    public override void Render(SpriteBatch spriteBatch, float deltaTime)
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
        
        DrawHead(spriteBatch, _snake.Head);

        if (_snake.Segments.Count > 1)
        {
            // Head & face drawing logic will cover this when length is equal to 1
            DrawTail(spriteBatch, _snake.Tail);
        }
    }

    private void DrawHead(SpriteBatch spriteBatch, SnakeSegment segment)
    {
        if (_snake.State == SnakeState.Alive)
        {
            // If snake is alive - draw face, otherwise face should not be shown
            Draw(spriteBatch, segment, _snakeFaceSprite);
        }
        
        Draw(spriteBatch, segment, _snakeHeadSprite);

        if (_snake.Segments.Count == 1)
        {
            // If snake size is equal to 1 - draw tail line on same segment too
            Draw(spriteBatch, segment, _snakeTailSprite);
        }
    }

    private void DrawTail(SpriteBatch spriteBatch, SnakeSegment segment)
    {
        Draw(spriteBatch, segment, _snakeTailSprite);
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
        Draw(spriteBatch, segment, _snakeSegmentSprite);
    }

    private void DrawCorner(SpriteBatch spriteBatch, SnakeSegment segment)
    {
        if (segment.IsClockwise)
            Draw(spriteBatch, segment, _snakeCornerSprites[0]);
        else
            Draw(spriteBatch, segment, _snakeCornerSprites[1]);
    }
}