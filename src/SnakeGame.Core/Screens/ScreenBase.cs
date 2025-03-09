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
    
    public ScreenScalingHandler ScalingHandler { get; private set; }

    public override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _renderTarget = new RenderTarget2D(GraphicsDevice, Constants.ScreenWidth, Constants.ScreenHeight);
        ScalingHandler = new ScreenScalingHandler(this, Constants.ScreenWidth, Constants.ScreenHeight);
        
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
            new Rectangle(ScalingHandler.Position, ScalingHandler.Size),
            Color.White);
        
        _spriteBatch.End();
    }

    public override void Update(GameTime gameTime)
    {
        ScalingHandler.Update();
        
        foreach (var renderer in _renderers)
        {
            renderer.Update(gameTime);
        }
    }

    protected void AddRenderer(RendererBase renderer)
    {
        _renderers.Add(renderer);
    }
}