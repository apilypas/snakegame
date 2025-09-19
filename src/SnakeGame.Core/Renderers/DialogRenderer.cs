using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using MonoGame.Extended.ECS;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Services;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.Renderers;

public class DialogRenderer
{
    private readonly Texture2D _userInterfaceTexture;
    private readonly SpriteFont _mainFont;
    private List<int> _orderedEntityIds = [];
    private ComponentMapper<DialogComponent> _dialogMapper;
    private ComponentMapper<ButtonComponent> _buttonMapper;
    private ComponentMapper<TransformComponent> _transformMapper;
    private ComponentMapper<DialogLabelComponent> _dialogLabelMapper;

    public DialogRenderer(GameContentManager contents)
    {
        _userInterfaceTexture = contents.UserInterfaceTexture;
        _mainFont = contents.MainFont;
    }

    public void Initialize(IComponentMapperService mapperService)
    {
        _dialogMapper = mapperService.GetMapper<DialogComponent>();
        _buttonMapper = mapperService.GetMapper<ButtonComponent>();
        _transformMapper = mapperService.GetMapper<TransformComponent>();
        _dialogLabelMapper = mapperService.GetMapper<DialogLabelComponent>();
    }

    public void Render(SpriteBatch spriteBatch, Bag<int> activeEntities)
    {
        foreach (var entityId in _orderedEntityIds)
        {
            var dialog = _dialogMapper.Get(entityId);

            if (dialog != null)
            {
                var transform = _transformMapper.Get(entityId);

                if (!dialog.IsTransparent)
                {
                    spriteBatch.FillRectangle(
                        Vector2.Zero,
                        new SizeF(Constants.VirtualScreenWidth, Constants.VirtualScreenHeight),
                        new Color(Color.Black, .6f),
                        1f);

                    spriteBatch.DrawFromNinePatch(
                        transform.Position,
                        dialog.Size,
                        _userInterfaceTexture,
                        new Rectangle(0, 0, 18, 18),
                        Color.White);
                }

                if (!string.IsNullOrEmpty(dialog.Title))
                {
                    var textSize = _mainFont.MeasureString(dialog.Title);
                    var position = new Vector2(
                        transform.Position.X + (dialog.Size.Width - textSize.X) / 2f,
                        transform.Position.Y + 4f);
                    
                    spriteBatch.DrawStringWithShadow(
                        _mainFont,
                        dialog.Title,
                        position,
                        Colors.DefaultTextColor);
                }

                if (!string.IsNullOrEmpty(dialog.Content))
                {
                    spriteBatch.DrawStringWithShadow(_mainFont,
                        dialog.Content,
                        transform.Position + new Vector2(8f, 24f),
                        Colors.DefaultTextColor);
                }

                foreach (var childEntityId in dialog.ChildrenEntities)
                {
                    var label = _dialogLabelMapper.Get(childEntityId);

                    if (label != null)
                    {
                        var labelTransform = _transformMapper.Get(childEntityId);
                
                        spriteBatch.DrawStringWithShadow(
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

                        if (button.IsFocused)
                        {
                            spriteBatch.DrawFromNinePatch(
                                buttonTransform.Position,
                                button.Size,
                                _userInterfaceTexture,
                                new Rectangle(144, 48, 18, 18),
                                Color.White);
                        }
                        
                        if (button.IsPressed)
                        {
                            spriteBatch.DrawFromNinePatch(
                                buttonTransform.Position,
                                button.Size,
                                _userInterfaceTexture,
                                new Rectangle(96, 48, 18, 18),
                                Color.White);
                        }
                        else if (button.IsHovered)
                        {
                            spriteBatch.DrawFromNinePatch(
                                buttonTransform.Position,
                                button.Size,
                                _userInterfaceTexture,
                                new Rectangle(48, 48, 18, 18),
                                Color.White);
                        }
                        else
                        {
                            spriteBatch.DrawFromNinePatch(
                                buttonTransform.Position,
                                button.Size,
                                _userInterfaceTexture,
                                new Rectangle(0, 48, 18, 18),
                                Color.White);
                        }

                        if (!string.IsNullOrEmpty(button.Text))
                        {
                            var textSize = _mainFont.MeasureString(button.Text);
                            var position = new Vector2(
                                buttonTransform.Position.X + (button.Size.Width - textSize.X) / 2f,
                                buttonTransform.Position.Y + (button.Size.Height - textSize.Y) / 2f);

                            spriteBatch.DrawStringWithShadow(
                                _mainFont,
                                button.Text,
                                position,
                                Colors.DefaultTextColor);
                        }
                    }
                }
            }
        }
    }

    public void OnEntityAdded(int entityId)
    {
        if (_dialogMapper.Has(entityId))
        {
            _orderedEntityIds.Add(entityId);
            _orderedEntityIds = _orderedEntityIds.OrderBy(x => _dialogMapper.Get(x).OrderId).ToList();
        }
    }

    public void OnEntityRemoved(int entityId)
    {
        if (_dialogMapper.Has(entityId))
            _orderedEntityIds.Remove(entityId);
    }
}