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

    public override void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
    {
        if (_tiledMapRenderer == null)
            _tiledMapRenderer = new TiledMapRenderer(spriteBatch.GraphicsDevice, TiledMap);
        
        _tiledMapRenderer.Draw(
            Matrix.CreateTranslation(
                position.X,
                position.Y,
                0.0f));
            
        spriteBatch.DrawRectangle(
            position.X,
            position.Y,
            TiledMap.WidthInPixels,
            TiledMap.WidthInPixels,
            Colors.DefaultTextColor);
    }
}