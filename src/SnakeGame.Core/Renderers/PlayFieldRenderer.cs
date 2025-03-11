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
        private PlayFieldOffsetHandler _playFieldOffsetHandler;
        
        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _map = content.Load<TiledMap>("Map");
            _mapRenderer = new TiledMapRenderer(graphicsDevice, _map);
            _playFieldOffsetHandler = new PlayFieldOffsetHandler(graphicsDevice);
        }

        public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var offset = _playFieldOffsetHandler.Offset;
            
            _mapRenderer.Draw(Matrix.CreateTranslation(offset.X, offset.Y, 0.0f));
            
            spriteBatch.DrawRectangle(
                offset.X,
                offset.Y,
                _map.WidthInPixels,
                _map.WidthInPixels,
                Colors.DefaultTextColor);
        }

        public override void Update(GameTime gameTime)
        {
            _playFieldOffsetHandler.Update();
            _mapRenderer.Update(gameTime);
        }
    }
}