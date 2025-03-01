using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.DesktopGL.Core.Sprites;

public class TextSprite
{
    public SpriteFont Font { get; private set; }

    public Vector2 Location { get; set; } = Vector2.Zero;
    public string Text { get; set; } = string.Empty;
    public float Scale { get; set; } = 1f;
    public float LayerDepth { get; set; } = 0f;
    public Color Color { get; set; } = Color.White;
    public float Rotation { get; set; } = 0f;

    private TextSprite()
    {
    }

    public static TextSprite Create()
    {
        return new TextSprite();
    }

    public TextSprite Load(ContentManager content, string fontName)
    {
        Font = content.Load<SpriteFont>(fontName);
        return this;
    }
}