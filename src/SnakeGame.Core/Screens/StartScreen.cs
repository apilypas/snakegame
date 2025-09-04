using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Screens;
using SnakeGame.Core.ECS.Components;
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
            .AddSystem(new InputSystem(_inputs, Game, entityFactory))
            .AddSystem(new ButtonSystem(Game.GraphicsDevice, _inputs))
            .AddSystem(new StartMenuButtonEventSystem(this, Game, entityFactory))
            .AddSystem(new DialogSystem())
            .AddSystem(new DialogRenderSystem(Game.GraphicsDevice, contents))
            .Build();
        
        entityFactory.Initialize(_world);

        var gameState = _world.CreateEntity();

        var startButtonId = entityFactory.CreateButton(
            "Start",
            new Vector2(100f, 100f),
            new SizeF(100f, 40f),
            () =>
            {
                gameState.Attach(new ButtonEventComponent
                {
                    Event = ButtonEvents.StartNew
                });
            });

        var scoreBoardButtonId = entityFactory.CreateButton(
            "Score Board",
            new Vector2(100f, 160f),
            new SizeF(100f, 40f),
            () =>
            {
                gameState.Attach(new ButtonEventComponent
                {
                    Event = ButtonEvents.ShowScoreBoard
                });
            });

        var creditsButtonId = entityFactory.CreateButton(
            "Credits",
            new Vector2(100f, 220f),
            new SizeF(100f, 40f),
            () =>
            {
                gameState.Attach(new ButtonEventComponent
                {
                    Event = ButtonEvents.ShowCredits
                });
            });

        var quitButtonId = entityFactory.CreateButton(
            "Quit",
            new Vector2(100f, 280f),
            new SizeF(100f, 40f),
            () =>
            {
                Game.Exit();
            });
        
        var label1 = _world.CreateEntity();
        label1.Attach(new DialogLabelComponent
        {
            Text = "Yet another",
            Font = contents.BigFont
        });
        label1.Attach(new TransformComponent
        {
            Position = new Vector2(
                Constants.VirtualScreenWidth / 2f - 40f,
                Constants.VirtualScreenHeight / 2f - 100f)
        });
        
        var label2 = _world.CreateEntity();
        label2.Attach(new DialogLabelComponent
        {
            Text = "Snake",
            Font = contents.LogoFont
        });
        label2.Attach(new TransformComponent
        {
            Position = new Vector2(
                Constants.VirtualScreenWidth / 2f - 40f,
                Constants.VirtualScreenHeight / 2f - 70f)
        });
        
        var label3 = _world.CreateEntity();
        label3.Attach(new DialogLabelComponent
        {
            Text = "Game",
            Font = contents.LogoFont
        });
        label3.Attach(new TransformComponent
        {
            Position = new Vector2(
                Constants.VirtualScreenWidth / 2f - 40f,
                Constants.VirtualScreenHeight / 2f - 0f)
        });
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