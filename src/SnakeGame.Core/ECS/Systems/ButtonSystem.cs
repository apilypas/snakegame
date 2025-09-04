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
        foreach (var entityId in ActiveEntities)
        {
            var button = _buttonMapper.Get(entityId);
            var transform = _transformMapper.Get(entityId);
            
            var bounds = new RectangleF(
                transform.Position.X,
                transform.Position.Y,
                button.Size.Width,
                button.Size.Height);

            var scaleY = (float)_graphicsDevice.Viewport.Height / Constants.VirtualScreenHeight;

            var pos = _inputManager.Mouse.Position;
            pos -= new Vector2(_graphicsDevice.Viewport.X, _graphicsDevice.Viewport.Y);
            pos /= scaleY;
            
            button.IsHovered = bounds.Contains(pos);
            
            button.IsPressed = button.IsHovered && _inputManager.Mouse.IsLeftButtonDown;

            if (button.IsHovered && _inputManager.Mouse.IsLeftButtonReleased)
            {
                button.Action?.Invoke();
            }
        }
    }
}