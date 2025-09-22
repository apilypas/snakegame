using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Input;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Services;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.ECS.Systems;

public class ButtonSystem : EntityUpdateSystem
{
    private readonly InputManager _inputManager;
    private readonly GraphicsDevice _graphics;
    private ComponentMapper<ButtonComponent> _buttonMapper;
    private ComponentMapper<TransformComponent> _transformMapper;
    private ComponentMapper<SoundEffectComponent> _soundEffectMapper;
    private Vector2 _lastMousePosition = Vector2.Zero;
    private float _renderTargetScale = 1f;
    private Vector2 _renderTargetOffset = Vector2.Zero;

    public ButtonSystem(GraphicsDevice graphics, InputManager inputManager, GameWindow window) 
        : base(Aspect.All(typeof(ButtonComponent)))
    {
        _graphics = graphics;
        _inputManager = inputManager;

        window.ClientSizeChanged += OnClientSizeChanged;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _buttonMapper = mapperService.GetMapper<ButtonComponent>();
        _transformMapper = mapperService.GetMapper<TransformComponent>();
        _soundEffectMapper = mapperService.GetMapper<SoundEffectComponent>();
        
        RebuildRenderTargetData();
    }

    public override void Update(GameTime gameTime)
    {
        HandleHoverState();
        HandlePressedState();
        HandleClicks();
    }

    protected override void OnEntityAdded(int entityId)
    {
        if (_buttonMapper.Has(entityId))
        {
            _lastMousePosition = Vector2.Zero;
        }
    }

    protected override void OnEntityRemoved(int entityId)
    {
        if (_buttonMapper.Has(entityId))
        {
            _lastMousePosition = Vector2.Zero;
        }
    }
    
    private void OnClientSizeChanged(object sender, EventArgs e)
    {
         RebuildRenderTargetData();
    }

    private void RebuildRenderTargetData()
    {
        _renderTargetScale = _graphics.Viewport.GetRenderTargetScale();
        _renderTargetOffset = _graphics.Viewport.GetRenderTargetRectangle(_renderTargetScale).Location.ToVector2();
    }

    private void HandleClicks()
    {
        foreach (var entityId in ActiveEntities)
        {
            var button = _buttonMapper.Get(entityId);
            if (button.IsClicked)
            {
                button.Action();
                button.IsClicked = false;

                _soundEffectMapper.Put(entityId, new SoundEffectComponent
                {
                    Type = SoundEffectTypes.Click
                });
            }
        }
    }

    private void HandlePressedState()
    {
        foreach (var entityId in ActiveEntities)
        {
            var button = _buttonMapper.Get(entityId);

            if (button.IsHandlingInput)
            {
                button.IsPressed = button.IsHovered && _inputManager.MouseInput.IsButtonDown(MouseButton.Left);

                if (button.IsHovered && _inputManager.MouseInput.WasButtonReleased(MouseButton.Left))
                {
                    button.IsClicked = true;
                }
            }
        }
    }

    private void HandleHoverState()
    {
        var mousePosition = _inputManager.MouseInput.MousePosition;

        if (Vector2.Distance(mousePosition, _lastMousePosition) < 1f)
            return;
        
        _lastMousePosition = mousePosition;
        
        // Adjust to scaling
        
        mousePosition.X = (mousePosition.X - _renderTargetOffset.X) / _renderTargetScale;
        mousePosition.Y = (mousePosition.Y - _renderTargetOffset.Y) / _renderTargetScale;
        
        foreach (var entityId in ActiveEntities)
        {
            var button = _buttonMapper.Get(entityId);

            if (button.IsHandlingInput)
            {
                var transform = _transformMapper.Get(entityId);

                var bounds = new RectangleF(
                    transform.Position.X,
                    transform.Position.Y,
                    button.Size.Width,
                    button.Size.Height);

                button.IsHovered = bounds.Contains(mousePosition);
            }
        }
    }
}