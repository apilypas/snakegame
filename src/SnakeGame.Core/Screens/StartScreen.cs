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
        var contents = new GameContentManager();
        contents.LoadContent(Content);

        var bindings = new[]
        {
            InputBinding.FromKeys(InputActions.Fullscreen, Keys.LeftAlt, Keys.Enter),
            InputBinding.FromKeys(InputActions.Fullscreen, Keys.RightAlt, Keys.Enter),
            InputBinding.FromKeys(InputActions.Cancel, Keys.Escape),
            InputBinding.FromKeys(InputActions.Up, Keys.W),
            InputBinding.FromKeys(InputActions.Up, Keys.Up),
            InputBinding.FromButton(InputActions.Up, Buttons.DPadUp),
            InputBinding.FromButton(InputActions.Up, Buttons.LeftThumbstickUp),
            InputBinding.FromKeys(InputActions.Down, Keys.S),
            InputBinding.FromKeys(InputActions.Down, Keys.Down),
            InputBinding.FromButton(InputActions.Down, Buttons.DPadDown),
            InputBinding.FromButton(InputActions.Down, Buttons.LeftThumbstickDown),
            InputBinding.FromKeys(InputActions.Left, Keys.A),
            InputBinding.FromKeys(InputActions.Left, Keys.Left),
            InputBinding.FromButton(InputActions.Left, Buttons.DPadLeft),
            InputBinding.FromButton(InputActions.Left, Buttons.LeftThumbstickLeft),
            InputBinding.FromKeys(InputActions.Right, Keys.D),
            InputBinding.FromKeys(InputActions.Right, Keys.Right),
            InputBinding.FromButton(InputActions.Right, Buttons.DPadRight),
            InputBinding.FromButton(InputActions.Right, Buttons.LeftThumbstickRight),
            InputBinding.FromKeys(InputActions.Select, Keys.Space),
            InputBinding.FromButton(InputActions.Select, Buttons.B),
            InputBinding.FromButton(InputActions.Start, Buttons.Start)
        };

        _inputs = new InputManager(bindings);
        
        var entityFactory = new EntityFactory();

        _world = new WorldBuilder()
            .AddSystem(new InputSystem(_inputs, Game, entityFactory, null))
            .AddSystem(new ButtonSystem(Game.GraphicsDevice, _inputs, Game.Window))
            .AddSystem(new StartMenuButtonEventSystem(this, Game, entityFactory))
            .AddSystem(new SoundEffectSystem(contents))
            .AddSystem(new DialogSystem())
            .AddSystem(new DialogButtonFocusSystem())
            .AddSystem(new RenderSystem(Game.GraphicsDevice, contents, Game.Window))
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