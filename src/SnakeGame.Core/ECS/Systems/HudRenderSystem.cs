using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.ECS.Systems;

public class HudRenderSystem : EntityDrawSystem
{
    private readonly GraphicsDevice _graphics;
    private readonly SpriteBatch _spriteBatch;
    private ComponentMapper<HudLabelComponent> _hudLabelMapper;
    private ComponentMapper<HudSpriteComponent> _hudSpriteMapper;
    private ComponentMapper<TransformComponent> _transformMapper;

    public HudRenderSystem(GraphicsDevice graphics) 
        : base(Aspect.One(typeof(HudLabelComponent), typeof(HudSpriteComponent)))
    {
        _graphics = graphics;
        _spriteBatch = new SpriteBatch(graphics);
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _hudLabelMapper = mapperService.GetMapper<HudLabelComponent>();
        _hudSpriteMapper = mapperService.GetMapper<HudSpriteComponent>();
        _transformMapper = mapperService.GetMapper<TransformComponent>();
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
        }
        
        _spriteBatch.End();
    }
}