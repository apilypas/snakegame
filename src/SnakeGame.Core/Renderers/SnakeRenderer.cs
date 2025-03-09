using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
using SnakeGame.Core.Entities;

namespace SnakeGame.Core.Renderers;

public class SnakeRenderer(IList<Snake> snakes) : RendererBase
{
    private struct SnakeSprites()
    {
        public Sprite Segment { get; set; }
        public Sprite[] Corners { get; } = new Sprite[5];
        public Sprite Face { get; set; }
        public Sprite Head { get; set; }
        public Sprite Tail { get; set; }
    }

    private SnakeSprites _playerSnakeSprites;
    private readonly SnakeSprites[] _enemySnakeTextures = new SnakeSprites[4];
    
    private readonly Vector2 _origin = new(Constants.SegmentSize / 2f, Constants.SegmentSize / 2f);
    private readonly Vector2 _offset = PlayFieldRenderer.Offset;
    
    private Texture2D _texture;

    public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _texture = content.Load<Texture2D>("Snake");
        _playerSnakeSprites = LoadSnakeSprites(0);
        _enemySnakeTextures[0] = LoadSnakeSprites(16);
        _enemySnakeTextures[1] = LoadSnakeSprites(32);
        _enemySnakeTextures[2] = LoadSnakeSprites(48);
        _enemySnakeTextures[3] = LoadSnakeSprites(64);
    }

    private SnakeSprites LoadSnakeSprites(int textureOffsetY)
    {
        var sprite = new SnakeSprites();
        
        // Segment
        sprite.Segment = new Sprite(new Texture2DRegion(
            _texture,
            new Rectangle(16, textureOffsetY, 16, 16))
            );
        sprite.Segment.Origin = _origin;

        // Corner
        sprite.Corners[0] = new Sprite(new Texture2DRegion(
            _texture,
            new Rectangle(0, textureOffsetY, 16, 16))
            );
        sprite.Corners[0].Origin = _origin;
        
        sprite.Corners[1] = new Sprite(new Texture2DRegion(
            _texture,
            new Rectangle(0, textureOffsetY, 16, 16))
            );
        sprite.Corners[1].Effect = SpriteEffects.FlipVertically;
        sprite.Corners[1].Origin = _origin;
        
        // Head
        sprite.Face = new Sprite(new Texture2DRegion(
            _texture,
            new Rectangle(32, textureOffsetY, 16, 16))
            );
        sprite.Face.Origin = _origin;

        sprite.Head = new Sprite(new Texture2DRegion(
            _texture,
            new Rectangle(48, textureOffsetY, 16, 16))
            );
        sprite.Head.Effect = SpriteEffects.FlipHorizontally;
        sprite.Head.Origin = _origin;

        // Tail
        sprite.Tail = new Sprite(new Texture2DRegion(
            _texture,
            new Rectangle(48, textureOffsetY, 16, 16))
            );
        sprite.Tail.Origin = _origin;
        
        return sprite;
    }

    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        foreach (var snake in snakes)
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
            GetSprites(snake).Face.Draw(
                spriteBatch,
                snake.Head.Location + _offset + _origin,
                snake.Head.Rotation,
                Vector2.One);
        }
        
        GetSprites(snake).Head.Draw(
            spriteBatch,
            snake.Head.Location + _offset + _origin,
            snake.Head.Rotation,
            Vector2.One);

        if (snake.Segments.Count == 1)
        {
            // If snake size is equal to 1 - draw tail line on same segment too
            GetSprites(snake).Tail.Draw(
                spriteBatch,
                snake.Head.Location + _offset + _origin,
                snake.Head.Rotation,
                Vector2.One);
        }
    }

    private void DrawTail(SpriteBatch spriteBatch, Snake snake)
    {
        GetSprites(snake).Tail.Draw(
            spriteBatch,
            snake.Tail.Location + _offset + _origin,
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
        GetSprites(snake).Segment.Draw(
            spriteBatch,
            segment.Location + _offset + _origin,
            segment.Rotation,
            Vector2.One);
    }

    private void DrawCorner(SpriteBatch spriteBatch, Snake snake, SnakeSegment segment)
    {
        if (segment.IsClockwise)
        {
            GetSprites(snake).Corners[0].Draw(
                spriteBatch,
                segment.Location + _offset + _origin,
                segment.Rotation,
                Vector2.One);
        }
        else
        {
            GetSprites(snake).Corners[1].Draw(
                spriteBatch,
                segment.Location + _offset + _origin,
                segment.Rotation,
                Vector2.One);
        }
    }

    private SnakeSprites GetSprites(Snake snake)
    {
        if (snake is PlayerSnake)
            return _playerSnakeSprites;
        
        var textureIndex = snake.Id % _enemySnakeTextures.Length;
        return _enemySnakeTextures[textureIndex];
    }

    public override void Update(GameTime gameTime)
    {
    }
}