using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.DesktopGL.Core;

namespace SnakeGame.DesktopGL;

public class SnakeGame : Game
{
    private GraphicsDeviceManager _graphics;

    private PlayState _playState;

    public SnakeGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1024;
        _graphics.PreferredBackBufferHeight = 768;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.IsBorderless = true;

        _playState = new PlayState();
    }

    protected override void Initialize()
    {
        _playState.Initialize();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _playState.LoadContent(GraphicsDevice, Content);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _playState.Update(gameTime);
    
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _playState.Draw(GraphicsDevice, gameTime);

        base.Draw(gameTime);
    }
}
