using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
using SnakeGame.Core.Entities;

namespace SnakeGame.Core.Renderers;

public class SnakeRenderer : RendererBase
{
    private readonly Vector2 _origin = new(Constants.SegmentSize / 2f, Constants.SegmentSize / 2f);
    
    private readonly IList<Snake> _snakes;
    
    public SnakeRenderer(IList<Snake> snakes)
    {
        _snakes = snakes;
    }

    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        foreach (var snake in _snakes)
        {
            RenderSnake(snake, spriteBatch);
        }
    }

    private void RenderSnake(Snake snake, SpriteBatch spriteBatch)
    {
        if (snake.Segments.Count == 0)
            return;

        DrawSegment(spriteBatch, snake, snake.Head);

        if (snake.Segments.Count > 1)
        {
            // Head & face drawing logic will cover this when length is equal to 1
            DrawSegment(spriteBatch, snake, snake.Tail);
        }
        
        for (var i = 0; i < snake.Segments.Count - 1; i++)
        {
            DrawBody(spriteBatch, snake, snake.Segments[i]);
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
            // If snake is alive - draw face, otherwise face should not be shown
            snake.FaceSprite.Draw(
                spriteBatch,
                snake.Head.Location + Globals.PlayFieldOffset + _origin,
                snake.Head.Rotation,
                Vector2.One);
        }
        
        snake.HeadSprite.Draw(
            spriteBatch,
            snake.Head.Location + Globals.PlayFieldOffset + _origin,
            snake.Head.Rotation,
            Vector2.One);

        if (snake.Segments.Count == 1)
        {
            // If snake size is equal to 1 - draw tail line on same segment too
            snake.TailSprite.Draw(
                spriteBatch,
                snake.Head.Location + Globals.PlayFieldOffset + _origin,
                snake.Head.Rotation,
                Vector2.One);
        }
    }

    private void DrawTail(SpriteBatch spriteBatch, Snake snake)
    {
        snake.TailSprite.Draw(
            spriteBatch,
            snake.Tail.Location + Globals.PlayFieldOffset + _origin,
            snake.Tail.Rotation,
            Vector2.One);
    }

    private void DrawBody(SpriteBatch spriteBatch, Snake snake, SnakeSegment segment)
    {
        if (segment.IsCorner)
            DrawCorner(spriteBatch, snake, segment);
        else
            DrawSegment(spriteBatch, snake, segment);
    }

    private void DrawSegment(SpriteBatch spriteBatch, Snake snake, SnakeSegment segment)
    {
        snake.SegmentSprite.Draw(
            spriteBatch,
            segment.Location + Globals.PlayFieldOffset + _origin,
            segment.Rotation,
            Vector2.One);
    }

    private void DrawCorner(SpriteBatch spriteBatch, Snake snake, SnakeSegment segment)
    {
        if (segment.IsClockwise)
        {
            snake.CornersSprites[0].Draw(
                spriteBatch,
                segment.Location + Globals.PlayFieldOffset + _origin,
                segment.Rotation,
                Vector2.One);
        }
        else
        {
            snake.CornersSprites[1].Draw(
                spriteBatch,
                segment.Location + Globals.PlayFieldOffset + _origin,
                segment.Rotation,
                Vector2.One);
        }
    }
}