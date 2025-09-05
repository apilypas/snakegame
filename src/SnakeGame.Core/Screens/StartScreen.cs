using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Screens;
using SnakeGame.Core.ECS.Entities;
using SnakeGame.Core.ECS.Systems;
using SnakeGame.Core.Inputs;
using SnakeGame.Core.Services;

namespace SnakeGame.Core.Screens;

public class StartScreen : GameScreen
{
    private readonly InputManager _inputs;
    private readonly World _world;

    public StartScreen(Game game) : base(game)
    {
        var contents = new ContentManager();
        contents.LoadContent(Content);

        _inputs = new InputManager();
        _inputs.BindKey(InputActions.Fullscreen, Keys.LeftAlt, Keys.Enter);
        _inputs.BindKey(InputActions.Fullscreen, Keys.RightAlt, Keys.Enter);
        _inputs.BindKey(InputActions.Cancel, Keys.Escape);
        _inputs.BindButton(InputActions.Start, Buttons.Start);
        
        var entityFactory = new EntityFactory();

        _world = new WorldBuilder()
            .AddSystem(new InputSystem(_inputs, Game, entityFactory, null))
            .AddSystem(new ButtonSystem(Game.GraphicsDevice, _inputs))
            .AddSystem(new StartMenuButtonEventSystem(this, Game, entityFactory))
            .AddSystem(new DialogSystem())
            .AddSystem(new DialogRenderSystem(Game.GraphicsDevice, contents))
            .Build();
        
        entityFactory.Initialize(_world, contents);

        entityFactory.Dialog.CreateStartScreen();
    }

    public override void Update(GameTime gameTime)
    {
        _world.Update(gameTime);
        _inputs.Update();
    }

    public override void Draw(GameTime gameTime)
    {
        Game.GraphicsDevice.Clear(Colors.DefaultBackgroundColor);
        
        _world.Draw(gameTime);
    }
}