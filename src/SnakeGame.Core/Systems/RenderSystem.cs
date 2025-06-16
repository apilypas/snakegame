using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Renderers;

namespace SnakeGame.Core.Systems;

public class RenderSystem
{
    private readonly IList<RendererBase> _renderers = [];
    private readonly GraphicsDevice _graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly InputManager _inputs;
    private RenderTarget2D _renderTarget;
    private int _previousViewportWidth;
    private int _previousViewportHeight;
    private float _scale = 1f;

    public RenderSystem(GraphicsDevice graphics, InputManager inputs)
    {
        _graphics = graphics;
        _inputs = inputs;
        
        _spriteBatch = new SpriteBatch(graphics);
        
        _renderTarget = new RenderTarget2D(
            _graphics,
            _graphics.Viewport.Width,
            _graphics.Viewport.Height);
    }

    public void Add(RendererBase renderer)
    {
        _renderers.Add(renderer);
    }

    public void Update()
    {
        HandleScreenChange();
    }

    public void Render(GameTime gameTime)
    {
        _graphics.SetRenderTarget(_renderTarget);
        
        _graphics.Clear(Colors.DefaultBackgroundColor);

        _spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp
            );
        
        foreach (var renderer in _renderers)
        {
            renderer.Render(_spriteBatch, gameTime);
        }
        
        _spriteBatch.End();
        
        _graphics.SetRenderTarget(null);

        _spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp
            );
        
        _spriteBatch.Draw(
            _renderTarget,
            Vector2.Zero,
            null,
            Color.White,
            0f,
            Vector2.Zero,
            _scale,
            SpriteEffects.None,
            0f);
        
        _spriteBatch.End();
    }
    
    private void HandleScreenChange()
    {
        if (_graphics.Viewport.Width != _previousViewportWidth
            || _graphics.Viewport.Height != _previousViewportHeight)
        {
            _previousViewportWidth = _graphics.Viewport.Width;
            _previousViewportHeight = _graphics.Viewport.Height;
            
            _scale = _graphics.Viewport.Height / (float)Constants.ScreenHeight;
            
            _renderTarget = new RenderTarget2D(
                _graphics,
                (int)(Constants.ScreenWidth * _scale),
                (int)(Constants.ScreenHeight * _scale));

            _inputs.Mouse.Scale = new Vector2(_scale, _scale);
        }
    }
}