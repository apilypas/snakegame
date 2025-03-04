using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using SnakeGame.DesktopGL.Core.Renderers;

namespace SnakeGame.DesktopGL.Core.Screens;

public abstract class ScreenBase(Game game) : GameScreen(game)
{
    private readonly Game _game = game;
    private readonly IList<RendererBase> _renderers = [];
    private SpriteBatch _spriteBatch;

    public override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        foreach (var renderer in _renderers)
        {
            renderer.LoadContent(_game.GraphicsDevice, _game.Content);
        }
    }

    public override void Draw(GameTime gameTime)
    {
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
    }

    public override void Update(GameTime gameTime)
    {
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