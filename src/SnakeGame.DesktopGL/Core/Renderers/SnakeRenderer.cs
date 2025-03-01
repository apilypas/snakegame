using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.DesktopGL.Core.Entities;
using SnakeGame.DesktopGL.Core.Sprites;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class SnakeRenderer<T> : RendererBase
    where T : Snake
{
    private readonly T _snake;
    private readonly IList<T> _snakes;

    private TextureSprite _snakeSegmentSprite;
    private TextureSprite[] _snakeCornerSprites = new TextureSprite[2];
    private TextureSprite _snakeFaceSprite;
    private TextureSprite _snakeHeadSprite;
    private TextureSprite _snakeTailSprite;

    public SnakeRenderer(T snake)
    {
        _snake = snake;
    }

    public SnakeRenderer(IList<T> snakes)
    {
        _snakes = snakes;
    }

    public override void LoadContent(ContentManager content)
    {
        var textureOffsetY = 0;

        if (typeof(T) == typeof(EnemySnake))
            textureOffsetY = 20;

        // Segment
        _snakeSegmentSprite = TextureSprite
            .Create(new Rectangle(20, textureOffsetY, 20, 20))
            .Load(content, "snake");

        // Corner
        _snakeCornerSprites[0] = TextureSprite
            .Create(new Rectangle(0, textureOffsetY, 20, 20))
            .Load(content, "snake");
        _snakeCornerSprites[1] = TextureSprite
            .Create(new Rectangle(0, textureOffsetY, 20, 20))
            .WithEffects(SpriteEffects.FlipVertically)
            .Load(content, "snake");

        // Head
        _snakeFaceSprite = TextureSprite
            .Create(new Rectangle(40, textureOffsetY, 20, 20))
            .Load(content, "snake");
        _snakeHeadSprite = TextureSprite
            .Create(new Rectangle(60, textureOffsetY, 20, 20))
            .WithEffects(SpriteEffects.FlipHorizontally)
            .Load(content, "snake");

        // Tail
        _snakeTailSprite = TextureSprite
            .Create(new Rectangle(60, textureOffsetY, 20, 20))
            .Load(content, "snake");
    }

    public override void Render(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, float deltaTime)
    {
        if (_snake != null)
        {
            RenderSnake(_snake, graphicsDevice, spriteBatch);
        }
        
        if (_snakes != null)
        {
            foreach (var snake in _snakes)
            {
                RenderSnake(snake, graphicsDevice, spriteBatch);
            }
        }
    }

    private void RenderSnake(T snake, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        if (snake.Segments.Count == 0)
            return;

        Offset = PlayFieldRenderer.GetPlayFieldOffset(graphicsDevice);
    
        DrawSegment(spriteBatch, snake.Head);

        if (snake.Segments.Count > 1)
        {
            // Head & face drawing logic will cover this when length is equal to 1
            DrawSegment(spriteBatch, snake.Tail);
        }
        
        for (var i = 0; i < snake.Segments.Count - 1; i++)
        {
            DrawBody(spriteBatch, snake.Segments[i]);
        }
        
        DrawHead(spriteBatch, snake);

        if (snake.Segments.Count > 1)
        {
            // Head & face drawing logic will cover this when length is equal to 1
            DrawTail(spriteBatch, snake.Tail);
        }
    }

    private void DrawHead(SpriteBatch spriteBatch, T snake)
    {
        if (snake.State == SnakeState.Alive)
        {
            // If snake is alive - draw face, otherwise face should not be shown
            Draw(spriteBatch, snake.Head, _snakeFaceSprite);
        }
        
        Draw(spriteBatch, snake.Head, _snakeHeadSprite);

        if (snake.Segments.Count == 1)
        {
            // If snake size is equal to 1 - draw tail line on same segment too
            Draw(spriteBatch, snake.Head, _snakeTailSprite);
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