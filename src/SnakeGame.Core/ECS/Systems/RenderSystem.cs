using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Renderers;
using SnakeGame.Core.Services;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.ECS.Systems;

public class RenderSystem : EntityDrawSystem
{
    private readonly SpriteBatch _spriteBatch;
    private readonly SnakeRenderer _snakeRenderer;
    private readonly SpriteFont _smallFont;
    private readonly CameraManager _cameraManager;
    private ComponentMapper<SnakeComponent> _snakeMapper;
    private ComponentMapper<SpriteComponent> _spriteMapper;
    private ComponentMapper<TransformComponent> _transformMapper;
    private ComponentMapper<FadingTextComponent> _fadingTextMapper;
    private ComponentMapper<PlayFieldComponent> _playFieldMapper;
    private ComponentMapper<HudLabelComponent> _labelMapper;
    private ComponentMapper<PlayerComponent> _playerMapper;

    public RenderSystem(GraphicsDevice graphics, ContentManager contents, CameraManager cameraManager)
        : base(Aspect.One(
            typeof(SnakeComponent),
            typeof(SpriteComponent),
            typeof(FadingTextComponent),
            typeof(PlayFieldComponent)))
    {
        _spriteBatch = new SpriteBatch(graphics);
        _snakeRenderer = new SnakeRenderer(contents);
        _smallFont = contents.SmallFont;
        _cameraManager = cameraManager;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _snakeMapper = mapperService.GetMapper<SnakeComponent>();
        _spriteMapper = mapperService.GetMapper<SpriteComponent>();
        _transformMapper = mapperService.GetMapper<TransformComponent>();
        _fadingTextMapper = mapperService.GetMapper<FadingTextComponent>();
        _playFieldMapper = mapperService.GetMapper<PlayFieldComponent>();
        _labelMapper = mapperService.GetMapper<HudLabelComponent>();
        _playerMapper = mapperService.GetMapper<PlayerComponent>();
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            transformMatrix: _cameraManager.GetViewMatrix());
        
        foreach (var entityId in ActiveEntities)
        {
            var snake = _snakeMapper.Get(entityId);

            if (snake is { IsInitialized: true })
            {
                var isPlayer = _playerMapper.Has(entityId);
                _snakeRenderer.Render(_spriteBatch, snake, isPlayer);
            }
            
            var sprite = _spriteMapper.Get(entityId);

            if (sprite != null)
            {
                var transform = _transformMapper.Get(entityId);
                _spriteBatch.Draw(sprite.Sprite, transform.Position);
            }
            
            var fadingText = _fadingTextMapper.Get(entityId);

            if (fadingText != null)
            {
                var transform = _transformMapper.Get(entityId);
                _spriteBatch.DrawString(_smallFont, fadingText.Text, transform.Position, Color.White);
            }
            
            var playField = _playFieldMapper.Get(entityId);

            if (playField != null)
            {
                foreach (var tile in playField.Tiles)
                {
                    _spriteBatch.Draw(playField.TilesTexture, tile.Position, new Rectangle(0, 0, 16, 16), Color.White);
                    _spriteBatch.Draw(playField.TilesTexture, tile.Position, tile.TileRectangle, Color.White);
                }
        
                _spriteBatch.DrawRectangle(
                    Vector2.Zero,
                    Globals.PlayFieldRectangle.Size,
                    Color.Black);
            }
        }
        
        _spriteBatch.End();
    }
}