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

public class HudRenderer
{
    private readonly SpriteFont _mainFont;
    private readonly Texture2D _userInterfaceTexture;
    private ComponentMapper<HudLabelComponent> _hudLabelMapper;
    private ComponentMapper<HudSpriteComponent> _hudSpriteMapper;
    private ComponentMapper<TransformComponent> _transformMapper;
    private ComponentMapper<HudLevelDisplayComponent> _hudLevelDisplayMapper;

    public HudRenderer(ContentManager contents)
    {
        _mainFont = contents.MainFont;
        _userInterfaceTexture = contents.UserInterfaceTexture;
    }

    public void Initialize(IComponentMapperService mapperService)
    {
        _hudLabelMapper = mapperService.GetMapper<HudLabelComponent>();
        _hudSpriteMapper = mapperService.GetMapper<HudSpriteComponent>();
        _transformMapper = mapperService.GetMapper<TransformComponent>();
        _hudLevelDisplayMapper = mapperService.GetMapper<HudLevelDisplayComponent>();
    }

    public void Render(SpriteBatch spriteBatch, Bag<int> activeEntities)
    {
        foreach (var entityId in activeEntities)
        {
            var label = _hudLabelMapper.Get(entityId);

            if (label != null)
            {
                var transform = _transformMapper.Get(entityId);
                
                spriteBatch.DrawStringWithShadow(
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
                spriteBatch.Draw(sprite.Sprite, transform.Position);
            }
            
            var hudLevelDisplay = _hudLevelDisplayMapper.Get(entityId);

            if (hudLevelDisplay != null)
            {
                var transform = _transformMapper.Get(entityId);
                
                spriteBatch.DrawStringWithShadow(
                    _mainFont,
                    hudLevelDisplay.Level,
                    transform.Position,
                    Colors.ScoreTimeColor);
                
                spriteBatch.DrawFromNinePatch(
                    transform.Position + new Vector2(-2f, 16f),
                    new SizeF(96f, 22f),
                    _userInterfaceTexture,
                    new Rectangle(32, 96, 18, 18),
                    Color.White,
                    6,
                    6);
                
                spriteBatch.DrawFromNinePatch(
                    transform.Position + new Vector2(0f, 18f),
                    new SizeF(92f * hudLevelDisplay.Progress, 18f),
                    _userInterfaceTexture,
                    new Rectangle(0, 96, 18, 18),
                    Color.White,
                    6,
                    6);
            }
        }
    }
}