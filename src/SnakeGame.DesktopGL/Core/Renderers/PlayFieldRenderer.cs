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
        private Vector2 _offset;
        
        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _map = content.Load<TiledMap>("Map");
            _mapRenderer = new TiledMapRenderer(graphicsDevice, _map);
            _offset = RendererUtils.GetPlayFieldOffset(graphicsDevice);
        }

        public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _mapRenderer.Draw(Matrix.CreateTranslation(_offset.X, _offset.Y, 0.0f));
        }

        public override void Update(GameTime gameTime)
        {
            _mapRenderer.Update(gameTime);
        }
    }
}