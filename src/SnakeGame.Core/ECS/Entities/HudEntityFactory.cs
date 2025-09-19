using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Graphics;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Services;

namespace SnakeGame.Core.ECS.Entities;

public class HudEntityFactory(World world, GameContentManager contents)
{
    public void CreateScoreDisplay()
    {
        const float x = Constants.VirtualScreenWidth / 2f 
                        + Constants.WallWidth * Constants.SegmentSize / 2f
                        - 36f;

        const float y = Constants.VirtualScreenHeight / 2f
                        - Constants.WallHeight * Constants.SegmentSize / 2f
                        + 10f;
        
        var entity = world.CreateEntity();
        
        var scoreLabelId = CreateLabel(contents.BigFont, string.Empty, Colors.DefaultTextColor);
        world.GetEntity(scoreLabelId).Get<TransformComponent>().Position = new Vector2(x, y + 14f);
        
        var multiplicatorLabelId = CreateLabel(contents.MainFont, string.Empty, Colors.ScoreMultiplicatorColor);
        world.GetEntity(multiplicatorLabelId).Get<TransformComponent>().Position = new Vector2(x + 66f, y + 32f);
        
        var timeLabelId = CreateLabel(contents.MainFont, string.Empty, Colors.ScoreTimeColor);
        world.GetEntity(timeLabelId).Get<TransformComponent>().Position = new Vector2(x + 20f, y - 2f);
        
        var clockSpriteId = CreateSprite(contents.CollectableTexture, new Rectangle(16, 0, 16, 16));
        world.GetEntity(clockSpriteId).Get<TransformComponent>().Position = new Vector2(x, y - 2f);

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
                        + Constants.WallWidth * Constants.SegmentSize / 2f
                        - 42f;

        const float y = Constants.VirtualScreenHeight / 2f
                        - Constants.WallHeight * Constants.SegmentSize / 2f
                        + 120f;
        
        List<KeyValuePair<string, string>> inputBindings = [
            new("Pause", "ESC"),
            new("Up", "W"),
            new("Down", "S"),
            new("Left", "A"),
            new("Right", "D"),
            new("Faster", "J")
        ];

        var offsetY = y;
        foreach (var inputBinding in inputBindings)
        {
            // Binding full name
            var namePosSize = contents.MainFont.MeasureString(inputBinding.Key);
            var namePosY = (36f - namePosSize.Y) / 2f - 2;
            var id1 = CreateLabel(contents.MainFont, inputBinding.Key,  Colors.DefaultTextColor);
            world.GetEntity(id1).Get<TransformComponent>().Position = new Vector2(
                x + 36f,
                offsetY + namePosY);

            // Binding button sprite
            var id2 = CreateSprite(contents.CollectableTexture, new Rectangle(32, 0, 32, 32));
            world.GetEntity(id2).Get<TransformComponent>().Position = new Vector2(
                x, 
                offsetY);

            // Binding key value
            var valuePosSize = contents.MainFont.MeasureString(inputBinding.Value);
            var valuePosX = (40f - valuePosSize.X) / 2f - 4f;
            var valuePosY = (36f - valuePosSize.Y) / 2f - 4f;
            var id3 = CreateLabel(contents.MainFont, inputBinding.Value, Colors.DefaultTextColor);
            world.GetEntity(id3).Get<TransformComponent>().Position = new Vector2(
                x + valuePosX, 
                offsetY + valuePosY);

            offsetY += 32f;
        }
    }

    public void CreateLevelDisplay()
    {
        const float x = Constants.VirtualScreenWidth / 2f 
                        + Constants.WallWidth * Constants.SegmentSize / 2f
                        - 36f;

        const float y = Constants.VirtualScreenHeight / 2f
                        - Constants.WallHeight * Constants.SegmentSize / 2f
                        + 64f;
        
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