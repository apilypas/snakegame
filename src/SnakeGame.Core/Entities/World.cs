using Microsoft.Xna.Framework;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Entities;

public class World : Entity
{
    public ScoreDisplay Score { get; }
    public PlayField PlayField { get; }
    public Entity FadingTextLayer { get; }
    public Entity DialogLayer { get; }
    public PlayerSnake PlayerSnake { get; set; }
    
    public World(AssetManager assets, EventBus eventBus)
    {
        PlayField = new PlayField();
        PlayField.Position = Globals.PlayFieldOffset;
        PlayField.TilesTexture = assets.TilesTexture;
        PlayField.Initialize();
        
        AddChild(PlayField);

        FadingTextLayer = new Entity
        {
            Position = Globals.PlayFieldOffset
        };
        
        AddChild(FadingTextLayer);
        
        Score = new ScoreDisplay(eventBus, assets.CollectableTexture)
        {
            Position = new Vector2(Globals.PlayFieldRectangle.Width + 16f, 16f) 
                       + Globals.PlayFieldOffset
        };
        
        AddChild(Score);
        
        AddChild(new InputBindingDisplay(assets, "Pause", "Esc")
        {
            Position = new Vector2(Globals.PlayFieldOffset.X - 160f, Globals.PlayFieldOffset.Y)
        });
        AddChild(new InputBindingDisplay(assets, "Move up", "W")
        {
            Position = new Vector2(Globals.PlayFieldOffset.X - 160f, Globals.PlayFieldOffset.Y + 40f)
        });
        AddChild(new InputBindingDisplay(assets, "Move down", "S")
        {
            Position = new Vector2(Globals.PlayFieldOffset.X - 160f, Globals.PlayFieldOffset.Y + 80f)
        });
        AddChild(new InputBindingDisplay(assets, "Move left", "A")
        {
            Position = new Vector2(Globals.PlayFieldOffset.X - 160f, Globals.PlayFieldOffset.Y + 120f)
        });
        AddChild(new InputBindingDisplay(assets, "Move right", "D")
        {
            Position = new Vector2(Globals.PlayFieldOffset.X - 160f, Globals.PlayFieldOffset.Y + 160f)
        });
        AddChild(new InputBindingDisplay(assets, "Faster", "Spc")
        {
            Position = new Vector2(Globals.PlayFieldOffset.X - 160f, Globals.PlayFieldOffset.Y + 200f)
        });

        DialogLayer = new Entity();
        AddChild(DialogLayer);
    }
}