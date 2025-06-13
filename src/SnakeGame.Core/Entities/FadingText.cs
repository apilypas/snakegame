using Microsoft.Xna.Framework;

namespace SnakeGame.Core.Entities;

public class FadingText : Entity
{
    private float _timeToLive;
    
    public FadingText(string text, float timeToLive = 1f)
    {
        _timeToLive = timeToLive;
        
        AddChild(new Label
        {
            Text = text
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