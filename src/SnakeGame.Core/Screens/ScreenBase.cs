using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using SnakeGame.Core.Renderers;

namespace SnakeGame.Core.Screens;

public class ScreenBase(Game game) : GameScreen(game)
{
    private SpriteBatch _spriteBatch;
    private RenderTarget2D _renderTarget;
    private ScreenScaleHandler _screenScaleHandler;
    private readonly IList<RendererBase> _renderers = [];

    public override void LoadContent()
    {
        base.LoadContent();
        
        _screenScaleHandler = new ScreenScaleHandler(this);
        _screenScaleHandler.UpdateScreenScaling();
        
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        _renderTarget = new RenderTarget2D(
            GraphicsDevice,
            Globals.VirtualScreenWidth,
            Globals.VirtualScreenHeight);
    }

    public override void Update(GameTime gameTime)
    {
        if (_screenScaleHandler.UpdateScreenScaling())
        {
            _renderTarget?.Dispose();
            _renderTarget = new RenderTarget2D(
                GraphicsDevice,
                Globals.VirtualScreenWidth,
                Globals.VirtualScreenHeight);
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
            Globals.ScreenScale,
            SpriteEffects.None,
            0f);
        
        _spriteBatch.End();
    }
    
    protected void AddRenderer(RendererBase renderer)
    {
        _renderers.Add(renderer);
    }
}