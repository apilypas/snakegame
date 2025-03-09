using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace SnakeGame.DesktopGL.Core.Renderers
{
    public class PlayFieldRenderer : RendererBase
    {
        private TiledMap _map;
        private TiledMapRenderer _mapRenderer;
        
        public readonly static Vector2 Offset = new(
            (Constants.ScreenWidth - Constants.WallWidth * Constants.SegmentSize) / 2f,
            (Constants.ScreenHeight - Constants.WallHeight * Constants.SegmentSize) / 2f
            );
        
        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _map = content.Load<TiledMap>("Map");
            _mapRenderer = new TiledMapRenderer(graphicsDevice, _map);
        }

        public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _mapRenderer.Draw(Matrix.CreateTranslation(Offset.X, Offset.Y, 0.0f));
            
            spriteBatch.DrawRectangle(
                Offset.X,
                Offset.Y,
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