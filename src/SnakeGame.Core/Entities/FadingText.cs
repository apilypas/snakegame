using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Core.Entities;

public class FadingText : Entity
{
    private float _timeToLive;
    
    public FadingText(string text, SpriteFont font, float timeToLive = 1f)
    {
        _timeToLive = timeToLive;
        
        Children.Add(new Label
        {
            Font = font,
            Text = text,
            Color = Color.Yellow
        });
    }

    public override void Update(GameTime gameTime)
    {
        var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        _timeToLive -= elapsed;
        Position += new Vector2(0f, -elapsed * 30f);

        if (_timeToLive <= 0f)
        {
            QueueRemove = true;
        }
    }
}