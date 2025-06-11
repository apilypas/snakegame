using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace SnakeGame.Core.Entities;

public class Collectable : Entity
{
    private readonly Texture2D _texture;
    
    private const float MoveByY = 1f;
    private const float Duration = .1f;
        
    private float _timer = 0f;
    private float _offsetY = 0f;
    private bool _isInverted = false;

    public CollectableType Type { get; }
    public Sprite2 Sprite { get; }

    public Collectable(Texture2D texture, CollectableType type)
    {
        _texture = texture;
        Type = type;
        Sprite = CreateSprite(Type);
        
        Children.Add(Sprite);
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

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.DrawRectangle(
            new RectangleF(GlobalPosition, new SizeF(16, 16)),
            Color.Blue);
    }

    private Sprite2 CreateSprite(CollectableType type)
    {
        return type switch
        {
            CollectableType.Diamond => new Sprite2
            {
                Texture = _texture,
                SourceRectangle = new Rectangle(0, 32, 16, 16)
            },
            CollectableType.SnakePart => new Sprite2
            {
                Texture = _texture,
                SourceRectangle = new Rectangle(0, 16, 16, 16)
            },
            CollectableType.SpeedBoost => new Sprite2
            {
                Texture = _texture,
                SourceRectangle = new Rectangle(0, 0, 16, 16)
            },
            CollectableType.Clock => new Sprite2
            {
                Texture = _texture,
                SourceRectangle = new Rectangle(16, 0, 16, 16)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}
