using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Core.Entities;

public class Collectable : Entity
{
    private readonly Texture2D _texture;
    
    private const float MoveByY = 1f;
    private const float Duration = .1f;
        
    private float _timer;
    private float _offsetY;
    private bool _isInverted;

    public CollectableType Type { get; }
    public Sprite Sprite { get; }

    public Collectable(Texture2D texture, CollectableType type)
    {
        _texture = texture;
        Type = type;
        Sprite = CreateSprite(Type);
        
        AddChild(Sprite);
    }

    public override void Update(GameTime gameTime)
    {
        UpdateHoverEffect(gameTime);
    }

    private void UpdateHoverEffect(GameTime gameTime)
    {
        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_timer >= Duration)
        {

            if (_isInverted)
                _offsetY += MoveByY;
            else
                _offsetY -= MoveByY;

            if (_offsetY is < -5f or > 0f)
                _isInverted = !_isInverted;

            Sprite.Position = new Vector2(0, _offsetY);

            _timer -= Duration;
        }
    }

    public Rectangle GetRectangle()
    {
        return new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            Constants.SegmentSize,
            Constants.SegmentSize);
    }

    private Sprite CreateSprite(CollectableType type)
    {
        return type switch
        {
            CollectableType.Diamond => new Sprite
            {
                Texture = _texture,
                SourceRectangle = new Rectangle(0, 32, 16, 16)
            },
            CollectableType.SnakePart => new Sprite
            {
                Texture = _texture,
                SourceRectangle = new Rectangle(0, 16, 16, 16)
            },
            CollectableType.SpeedBoost => new Sprite
            {
                Texture = _texture,
                SourceRectangle = new Rectangle(0, 0, 16, 16)
            },
            CollectableType.Clock => new Sprite
            {
                Texture = _texture,
                SourceRectangle = new Rectangle(16, 0, 16, 16)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}
