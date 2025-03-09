using Microsoft.Xna.Framework;

namespace SnakeGame.DesktopGL.Core.Renderers.Animations;

public class JumpingAnimation
{
    private const float MoveByY = 1f;
    private const float Duration = .1f;
        
    private float _timer = 0f;
    private float _offsetY = 0f;
    private bool _isInverted = false;
        
    public void Update(GameTime gameTime)
    {
        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_timer < Duration)
            return;

        if (_isInverted)
            _offsetY += MoveByY;
        else
            _offsetY -= MoveByY;

        if (_offsetY is < -5f or > 0f)
            _isInverted = !_isInverted;

        _timer -= Duration;
    }

    public Vector2 AdjustVector => new(0f, _offsetY);
}