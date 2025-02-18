using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame;

public class Sprite
{
    protected readonly Texture2D _texture;
    protected readonly Rectangle? _sourceRectangle;

    public Vector2 Location { get; set; } = Vector2.Zero;
    public float Rotation { get; set; } = 0f;

    private Sprite() { }

    public Sprite(Texture2D texture, Rectangle? sourceRectangle = null)
    {
        _texture = texture;
        _sourceRectangle = sourceRectangle;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (Rotation == 0f)
        {
            spriteBatch.Draw(_texture, Location, GetSourceRectangle(), Color.White);
        }
        else
        {
            var sourceRectangle = GetSourceRectangle();
            var width = (float)sourceRectangle.Width;
            var height = (float)sourceRectangle.Height;

            var originLocation = new Vector2(Location.X + width / 2f, Location.Y + height / 2f);

            var origin = new Vector2(width / 2f, height / 2f);

            spriteBatch.Draw(
                _texture,
                originLocation,
                sourceRectangle,
                Color.White,
                Rotation,
                origin,
                1f,
                SpriteEffects.None,
                0f);
        }
    }

    protected virtual Rectangle GetSourceRectangle()
    {
        if (_sourceRectangle == null)
            return new Rectangle(0, 0, _texture.Width, _texture.Height);

        return _sourceRectangle.Value;
    }
}
