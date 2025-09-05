using System;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Services;

namespace SnakeGame.Core.ECS.Entities;

public class DialogEntityFactory(World world, ContentManager contents)
{
    public int CreateStartScreen()
    {
        var dialogEntity = world.CreateEntity();
        
        var dialog = new DialogComponent
        {
            Size = new SizeF(Constants.VirtualScreenWidth, Constants.VirtualScreenHeight),
            IsTransparent = true
        };

        dialogEntity.Attach(dialog);

        var startButtonId = CreateButton(
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
        
        dialog.ChildrenEntities.Add(startButtonId);

        var scoreBoardButtonId = CreateButton(
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
        
        dialog.ChildrenEntities.Add(scoreBoardButtonId);

        var creditsButtonId = CreateButton(
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
        
        dialog.ChildrenEntities.Add(creditsButtonId);

        var quitButtonId = CreateButton(
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
        
        dialog.ChildrenEntities.Add(quitButtonId);
        
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

        return dialogEntity.Id;
    }

    public int CreateCreditsDialog()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        var content = new StringBuilder()
            .AppendLine($"Yet another Snake Game ({version})")
            .AppendLine()
            .AppendLine("Developed by: g1ngercat.itch.io")
            .AppendLine("Music and art too...")
            .AppendLine()
            .AppendLine("Font: Pixel Operator")
            .AppendLine("Game engine based on MonoGame")
            .ToString();

        var dialogId = CreateDialog(
            "Credits",
            content,
            new SizeF(310f, 260f),
            ("Back", ButtonEvents.Close));

        return dialogId;
    }

    public int CreateScoreBoardDialog()
    {
        var dataManager = new DataManager();
        var scoreBoard = dataManager.LoadScoreBoard();

        var resultBuilder = new StringBuilder()
            .AppendFormat("{0,14}", "Date")
            .AppendFormat("{0,14}", "Score")
            .AppendFormat("{0,14}", "Time (s)")
            .AppendLine();

        foreach (var entry in scoreBoard.Entries)
        {
            resultBuilder
                .AppendFormat("{0,14}", entry.CreatedAt.ToShortDateString())
                .AppendFormat("{0,14}", entry.Score.ToString(Constants.ScoreFormat))
                .AppendFormat("{0,8}", entry.TimePlayed.ToString())
                .AppendLine();
        }

        var dialogId = CreateDialog(
            "Score Board",
            resultBuilder.ToString(),
            new SizeF(300f, 400f),
            ("Back", ButtonEvents.Close));

        return dialogId;
    }
    
    public int CreateButton(string text, Vector2 position, SizeF size, Action action)
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
        
        return entity.Id;
    }
    
    public int CreateDialog(string title, string content, SizeF size, params (string, ButtonEvents)[] buttons)
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
            Position = new Vector2(
                (Constants.VirtualScreenWidth - size.Width) / 2,
                (Constants.VirtualScreenHeight - size.Height) / 2)
        };
        dialogEntity.Attach(transform);
        
        var totalButtonWidth = buttons.Length * 100f + (buttons.Length - 1) * 4f;
            
        var buttonPositionX = (dialog.Size.Width - totalButtonWidth) / 2f;
        var buttonPositionY = dialog.Size.Height - 46f;

        foreach (var button in buttons)
        {
            var buttonEntity = world.CreateEntity();
            
            buttonEntity.Attach(new ButtonComponent
            {
                Text = button.Item1,
                Size = new SizeF(100f, 40f),
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
            
            dialog.ChildrenEntities.Add(buttonEntity.Id);
        }

        return dialogEntity.Id;
    }
}