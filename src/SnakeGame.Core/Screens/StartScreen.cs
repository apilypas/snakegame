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
        _inputs.BindKey(InputActions.Up, Keys.W);
        _inputs.BindKey(InputActions.Up, Keys.Up);
        _inputs.BindButton(InputActions.Up, Buttons.DPadUp);
        _inputs.BindButton(InputActions.Up, Buttons.LeftThumbstickUp);
        _inputs.BindKey(InputActions.Down, Keys.S);
        _inputs.BindKey(InputActions.Down, Keys.Down);
        _inputs.BindButton(InputActions.Down, Buttons.DPadDown);
        _inputs.BindButton(InputActions.Down, Buttons.LeftThumbstickDown);
        _inputs.BindKey(InputActions.Left, Keys.A);
        _inputs.BindKey(InputActions.Left, Keys.Left);
        _inputs.BindButton(InputActions.Left, Buttons.DPadLeft);
        _inputs.BindButton(InputActions.Left, Buttons.LeftThumbstickLeft);
        _inputs.BindKey(InputActions.Right, Keys.D);
        _inputs.BindKey(InputActions.Right, Keys.Right);
        _inputs.BindButton(InputActions.Right, Buttons.DPadRight);
        _inputs.BindButton(InputActions.Right, Buttons.LeftThumbstickRight);
        _inputs.BindKey(InputActions.Faster, Keys.Space);
        _inputs.BindButton(InputActions.Faster, Buttons.A);
        _inputs.BindButton(InputActions.Start, Buttons.Start);
        
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