using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace SnakeGame.Core.Entities;

public class PlayField : Entity
{
    private TiledMapRenderer _tiledMapRenderer;
    
    public TiledMap TiledMap { get; set; }

    public override void Update(GameTime gameTime)
    {
        if (_tiledMapRenderer != null)
            _tiledMapRenderer.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (_tiledMapRenderer == null)
            _tiledMapRenderer = new TiledMapRenderer(spriteBatch.GraphicsDevice, TiledMap);
        
        _tiledMapRenderer.Draw(
            Matrix.CreateTranslation(
                GlobalPosition.X,
                GlobalPosition.Y,
                0.0f));
            
        spriteBatch.DrawRectangle(
            GlobalPosition.X,
            GlobalPosition.Y,
            TiledMap.WidthInPixels,
            TiledMap.WidthInPixels,
            Colors.DefaultTextColor);
    }
}