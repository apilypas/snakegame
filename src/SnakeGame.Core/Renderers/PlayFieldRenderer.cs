using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using SnakeGame.Core.Entities;

namespace SnakeGame.Core.Renderers;

public class PlayFieldRenderer : RendererBase
{
    private readonly TiledMapRenderer _mapRenderer;
    private readonly TiledMap _tiledMap;

    public PlayFieldRenderer(GraphicsDevice graphicsDevice, PlayField playField)
    {
        _tiledMap = playField.TiledMap;
        _mapRenderer = new TiledMapRenderer(graphicsDevice, _tiledMap);
    }

    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        _mapRenderer.Update(gameTime);
            
        _mapRenderer.Draw(
            Matrix.CreateTranslation(
                Globals.PlayFieldOffset.X,
                Globals.PlayFieldOffset.Y,
                0.0f));
            
        spriteBatch.DrawRectangle(
            Globals.PlayFieldOffset.X,
            Globals.PlayFieldOffset.Y,
            _tiledMap.WidthInPixels,
            _tiledMap.WidthInPixels,
            Colors.DefaultTextColor);
    }
}