using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Services;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.ECS.Systems;

public class HudRenderSystem : EntityDrawSystem
{
    private readonly GraphicsDevice _graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly SpriteFont _mainFont;
    private readonly Texture2D _userInterfaceTexture;
    private ComponentMapper<HudLabelComponent> _hudLabelMapper;
    private ComponentMapper<HudSpriteComponent> _hudSpriteMapper;
    private ComponentMapper<TransformComponent> _transformMapper;
    private ComponentMapper<HudLevelDisplayComponent> _hudLevelDisplayMapper;

    public HudRenderSystem(GraphicsDevice graphics, ContentManager contents) 
        : base(Aspect.One(typeof(HudLabelComponent), typeof(HudSpriteComponent), typeof(HudLevelDisplayComponent)))
    {
        _graphics = graphics;
        _spriteBatch = new SpriteBatch(graphics);
        _mainFont = contents.MainFont;
        _userInterfaceTexture = contents.UserInterfaceTexture;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _hudLabelMapper = mapperService.GetMapper<HudLabelComponent>();
        _hudSpriteMapper = mapperService.GetMapper<HudSpriteComponent>();
        _transformMapper = mapperService.GetMapper<TransformComponent>();
        _hudLevelDisplayMapper = mapperService.GetMapper<HudLevelDisplayComponent>();
    }

    public override void Draw(GameTime gameTime)
    {
        var scaleY = (float)_graphics.Viewport.Height / Constants.VirtualScreenHeight;

        var transformMatrix = Matrix.CreateScale(scaleY * Constants.Zoom, scaleY * Constants.Zoom, 1f);
        
        _spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            transformMatrix: transformMatrix);

        foreach (var entityId in ActiveEntities)
        {
            var label = _hudLabelMapper.Get(entityId);

            if (label != null)
            {
                var transform = _transformMapper.Get(entityId);
                
                _spriteBatch.DrawStringWithShadow(
                    label.Font,
                    label.Text,
                    transform.Position,
                    label.Color,
                    transform.Rotation,
                    Vector2.Zero);
            }
            
            var sprite = _hudSpriteMapper.Get(entityId);

            if (sprite != null)
            {
                var transform = _transformMapper.Get(entityId);
                _spriteBatch.Draw(sprite.Sprite, transform.Position);
            }
            
            var hudLevelDisplay = _hudLevelDisplayMapper.Get(entityId);

            if (hudLevelDisplay != null)
            {
                var transform = _transformMapper.Get(entityId);
                
                _spriteBatch.DrawStringWithShadow(
                    _mainFont,
                    hudLevelDisplay.Level,
                    transform.Position,
                    Colors.ScoreTimeColor);
                
                _spriteBatch.DrawFromNinePatch(
                    transform.Position + new Vector2(0f, 22f),
                    new SizeF(160f, 26f),
                    _userInterfaceTexture,
                    new Rectangle(32, 96, 18, 18),
                    Color.White,
                    6,
                    6);
                
                _spriteBatch.DrawFromNinePatch(
                    transform.Position + new Vector2(2f, 24f),
                    new SizeF(156f * hudLevelDisplay.Progress, 22f),
                    _userInterfaceTexture,
                    new Rectangle(0, 96, 18, 18),
                    Color.White,
                    6,
                    6);
            }
        }
        
        _spriteBatch.End();
    }
}