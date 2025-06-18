using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using SnakeGame.Core.Renderers;

namespace SnakeGame.Core.Systems;

public class RenderSystem
{
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    private readonly IList<RendererBase> _renderers = [];
    private readonly GraphicsDevice _graphics;
    private readonly SpriteBatch _spriteBatch;
    private readonly InputManager _inputs;
    private readonly RenderTarget2D _renderTarget;
    private int _previousViewportWidth;
    private int _previousViewportHeight;
    private float _scale = 1f;
    private Vector2 _renderAt;

    public RenderSystem(GraphicsDevice graphics, InputManager inputs)
    {
        _graphics = graphics;
        _inputs = inputs;
        
        _spriteBatch = new SpriteBatch(graphics);
        
        _renderTarget = new RenderTarget2D(
            _graphics,
            Constants.ScreenWidth,
            Constants.ScreenHeight);
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
            _renderAt,
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
            
            _scale = Math.Min(
                _graphics.Viewport.Height / (float)Constants.ScreenHeight,
                _graphics.Viewport.Width / (float)Constants.ScreenWidth);

            _renderAt = new Vector2(
                (_graphics.Viewport.Width - Constants.ScreenWidth * _scale) / 2f,
                (_graphics.Viewport.Height - Constants.ScreenHeight * _scale) / 2f);
            
            _inputs.Mouse.Scale = new Vector2(_scale, _scale);
            _inputs.Mouse.Offset = _renderAt;
            
            _logger.Info($"Screen scale changed: {_scale}");
        }
    }
}