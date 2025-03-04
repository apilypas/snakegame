using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.DesktopGL.Core.Renderers;

namespace SnakeGame.DesktopGL.Core.Screens;

public abstract class ScreenBase
{
    private readonly IList<RendererBase> _renderers = [];

    public abstract void Initialize();
    public abstract void Update(float deltaTime);

    public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        foreach (var renderer in _renderers)
        {
            renderer.LoadContent(content);
        }
    }

    public void Draw(GraphicsDevice graphicsDevice, float deltaTime, SpriteBatch spriteBatch)
    {
        foreach (var renderer in _renderers)
        {
            renderer.Render(graphicsDevice, spriteBatch, deltaTime);
        }
    }

    protected void AddRenderer(RendererBase renderer)
    {
        _renderers.Add(renderer);
    }
}