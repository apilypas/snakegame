using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Graphics;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Services;

namespace SnakeGame.Core.ECS.Entities;

public class HudEntityFactory(World world, ContentManager contents)
{
    public void CreateScoreDisplay()
    {
        const float x = Constants.VirtualScreenWidth / 2f 
                        + Constants.WallWidth * Constants.SegmentSize / 2f
                        + 20f;

        const float y = Constants.VirtualScreenHeight / 2f
                        - Constants.WallHeight * Constants.SegmentSize / 2f
                        + 20f;
        
        var entity = world.CreateEntity();
        
        var scoreLabelId = CreateLabel(contents.BigFont, string.Empty, Colors.DefaultTextColor);
        world.GetEntity(scoreLabelId).Get<TransformComponent>().Position = new Vector2(x, y + 9f);
        
        var multiplicatorLabelId = CreateLabel(contents.MainFont, string.Empty, Colors.ScoreMultiplicatorColor);
        world.GetEntity(multiplicatorLabelId).Get<TransformComponent>().Position = new Vector2(x + 156f, y + 35f);
        
        var timeLabelId = CreateLabel(contents.MainFont, string.Empty, Colors.ScoreTimeColor);
        world.GetEntity(timeLabelId).Get<TransformComponent>().Position = new Vector2(x + 26f, y - 2f);
        
        var clockSpriteId = CreateSprite(contents.CollectableTexture, new Rectangle(20, 0, 20, 20));
        world.GetEntity(clockSpriteId).Get<TransformComponent>().Position = new Vector2(x + 2f, y - 2f);

        entity.Attach(new ScoreDisplayComponent
        {
            ScoreLabelId = scoreLabelId,
            MultiplicatorLabelId = multiplicatorLabelId,
            TimeLabelId = timeLabelId
        });
    }
    
    public void CreateKeybindsDisplay()
    {
        const float x = Constants.VirtualScreenWidth / 2f 
                        - Constants.WallWidth * Constants.SegmentSize / 2f
                        - 160f;

        const float y = Constants.VirtualScreenHeight / 2f
                        - Constants.WallHeight * Constants.SegmentSize / 2f
                        + 20f;
        
        List<KeyValuePair<string, string>> inputBindings = [
            new("Pause", "Esc"),
            new("Move up", "W"),
            new("Move down", "S"),
            new("Move left", "A"),
            new("Move right", "D"),
            new("Faster", "J")
        ];

        var offsetY = y;
        foreach (var inputBinding in inputBindings)
        {
            // Binding full name
            var namePosSize = contents.MainFont.MeasureString(inputBinding.Key);
            var namePosX = 100f - namePosSize.X;
            var namePosY = (36f - namePosSize.Y) / 2f - 2;
            var id1 = CreateLabel(contents.MainFont, inputBinding.Key,  Colors.DefaultTextColor);
            world.GetEntity(id1).Get<TransformComponent>().Position = new Vector2(
                x + namePosX,
                offsetY + namePosY);

            // Binding button sprite
            var id2 = CreateSprite(contents.CollectableTexture, new Rectangle(40, 0, 40, 40));
            world.GetEntity(id2).Get<TransformComponent>().Position = new Vector2(
                x + 104f, 
                offsetY);

            // Binding key value
            var valuePosSize = contents.MainFont.MeasureString(inputBinding.Value);
            var valuePosX = (40f - valuePosSize.X) / 2f;
            var valuePosY = (36f - valuePosSize.Y) / 2f - 2;
            var id3 = CreateLabel(contents.MainFont, inputBinding.Value, Colors.DefaultTextColor);
            world.GetEntity(id3).Get<TransformComponent>().Position = new Vector2(
                x + 104f + valuePosX, 
                offsetY + valuePosY);

            offsetY += 40f;
        }
    }

    public void CreateLevelDisplay()
    {
        const float x = Constants.VirtualScreenWidth / 2f 
                        + Constants.WallWidth * Constants.SegmentSize / 2f
                        + 20f;

        const float y = Constants.VirtualScreenHeight / 2f
                        - Constants.WallHeight * Constants.SegmentSize / 2f
                        + 90f;
        
        var entity = world.CreateEntity();

        entity.Attach(new HudLevelDisplayComponent());
        
        entity.Attach(new TransformComponent
        {
            Position = new Vector2(x, y)
        });
    }

    private int CreateLabel(SpriteFont spriteFont, string text, Color color)
    {
        var entity = world.CreateEntity();

        entity.Attach(new HudLabelComponent
        {
            Font = spriteFont,
            Text = text,
            Color = color
        });
        
        entity.Attach(new TransformComponent());
        
        return entity.Id;
    }

    private int CreateSprite(Texture2D texture, Rectangle source)
    {
        var entity = world.CreateEntity();
        
        entity.Attach(new HudSpriteComponent
        {
            Sprite = new Sprite(new Texture2DRegion(texture, source))
        });
        
        entity.Attach(new TransformComponent());
        
        return entity.Id;
    }
}