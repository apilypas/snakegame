using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Renderers;
using SnakeGame.Core.Services;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.ECS.Systems;

public class RenderSystem : EntityDrawSystem
{
    private readonly SpriteBatch _spriteBatch;
    private readonly GraphicsDevice _graphics;
    private readonly WorldRenderer _worldRenderer;
    private readonly HudRenderer _hudRenderer;
    private readonly DialogRenderer _dialogRenderer;
    private RenderTarget2D _renderTarget;
    private Rectangle _renderRectangle;

    public RenderSystem(GraphicsDevice graphics, ContentManager contents, GameWindow window)
        : base(Aspect.One(
            typeof(SnakeComponent),
            typeof(SpriteComponent),
            typeof(FadingTextComponent),
            typeof(PlayFieldComponent),
            typeof(HudLabelComponent),
            typeof(HudSpriteComponent),
            typeof(HudLevelDisplayComponent),
            typeof(DialogComponent),
            typeof(ButtonComponent),
            typeof(DialogLabelComponent)))
    {
        _graphics = graphics;
        _spriteBatch = new SpriteBatch(graphics);
        _worldRenderer = new WorldRenderer(contents);
        _hudRenderer = new HudRenderer(contents);
        _dialogRenderer = new DialogRenderer(contents);
        
        window.ClientSizeChanged += OnClientSizeChanged;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _worldRenderer.Initialize(mapperService);
        _hudRenderer.Initialize(mapperService);
        _dialogRenderer.Initialize(mapperService);
        
        RebuildRenderTarget();
    }

    public override void Draw(GameTime gameTime)
    {
        _graphics.SetRenderTarget(_renderTarget);
        
        _graphics.Clear(Colors.DefaultBackgroundColor);
        
        _spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            transformMatrix: Globals.PlayFieldCenterViewTransform);
        
        _worldRenderer.Render(_spriteBatch, ActiveEntities);
        
        _spriteBatch.End();
        
        _spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp);

        _hudRenderer.Render(_spriteBatch, ActiveEntities);
        _dialogRenderer.Render(_spriteBatch, ActiveEntities);
        
        _spriteBatch.End();
        
        _graphics.SetRenderTarget(null);
        
        _graphics.Clear(Colors.DefaultBackgroundColor);
        
        _spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp);
        
        _spriteBatch.Draw(_renderTarget, _renderRectangle, Color.White);
        
        _spriteBatch.End();
    }
    
    protected override void OnEntityAdded(int entityId)
    {
        _dialogRenderer.OnEntityAdded(entityId);
    }

    protected override void OnEntityRemoved(int entityId)
    {
        _dialogRenderer.OnEntityRemoved(entityId);
    }
    
    private void OnClientSizeChanged(object sender, EventArgs e)
    {
        RebuildRenderTarget();
    }

    private void RebuildRenderTarget()
    {
        _renderTarget?.Dispose();
        
        _renderTarget = new RenderTarget2D(
            _graphics,
            Constants.VirtualScreenWidth,
            Constants.VirtualScreenHeight,
            false,
            SurfaceFormat.Color,
            DepthFormat.None,
            0,
            RenderTargetUsage.DiscardContents);
        
        var scale = _graphics.Viewport.GetRenderTargetScale();
        _renderRectangle =  _graphics.Viewport.GetRenderTargetRectangle(scale);
    }
}