using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Renderers;
using SnakeGame.Core.Screens;

namespace SnakeGame.Core.Systems;

public class RenderSystem
{
    private readonly IList<RendererBase> _renderers = [];
    private readonly GraphicsDevice _graphics;
    private SpriteBatch _spriteBatch;
    private RenderTarget2D _renderTarget;
    private ScreenScaleHandler _screenScaleHandler;

    public RenderSystem(GraphicsDevice graphics)
    {
        _graphics = graphics;
        _screenScaleHandler = new ScreenScaleHandler(graphics);
        _screenScaleHandler.UpdateScreenScaling();
        
        _spriteBatch = new SpriteBatch(graphics);
        
        _renderTarget = new RenderTarget2D(
            graphics,
            Globals.VirtualScreenWidth,
            Globals.VirtualScreenHeight);
    }

    public void Add(RendererBase renderer)
    {
        _renderers.Add(renderer);
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
            Globals.ScreenScale,
            SpriteEffects.None,
            0f);
        
        _spriteBatch.End();
    }

    public void Update(GameTime gameTime)
    {
        if (_screenScaleHandler.UpdateScreenScaling())
        {
            _renderTarget?.Dispose();
            _renderTarget = new RenderTarget2D(
                _graphics,
                Globals.VirtualScreenWidth,
                Globals.VirtualScreenHeight);
        }
    }
}