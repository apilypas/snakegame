using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Screens;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Entities;
using SnakeGame.Core.ECS.Systems;
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

        var bindings = new[]
        {
            InputBinding.Create(InputActions.Fullscreen, Keys.LeftAlt, Keys.Enter),
            InputBinding.Create(InputActions.Fullscreen, Keys.RightAlt, Keys.Enter),
            InputBinding.Create(InputActions.Cancel, Keys.Escape),
            InputBinding.Create(InputActions.Up, Keys.W),
            InputBinding.Create(InputActions.Up, Keys.Up),
            InputBinding.Create(InputActions.Up, Buttons.DPadUp),
            InputBinding.Create(InputActions.Up, Buttons.LeftThumbstickUp),
            InputBinding.Create(InputActions.Down, Keys.S),
            InputBinding.Create(InputActions.Down, Keys.Down),
            InputBinding.Create(InputActions.Down, Buttons.DPadDown),
            InputBinding.Create(InputActions.Down, Buttons.LeftThumbstickDown),
            InputBinding.Create(InputActions.Left, Keys.A),
            InputBinding.Create(InputActions.Left, Keys.Left),
            InputBinding.Create(InputActions.Left, Buttons.DPadLeft),
            InputBinding.Create(InputActions.Left, Buttons.LeftThumbstickLeft),
            InputBinding.Create(InputActions.Right, Keys.D),
            InputBinding.Create(InputActions.Right, Keys.Right),
            InputBinding.Create(InputActions.Right, Buttons.DPadRight),
            InputBinding.Create(InputActions.Right, Buttons.LeftThumbstickRight),
            InputBinding.Create(InputActions.Faster, Keys.Space),
            InputBinding.Create(InputActions.Faster, Buttons.A),
            InputBinding.Create(InputActions.Start, Buttons.Start)
        };

        _inputs = new InputManager(bindings);
        
        var entityFactory = new EntityFactory();

        _world = new WorldBuilder()
            .AddSystem(new InputSystem(_inputs, Game, entityFactory, null))
            .AddSystem(new ButtonSystem(Game.GraphicsDevice, _inputs))
            .AddSystem(new StartMenuButtonEventSystem(this, Game, entityFactory))
            .AddSystem(new SoundEffectSystem(contents))
            .AddSystem(new DialogSystem())
            .AddSystem(new DialogButtonFocusSystem())
            .AddSystem(new DialogRenderSystem(Game.GraphicsDevice, contents))
            .Build();
        
        entityFactory.Initialize(_world, contents);

        entityFactory.Dialog.CreateNavigationIntent();
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