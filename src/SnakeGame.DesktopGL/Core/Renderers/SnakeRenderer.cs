using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.DesktopGL.Core.Entities;
using SnakeGame.DesktopGL.Core.Sprites;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class SnakeRenderer : RendererBase
{
    private readonly IList<Snake> _snakes;

    private struct SnakeSprites()
    {
        public TextureSprite Segment { get; set; }
        public TextureSprite[] Corners { get; } = new TextureSprite[2];
        public TextureSprite Face { get; set; }
        public TextureSprite Head { get; set; }
        public TextureSprite Tail { get; set; }
    }

    private SnakeSprites _playerSnakeSprites;
    private SnakeSprites _enemySnakeTextures;

    public SnakeRenderer(IList<Snake> snakes)
    {
        _snakes = snakes;
    }

    public override void LoadContent(ContentManager content)
    {
        _playerSnakeSprites = LoadSnakeSprites(content, 0);
        _enemySnakeTextures = LoadSnakeSprites(content, 20);
    }

    private SnakeSprites LoadSnakeSprites(ContentManager content, int textureOffsetY)
    {
        var sprite = new SnakeSprites();

        // Segment
        sprite.Segment = TextureSprite
            .Create(new Rectangle(20, textureOffsetY, 20, 20))
            .Load(content, "snake");

        // Corner
        sprite.Corners[0] = TextureSprite
            .Create(new Rectangle(0, textureOffsetY, 20, 20))
            .Load(content, "snake");
        sprite.Corners[1] = TextureSprite
            .Create(new Rectangle(0, textureOffsetY, 20, 20))
            .Load(content, "snake");
        sprite.Corners[1].Effects = SpriteEffects.FlipVertically;

        // Head
        sprite.Face = TextureSprite
            .Create(new Rectangle(40, textureOffsetY, 20, 20))
            .Load(content, "snake");
        sprite.Head = TextureSprite
            .Create(new Rectangle(60, textureOffsetY, 20, 20))
            .Load(content, "snake");
        sprite.Head.Effects = SpriteEffects.FlipHorizontally;

        // Tail
        sprite.Tail = TextureSprite
            .Create(new Rectangle(60, textureOffsetY, 20, 20))
            .Load(content, "snake");
        
        return sprite;
    }

    public override void Render(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, float deltaTime)
    {
        foreach (var snake in _snakes)
        {
            RenderSnake(snake, graphicsDevice, spriteBatch);
        }
    }

    private void RenderSnake(Snake snake, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        if (snake.Segments.Count == 0)
            return;

        Offset = PlayFieldRenderer.GetPlayFieldOffset(graphicsDevice);
    
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
            Draw(spriteBatch, snake.Head, GetSprites(snake).Face);
        }
        
        Draw(spriteBatch, snake.Head, GetSprites(snake).Head);

        if (snake.Segments.Count == 1)
        {
            // If snake size is equal to 1 - draw tail line on same segment too
            Draw(spriteBatch, snake.Head, GetSprites(snake).Tail);
        }
    }

    private void DrawTail(SpriteBatch spriteBatch, Snake snake)
    {
        Draw(spriteBatch, snake.Tail, GetSprites(snake).Tail);
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
        Draw(spriteBatch, segment, GetSprites(snake).Segment);
    }

    private void DrawCorner(SpriteBatch spriteBatch, Snake snake, SnakeSegment segment)
    {
        if (segment.IsClockwise)
            Draw(spriteBatch, segment, GetSprites(snake).Corners[0]);
        else
            Draw(spriteBatch, segment, GetSprites(snake).Corners[1]);
    }

    private SnakeSprites GetSprites(Snake snake)
    {
        return snake is PlayerSnake ? _playerSnakeSprites : _enemySnakeTextures;
    }
}