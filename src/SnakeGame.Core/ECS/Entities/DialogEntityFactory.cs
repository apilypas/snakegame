using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Services;

namespace SnakeGame.Core.ECS.Entities;

public class DialogEntityFactory(World world, ContentManager contents)
{
    public void CreateStartScreen()
    {
        var dialogEntity = world.CreateEntity();
        
        var dialog = new DialogComponent
        {
            Size = new SizeF(Constants.VirtualScreenWidth, Constants.VirtualScreenHeight),
            IsTransparent = true
        };

        dialogEntity.Attach(dialog);

        var startButton = CreateButton(
            "Start",
            new Vector2(740f, 150f),
            new SizeF(120f, 52f),
            () =>
            {
                dialogEntity.Attach(new ButtonEventComponent
                {
                    Event = ButtonEvents.StartNew
                });
            });

        startButton.Get<ButtonComponent>().FocusOrderId = 1;
        
        dialog.ChildrenEntities.Add(startButton.Id);

        var scoreBoardButton = CreateButton(
            "Score Board",
            new Vector2(740f, 210f),
            new SizeF(120f, 52f),
            () =>
            {
                dialogEntity.Attach(new ButtonEventComponent
                {
                    Event = ButtonEvents.ShowScoreBoard
                });
            });
        
        scoreBoardButton.Get<ButtonComponent>().FocusOrderId = 2;
        
        dialog.ChildrenEntities.Add(scoreBoardButton.Id);

        var creditsButton = CreateButton(
            "Credits",
            new Vector2(740f, 270f),
            new SizeF(120f, 52f),
            () =>
            {
                dialogEntity.Attach(new ButtonEventComponent
                {
                    Event = ButtonEvents.ShowCredits
                });
            });
        
        creditsButton.Get<ButtonComponent>().FocusOrderId = 3;
        
        dialog.ChildrenEntities.Add(creditsButton.Id);

        var quitButton = CreateButton(
            "Quit",
            new Vector2(740f, 330f),
            new SizeF(120f, 52f),
            () =>
            {
                dialogEntity.Attach(new ButtonEventComponent
                {
                    Event = ButtonEvents.Exit
                });
            });
        
        quitButton.Get<ButtonComponent>().FocusOrderId = 4;
        
        dialog.ChildrenEntities.Add(quitButton.Id);
        
        var label1 = world.CreateEntity();
        label1.Attach(new DialogLabelComponent
        {
            Text = "Yet another",
            Font = contents.BigFont,
            Color = Colors.ScoreTimeColor
        });
        label1.Attach(new TransformComponent
        {
            Position = new Vector2(
                Constants.VirtualScreenWidth / 2f - 120f,
                Constants.VirtualScreenHeight / 2f - 100f)
        });

        dialog.ChildrenEntities.Add(label1.Id);
        
        var label2 = world.CreateEntity();
        label2.Attach(new DialogLabelComponent
        {
            Text = "Snake",
            Font = contents.LogoFont
        });
        label2.Attach(new TransformComponent
        {
            Position = new Vector2(
                Constants.VirtualScreenWidth / 2f - 160f,
                Constants.VirtualScreenHeight / 2f - 70f)
        });
        
        dialog.ChildrenEntities.Add(label2.Id);
        
        var label3 = world.CreateEntity();
        label3.Attach(new DialogLabelComponent
        {
            Text = "Game",
            Font = contents.LogoFont
        });
        label3.Attach(new TransformComponent
        {
            Position = new Vector2(
                Constants.VirtualScreenWidth / 2f - 130f,
                Constants.VirtualScreenHeight / 2f - 0f)
        });
        
        dialog.ChildrenEntities.Add(label3.Id);
    }

    public void CreateCreditsDialog()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        var content = new StringBuilder()
            .AppendLine($"Yet another Snake Game ({version})")
            .AppendLine()
            .AppendLine("Developed by: g1ngercat.itch.io")
            .AppendLine("Art too...")
            .AppendLine("Audio effects: sfxr")
            .AppendLine()
            .AppendLine("Font: Pixel Operator")
            .AppendLine("Game engine based on MonoGame")
            .ToString();

        CreateDialog(
            "Credits",
            content,
            new SizeF(310f, 260f),
            ("Back", ButtonEvents.Close));
    }

    public void CreateScoreBoardDialog()
    {
        var dataManager = new DataManager();
        var scoreBoard = dataManager.LoadScoreBoard();

        var resultBuilder = new StringBuilder()
            .Append($"{"Date",14}")
            .Append($"{"Score",14}")
            .Append($"{"Time (s)",14}")
            .AppendLine();

        foreach (var entry in scoreBoard.Entries)
        {
            resultBuilder
                .Append($"{entry.CreatedAt.ToShortDateString(),14}")
                .Append($"{entry.Score.ToString(Constants.ScoreFormat),14}")
                .Append($"{entry.TimePlayed.ToString(),8}")
                .AppendLine();
        }

        CreateDialog(
            "Score Board",
            resultBuilder.ToString(),
            new SizeF(300f, 400f),
            ("Back", ButtonEvents.Close));
    }

    public void CreatePauseDialog()
    {
        CreateDialog(
            "Paused",
            "Your game is paused",
            new SizeF(220, 140),
            ("Resume", ButtonEvents.Resume),
            ("Exit", ButtonEvents.ShowStartScreen));
    }

    public void CreateGameOverDialog(GameState gameState)
    {
        var results = new StringBuilder()
            .AppendLine("Your results:")
            .AppendLine($"Score: {gameState.Score}")
            .AppendLine($"Deaths: {gameState.Deaths}")
            .AppendLine($"Longest snake: {gameState.LongestSnake}")
            .AppendLine($"Time played: {(int)gameState.TotalTime}s")
            .ToString();

        CreateDialog(
            "Game is over",
            results,
            new SizeF(250f, 300f),
            ("Score Board", ButtonEvents.ShowScoreBoard),
            ("Exit", ButtonEvents.ShowStartScreen));
    }

    public void CreateNavigationIntent()
    {
        var entity = world.CreateEntity();
        entity.Attach(new NavigationIntentComponent
        {
            Event = NavigationEvent.None
        });
    }

    public void CreateLevelBonusDialog()
    {
        var dialogEntity = world.CreateEntity();

        var dialog = new DialogComponent
        {
            Title = "Select bonus",
            Size = new SizeF(250f, 180f)
        };
        
        dialogEntity.Attach(dialog);
        
        var transform = new TransformComponent
        {
            Position = GetDialogCenterPosition(dialog)
        };
        dialogEntity.Attach(transform);

        var buttons = new [] {
            new { Text = "+Time", Event = ButtonEvents.AddTime },
            new { Text = "+Invincibility", Event = ButtonEvents.AddInvincibility },
            new { Text = "-Enemies", Event = ButtonEvents.DestroyEnemies },
            new { Text = "+Diamond Spawn Rate", Event = ButtonEvents.AddDiamondSpawnRate },
            new { Text = "+Score Multiplicator", Event = ButtonEvents.AddScoreMultiplicator }
        };
        
        var randomButtons = buttons
            .OrderBy(x => Random.Shared.Next()) // Shuffle randomly
            .Take(3)
            .ToArray();

        var buttonPositionY = 30f;
        var focusOrderId = 1;
        
        foreach (var button in randomButtons)
        {
            var buttonEntity = CreateButton(
                button.Text,
                new Vector2(transform.Position.X + 10f, transform.Position.Y + buttonPositionY),
                new SizeF(dialog.Size.Width - 20f, 40f),
                () =>
                {
                    dialogEntity.Attach(new ButtonEventComponent
                    {
                        DialogEntityId = dialogEntity.Id,
                        Event = button.Event
                    });
                });

            buttonEntity.Get<ButtonComponent>().FocusOrderId = focusOrderId;

            dialog.ChildrenEntities.Add(buttonEntity.Id);

            buttonPositionY += 50f;
            focusOrderId++;
        }
    }

    private Entity CreateButton(string text, Vector2 position, SizeF size, Action action)
    {
        var entity = world.CreateEntity();
        
        entity.Attach(new ButtonComponent
        {
            Text = text,
            Size = size,
            Action = action
        });
        
        entity.Attach(new TransformComponent
        {
            Position = position
        });
        
        return entity;
    }

    private void CreateDialog(string title, string content, SizeF size, params (string, ButtonEvents)[] buttons)
    {
        var dialogEntity = world.CreateEntity();

        var dialog = new DialogComponent
        {
            Title = title,
            Content = content,
            Size = size
        };
        dialogEntity.Attach(dialog);

        var transform = new TransformComponent
        {
            Position = GetDialogCenterPosition(dialog)
        };
        dialogEntity.Attach(transform);
        
        var totalButtonWidth = buttons.Length * 100f + (buttons.Length - 1) * 4f;
            
        var buttonPositionX = (dialog.Size.Width - totalButtonWidth) / 2f;
        var buttonPositionY = dialog.Size.Height - 46f;
        var buttonFocusOrderId = 1;

        foreach (var button in buttons)
        {
            var buttonEntity = world.CreateEntity();
            
            buttonEntity.Attach(new ButtonComponent
            {
                Text = button.Item1,
                Size = new SizeF(100f, 40f),
                FocusOrderId = buttonFocusOrderId,
                Action = () =>
                {
                    dialogEntity.Attach(new ButtonEventComponent
                    {
                        DialogEntityId = dialogEntity.Id,
                        Event = button.Item2
                    });
                }
            });
            
            buttonEntity.Attach(new TransformComponent
            {
                Position = transform.Position + new Vector2(buttonPositionX, buttonPositionY)
            });

            buttonPositionX += 100f;
            buttonPositionX += 4f;
            buttonFocusOrderId++;
            
            dialog.ChildrenEntities.Add(buttonEntity.Id);
        }
    }

    private static Vector2 GetDialogCenterPosition(DialogComponent dialog)
    {
        return new Vector2(
            (Constants.VirtualScreenWidth - dialog.Size.Width) / 2,
            (Constants.VirtualScreenHeight - dialog.Size.Height) / 2);
    }
}