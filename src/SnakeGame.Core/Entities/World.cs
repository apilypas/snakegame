using Microsoft.Xna.Framework;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Entities;

public class World : Entity
{
    public ScoreDisplay Score { get; }
    public PlayField PlayField { get; }
    
    public World(AssetManager assets)
    {
        PlayField = new PlayField
        {
            TiledMap = assets.TiledMap,
            Position = Globals.PlayFieldOffset
        };
        
        Children.Add(PlayField);
        
        Score = new ScoreDisplay(assets)
        {
            Position = new Vector2(GameManager.GetRectangle().Width + 16f, 16f) 
                       + Globals.PlayFieldOffset
        };
        
        Children.Add(Score);
    }
}