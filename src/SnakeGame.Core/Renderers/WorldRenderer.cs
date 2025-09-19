using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Graphics;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Services;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.Renderers;

public class WorldRenderer
{
    private readonly SnakeRenderer _snakeRenderer;
    private readonly SpriteFont _smallFont;
    private ComponentMapper<SnakeComponent> _snakeMapper;
    private ComponentMapper<SpriteComponent> _spriteMapper;
    private ComponentMapper<TransformComponent> _transformMapper;
    private ComponentMapper<FadingTextComponent> _fadingTextMapper;
    private ComponentMapper<PlayFieldComponent> _playFieldMapper;
    private ComponentMapper<PlayerComponent> _playerMapper;

    public WorldRenderer(ContentManager contents)
    {
        _snakeRenderer = new SnakeRenderer(contents);
        _smallFont = contents.SmallFont;
    }

    public void Initialize(IComponentMapperService mapperService)
    {
        _snakeMapper = mapperService.GetMapper<SnakeComponent>();
        _spriteMapper = mapperService.GetMapper<SpriteComponent>();
        _transformMapper = mapperService.GetMapper<TransformComponent>();
        _fadingTextMapper = mapperService.GetMapper<FadingTextComponent>();
        _playFieldMapper = mapperService.GetMapper<PlayFieldComponent>();
        _playerMapper = mapperService.GetMapper<PlayerComponent>();
    }
    
    public void Render(SpriteBatch spriteBatch, Bag<int> activeEntities)
    {
        foreach (var entityId in activeEntities)
        {
            var playField = _playFieldMapper.Get(entityId);

            if (playField != null)
            {
                foreach (var tile in playField.Tiles)
                {
                    spriteBatch.Draw(playField.TilesTexture, tile.Position, new Rectangle(0, 0, 16, 16), Color.White);
                    spriteBatch.Draw(playField.TilesTexture, tile.Position, tile.TileRectangle, Color.White);
                }
        
                spriteBatch.DrawRectangle(
                    Vector2.Zero,
                    Globals.PlayFieldRectangle.Size,
                    Color.Black);
            }
        }

        foreach (var entityId in activeEntities)
        {
            var snake = _snakeMapper.Get(entityId);

            if (snake is { IsInitialized: true })
            {
                var isPlayer = _playerMapper.Has(entityId);
                _snakeRenderer.Render(spriteBatch, snake);
            }
        }

        foreach (var entityId in activeEntities)
        {
            var sprite = _spriteMapper.Get(entityId);

            if (sprite != null)
            {
                var transform = _transformMapper.Get(entityId);
                spriteBatch.Draw(sprite.Sprite, transform.Position, transform.Rotation, transform.Scale);
            }
        }

        foreach (var entityId in activeEntities)
        {
            var fadingText = _fadingTextMapper.Get(entityId);

            if (fadingText != null)
            {
                var transform = _transformMapper.Get(entityId);
                spriteBatch.DrawStringWithShadow(_smallFont,
                    fadingText.Text,
                    transform.Position,
                    Colors.DefaultTextColor);
            }
        }
    }
}