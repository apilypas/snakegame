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
        var entity = world.CreateEntity();
        
        var scoreLabelId = CreateLabel(contents.BigFont, string.Empty, Colors.DefaultTextColor);
        world.GetEntity(scoreLabelId).Get<TransformComponent>().Position = new Vector2(744f, 38f);
        
        var multiplicatorLabelId = CreateLabel(contents.MainFont, string.Empty, Colors.ScoreMultiplicatorColor);
        world.GetEntity(multiplicatorLabelId).Get<TransformComponent>().Position = new Vector2(900f, 64f);
        
        var timeLabelId = CreateLabel(contents.MainFont, string.Empty, Colors.ScoreTimeColor);
        world.GetEntity(timeLabelId).Get<TransformComponent>().Position = new Vector2(764f, 29f);
        
        var clockSpriteId = CreateSprite(contents.CollectableTexture, new Rectangle(16, 0, 16, 16));
        world.GetEntity(clockSpriteId).Get<TransformComponent>().Position = new Vector2(746f, 32f);

        entity.Attach(new ScoreDisplayComponent
        {
            ScoreLabelId = scoreLabelId,
            MultiplicatorLabelId = multiplicatorLabelId,
            TimeLabelId = timeLabelId
        });
    }
    
    public void CreateKeybindsDisplay()
    {
        List<KeyValuePair<string, string>> inputBindings = [
            new("Pause", "Esc"),
            new("Move up", "W"),
            new("Move down", "S"),
            new("Move left", "A"),
            new("Move right", "D"),
            new("Faster", "J")
        ];

        var p = 36f;
        foreach (var inputBinding in inputBindings)
        {
            {
                // Binding full name
                var size = contents.MainFont.MeasureString(inputBinding.Key);
                var x = 100 - size.X;
                var y = (32 - size.Y) / 2f - 2;
                var id1 = CreateLabel(contents.MainFont, inputBinding.Key,  Colors.DefaultTextColor);
                world.GetEntity(id1).Get<TransformComponent>().Position = new Vector2(76f + x, p + y);
            }

            {
                // Binding button sprite
                var id2 = CreateSprite(contents.CollectableTexture, new Rectangle(32, 0, 32, 32));
                world.GetEntity(id2).Get<TransformComponent>().Position = new Vector2(180f, p);
            }

            {
                // Binding key value
                var size = contents.MainFont.MeasureString(inputBinding.Value);
                var x = (32 - size.X) / 2f;
                var y = (32 - size.Y) / 2f - 2;
                var id3 = CreateLabel(contents.MainFont, inputBinding.Value, Colors.DefaultTextColor);
                world.GetEntity(id3).Get<TransformComponent>().Position = new Vector2(180f + x, p + y);
            }

            p += 40f;
        }
    }

    public void CreateLevelDisplay()
    {
        var entity = world.CreateEntity();

        entity.Attach(new HudLevelDisplayComponent());
        
        entity.Attach(new TransformComponent
        {
            Position = new Vector2(744f, 100f)
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