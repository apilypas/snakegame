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
        PlayField = new PlayField
        {
            TiledMap = assets.TiledMap,
            Position = Globals.PlayFieldOffset
        };
        
        AddChild(PlayField);

        FadingTextLayer = new Entity
        {
            Position = Globals.PlayFieldOffset
        };
        
        AddChild(FadingTextLayer);
        
        Score = new ScoreDisplay(eventBus)
        {
            Position = new Vector2(Globals.PlayFieldRectangle.Width + 16f, 16f) 
                       + Globals.PlayFieldOffset
        };
        
        AddChild(Score);

        DialogLayer = new Entity();
        AddChild(DialogLayer);
    }
}