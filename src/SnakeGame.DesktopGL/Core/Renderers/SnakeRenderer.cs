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
    private TextureSprite _snakeHeadSprite;
    private TextureSprite _snakeTailSprite;

    public SnakeRenderer(Snake snake)
    {
        _snake = snake;
    }

    public override void LoadContent(ContentManager content)
    {
        // Segment
        _snakeSegmentSprite = new TextureSprite(new Rectangle(20, 40, 20, 20))
            .Load(content, "snake");

        // Corner
        _snakeCornerSprites[0] = new TextureSprite(new Rectangle(0, 40, 20, 20))
            .Load(content, "snake");
        _snakeCornerSprites[1] = new TextureSprite(new Rectangle(0, 40, 20, 20))
            .WithEffects(SpriteEffects.FlipVertically)
            .Load(content, "snake");

        // Head
        _snakeHeadSprite = new TextureSprite(new Rectangle(40, 40, 20, 20))
            .Load(content, "snake");

        // Tail
        _snakeTailSprite = new TextureSprite(new Rectangle(60, 40, 20, 20))
            .Load(content, "snake");
    }

    public override void Render(SpriteBatch spriteBatch, float deltaTime)
    {
        DrawSegment(spriteBatch, _snake.Head);
        DrawSegment(spriteBatch, _snake.Tail);

        for (var i = 0; i < _snake.Segments.Count - 1; i++)
            DrawBody(spriteBatch, _snake.Segments[i]);
        
        DrawHead(spriteBatch, _snake.Head);
        DrawTail(spriteBatch, _snake.Tail);
    }

    private void DrawHead(SpriteBatch spriteBatch, SnakeSegment segment)
    {
        Draw(spriteBatch, segment, _snakeHeadSprite);
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