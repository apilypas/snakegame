using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.DesktopGL.Core.Sprites;

public class TextSprite
{
    protected readonly SpriteFont _spriteFont;

    public Vector2 Location { get; set; } = Vector2.Zero;
    public string Text { get; set; }

    private TextSprite() { }

    public TextSprite(SpriteFont spriteFont)
    {
        _spriteFont = spriteFont;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(
            _spriteFont,
            Text,
            Location,
            Color.White,
            0,
            _spriteFont.MeasureString(Text) / 2f,
            1f,
            SpriteEffects.None,
            0f);
    }
}