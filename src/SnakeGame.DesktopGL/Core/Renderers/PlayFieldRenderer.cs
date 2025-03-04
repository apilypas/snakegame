using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace SnakeGame.DesktopGL.Core.Renderers
{
    public class PlayFieldRenderer : RendererBase
    {
        private TiledMap _map;
        private TiledMapRenderer _mapRenderer;

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _map = content.Load<TiledMap>("Level1");
            _mapRenderer = new TiledMapRenderer(graphicsDevice, _map);
        }

        public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var offset = GetPlayFieldOffset(spriteBatch.GraphicsDevice);
            
            _mapRenderer.Draw(Matrix.CreateTranslation(offset.X, offset.Y, 0.0f));
        }

        public override void Update(GameTime gameTime)
        {
            _mapRenderer.Update(gameTime);
        }

        public static Vector2 GetPlayFieldOffset(GraphicsDevice graphicsDevice)
        {
            var x = (graphicsDevice.Viewport.Width - 640) / 2f;
            var y = (graphicsDevice.Viewport.Height - 640) / 2f;
            return new Vector2(x, y);
        }
    }
}