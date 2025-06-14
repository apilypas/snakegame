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
    private readonly RenderTarget2D _renderTarget;

    public RenderSystem(GraphicsDevice graphics)
    {
        _graphics = graphics;
        
        _spriteBatch = new SpriteBatch(graphics);
        
        _renderTarget = new RenderTarget2D(
            graphics,
            Constants.ScreenWidth,
            Constants.ScreenHeight);
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
            1f,
            SpriteEffects.None,
            0f);
        
        _spriteBatch.End();
    }
}