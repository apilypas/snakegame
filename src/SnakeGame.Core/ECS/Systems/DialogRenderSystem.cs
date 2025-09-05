using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Services;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.ECS.Systems;

public class DialogRenderSystem : EntityDrawSystem
{
    private readonly GraphicsDevice _graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _userInterfaceTexture;
    private readonly SpriteFont _mainFont;
    private ComponentMapper<DialogComponent> _dialogMapper;
    private ComponentMapper<ButtonComponent> _buttonMapper;
    private ComponentMapper<TransformComponent> _transformMapper;
    private ComponentMapper<DialogLabelComponent> _dialogLabelMapper;

    public DialogRenderSystem(
        GraphicsDevice graphics,
        ContentManager contents) 
        : base(Aspect.One(
            typeof(DialogComponent),
            typeof(ButtonComponent),
            typeof(DialogLabelComponent)))
    {
        _graphics = graphics;
        _spriteBatch = new SpriteBatch(graphics);
        _userInterfaceTexture = contents.UserInterfaceTexture;
        _mainFont = contents.MainFont;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _dialogMapper = mapperService.GetMapper<DialogComponent>();
        _buttonMapper = mapperService.GetMapper<ButtonComponent>();
        _transformMapper = mapperService.GetMapper<TransformComponent>();
        _dialogLabelMapper = mapperService.GetMapper<DialogLabelComponent>();
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
            var dialog = _dialogMapper.Get(entityId);

            if (dialog != null)
            {
                var transform = _transformMapper.Get(entityId);

                if (!dialog.IsTransparent)
                {
                    _spriteBatch.FillRectangle(
                        Vector2.Zero,
                        new SizeF(Constants.VirtualScreenWidth, Constants.VirtualScreenHeight),
                        new Color(Color.Black, .6f),
                        1f);

                    _spriteBatch.DrawFromNinePatch(
                        transform.Position,
                        dialog.Size,
                        _userInterfaceTexture,
                        new Rectangle(0, 0, 48, 48),
                        Color.White);
                }

                if (!string.IsNullOrEmpty(dialog.Title))
                {
                    var textSize = _mainFont.MeasureString(dialog.Title);
                    var position = new Vector2(
                        transform.Position.X + (dialog.Size.Width - textSize.X) / 2f,
                        transform.Position.Y + 4f);
                    
                    _spriteBatch.DrawStringWithShadow(
                        _mainFont,
                        dialog.Title,
                        position,
                        Color.White);
                }

                if (!string.IsNullOrEmpty(dialog.Content))
                {
                    _spriteBatch.DrawStringWithShadow(_mainFont,
                        dialog.Content,
                        transform.Position + new Vector2(8f, 24f),
                        Color.White);
                }

                foreach (var childEntityId in dialog.ChildrenEntities)
                {
                    var label = _dialogLabelMapper.Get(childEntityId);

                    if (label != null)
                    {
                        var labelTransform = _transformMapper.Get(childEntityId);
                
                        _spriteBatch.DrawStringWithShadow(
                            label.Font,
                            label.Text,
                            labelTransform.Position,
                            label.Color,
                            labelTransform.Rotation,
                            Vector2.Zero);
                    }
                    
                    var button = _buttonMapper.Get(childEntityId);

                    if (button != null)
                    {
                        var buttonTransform = _transformMapper.Get(childEntityId);

                        if (button.IsPressed)
                        {
                            _spriteBatch.DrawFromNinePatch(
                                buttonTransform.Position,
                                button.Size,
                                _userInterfaceTexture,
                                new Rectangle(96, 48, 48, 48),
                                Color.White);
                        }
                        else if (button.IsHovered)
                        {
                            _spriteBatch.DrawFromNinePatch(
                                buttonTransform.Position,
                                button.Size,
                                _userInterfaceTexture,
                                new Rectangle(48, 48, 48, 48),
                                Color.White);
                        }
                        else
                        {
                            _spriteBatch.DrawFromNinePatch(
                                buttonTransform.Position,
                                button.Size,
                                _userInterfaceTexture,
                                new Rectangle(0, 48, 48, 48),
                                Color.White);
                        }

                        if (!string.IsNullOrEmpty(button.Text))
                        {
                            var textSize = _mainFont.MeasureString(button.Text);
                            var position = new Vector2(
                                buttonTransform.Position.X + (button.Size.Width - textSize.X) / 2f,
                                buttonTransform.Position.Y + (button.Size.Height - textSize.Y) / 2f);

                            _spriteBatch.DrawStringWithShadow(
                                _mainFont,
                                button.Text,
                                position,
                                Color.White);
                        }
                    }
                }
            }
        }

        _spriteBatch.End();
    }
}