using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace SnakeGame.Core.Renderers
{
    public class PlayFieldRenderer : RendererBase
    {
        private TiledMap _map;
        private TiledMapRenderer _mapRenderer;
        
        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _map = content.Load<TiledMap>("Map");
            _mapRenderer = new TiledMapRenderer(graphicsDevice, _map);
        }

        public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _mapRenderer.Draw(
                Matrix.CreateTranslation(
                    Globals.PlayFieldOffset.X,
                    Globals.PlayFieldOffset.Y,
                    0.0f));
            
            spriteBatch.DrawRectangle(
                Globals.PlayFieldOffset.X,
                Globals.PlayFieldOffset.Y,
                _map.WidthInPixels,
                _map.WidthInPixels,
                Colors.DefaultTextColor);
        }

        public override void Update(GameTime gameTime)
        {
            _mapRenderer.Update(gameTime);
        }
    }
}