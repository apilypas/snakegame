using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Services;

namespace SnakeGame.Core.ECS.Systems;

public class ButtonSystem : EntityUpdateSystem
{
    private readonly InputManager _inputManager;
    private readonly GraphicsDevice _graphicsDevice;
    private ComponentMapper<ButtonComponent> _buttonMapper;
    private ComponentMapper<TransformComponent> _transformMapper;
    private Vector2 _lastMousePosition = Vector2.Zero;

    public ButtonSystem(GraphicsDevice graphicsDevice, InputManager inputManager) 
        : base(Aspect.All(typeof(ButtonComponent)))
    {
        _graphicsDevice = graphicsDevice;
        _inputManager = inputManager;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _buttonMapper = mapperService.GetMapper<ButtonComponent>();
        _transformMapper = mapperService.GetMapper<TransformComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        HandleHoverState();
        HandlePressedState();
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

    private void HandlePressedState()
    {
        foreach (var entityId in ActiveEntities)
        {
            var button = _buttonMapper.Get(entityId);

            if (button.IsHandlingInput)
            {
                button.IsPressed = button.IsHovered && _inputManager.Mouse.IsLeftButtonDown;

                if (button.IsHovered && _inputManager.Mouse.IsLeftButtonReleased)
                {
                    button.Action?.Invoke();
                }
            }
        }
    }

    private void HandleHoverState()
    {
        var mousePosition = _inputManager.Mouse.Position;

        if (Vector2.Distance(mousePosition, _lastMousePosition) < 1f)
            return;
        
        _lastMousePosition = mousePosition;
        
        // Adjust to scaling
        var scaleY = (float)_graphicsDevice.Viewport.Height / Constants.VirtualScreenHeight;
        mousePosition -= new Vector2(_graphicsDevice.Viewport.X, _graphicsDevice.Viewport.Y);
        mousePosition /= scaleY;
        
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