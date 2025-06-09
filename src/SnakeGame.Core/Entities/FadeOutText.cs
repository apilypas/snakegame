using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Entities;

public class FadeOutText : Entity
{
    public string Text { get; private set; }
    public float TimeToLive { get; set; }
    public SpriteFont Font { get; private set; }

    public FadeOutText(AssetManager assets, string text, float timeToLive = 1f)
    {
        Text = text;
        TimeToLive = timeToLive;
        Font = assets.MainFont;
    }
}