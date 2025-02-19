using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.DesktopGL.Core;

public interface IState
{
    void Initialize();
    void LoadContent(GraphicsDevice graphicsDevice, ContentManager content);
    void Update(GameTime gameTime);
    void Draw(GraphicsDevice graphicsDevice, GameTime gameTime);
}