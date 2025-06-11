using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Entities;

namespace SnakeGame.Core.Renderers;

public class SnakeRenderer : RendererBase
{
    private readonly Vector2 _origin = new(Constants.SegmentSize / 2f, Constants.SegmentSize / 2f);
    
    private readonly Snake _snake;
    
    public SnakeRenderer(Snake snake)
    {
        _snake = snake;
    }

    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        RenderSnake(_snake, spriteBatch);
    }

    private void RenderSnake(Snake snake, SpriteBatch spriteBatch)
    {
        if (snake.Segments.Count == 0)
            return;

        if (snake.GlobalPosition == Vector2.Zero)
        {
            //
        }

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
            snake.FaceSprite.GlobalPosition = _snake.GlobalPosition + snake.Head.Position + _origin;
            snake.FaceSprite.Rotation = snake.Head.Rotation;
            
            snake.FaceSprite.Draw(
                spriteBatch,
                null);
        }
        
        snake.HeadSprite.GlobalPosition = _snake.GlobalPosition + snake.Head.Position + _origin;
        snake.HeadSprite.Rotation = snake.Head.Rotation;
        
        snake.HeadSprite.Draw(
            spriteBatch,
            null);

        if (snake.Segments.Count == 1)
        {
            // If snake size is equal to 1 - draw tail line on same segment too
            snake.TailSprite.GlobalPosition = _snake.GlobalPosition + snake.Head.Position + _origin;
            snake.TailSprite.Rotation = snake.Head.Rotation;
            
            snake.TailSprite.Draw(
                spriteBatch,
                null);
        }
    }

    private void DrawTail(SpriteBatch spriteBatch, Snake snake)
    {
        snake.TailSprite.GlobalPosition = _snake.GlobalPosition + snake.Tail.Position + _origin;
        snake.TailSprite.Rotation = snake.Tail.Rotation;
        
        snake.TailSprite.Draw(
            spriteBatch,
            null);
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
        snake.SegmentSprite.GlobalPosition = _snake.GlobalPosition + segment.Position + _origin;
        snake.SegmentSprite.Rotation = segment.Rotation;
        
        snake.SegmentSprite.Draw(
            spriteBatch,
            null);
    }

    private void DrawCorner(SpriteBatch spriteBatch, Snake snake, SnakeSegment segment)
    {
        if (segment.IsClockwise)
        {
            snake.CornersSprites[0].GlobalPosition = _snake.GlobalPosition + segment.Position + _origin;
            snake.CornersSprites[0].Rotation = segment.Rotation;
            
            snake.CornersSprites[0].Draw(
                spriteBatch,
                null);
        }
        else
        {
            snake.CornersSprites[1].GlobalPosition = _snake.GlobalPosition + segment.Position + _origin;
            snake.CornersSprites[1].Rotation = segment.Rotation;
            
            snake.CornersSprites[1].Draw(
                spriteBatch,
                null);
        }
    }
}