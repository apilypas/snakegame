using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.DesktopGL.Core;

namespace SnakeGame.DesktopGL;

public class SnakeGame : Game
{
    private GraphicsDeviceManager _graphics;

    private StateManager _stateManager;

    public SnakeGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1024;
        _graphics.PreferredBackBufferHeight = 768;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.IsBorderless = true;
    }

    protected override void Initialize()
    {
        _stateManager = new StateManager(GraphicsDevice, Content);
        _stateManager.Initialize();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _stateManager.GetCurrentState().LoadContent(GraphicsDevice, Content);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _stateManager.GetCurrentState().Update(gameTime);
    
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _stateManager.GetCurrentState().Draw(GraphicsDevice, gameTime);

        base.Draw(gameTime);
    }
}
