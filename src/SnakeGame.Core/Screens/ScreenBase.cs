using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using SnakeGame.Core.Renderers;

namespace SnakeGame.Core.Screens;

public abstract class ScreenBase(Game game) : GameScreen(game)
{
    private readonly IList<RendererBase> _renderers = [];
    private SpriteBatch _spriteBatch;
    private RenderTarget2D _renderTarget;
    private int _screenWidth;
    private int _screenHeight;
    private Vector2 _scale = Vector2.One;
    
    public override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        HandleScreenSizeChange();
        
        foreach (var renderer in _renderers)
        {
            renderer.LoadContent(GraphicsDevice, Content);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(_renderTarget);
        
        GraphicsDevice.Clear(Colors.DefaultBackgroundColor);

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
        
        GraphicsDevice.SetRenderTarget(null);

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
            GetScale(),
            SpriteEffects.None,
            0f);
        
        _spriteBatch.End();
    }

    public override void Update(GameTime gameTime)
    {
        HandleScreenSizeChange();
        
        foreach (var renderer in _renderers)
        {
            renderer.Update(gameTime);
        }
    }
    
    public Vector2 GetScale()
    {
        return _scale;
    }

    protected void AddRenderer(RendererBase renderer)
    {
        _renderers.Add(renderer);
    }
    
    private void HandleScreenSizeChange()
    {
        if (_screenWidth == GraphicsDevice.Viewport.Width && _screenHeight == GraphicsDevice.Viewport.Height)
            return;

        _screenWidth = GraphicsDevice.Viewport.Width;
        _screenHeight = GraphicsDevice.Viewport.Height;

        _renderTarget?.Dispose();
        
        var screenRatio = _screenWidth / (float)_screenHeight;
        var width = (int) (Constants.ScreenHeight * screenRatio);

        _renderTarget = new RenderTarget2D(GraphicsDevice, width, Constants.ScreenHeight);
        
        var scale = MathF.Min(_screenWidth / (float)width, _screenHeight / (float)Constants.ScreenHeight);

        _scale = new Vector2(scale, scale);
    }
}