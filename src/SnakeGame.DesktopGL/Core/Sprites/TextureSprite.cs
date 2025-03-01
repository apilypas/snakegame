using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.DesktopGL.Core.Sprites;

public class TextureSprite
{
    public Texture2D Texture { get; private set; }

    public Rectangle SourceRectangle { get; private set; } = Rectangle.Empty;
    
    public Vector2 Location { get; set; } = Vector2.Zero;
    public Vector2 Origin { get; set; } = Vector2.Zero;
    public float Rotation { get; set; } = 0f;
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;
    public float Scale { get; set; } = 1f;
    public float LayerDepth { get; set; } = 0f;
    public Color Color { get; set; } = Color.White;

    private TextureSprite() {}

    public static TextureSprite Create()
    {
        return new TextureSprite();
    }

    public static TextureSprite Create(Rectangle sourceRectangle)
    {
        return new TextureSprite()
        {
            SourceRectangle = sourceRectangle
        };
    }
    
    public TextureSprite Load(ContentManager content, string assetName)
    {
        Texture = content.Load<Texture2D>(assetName);
        
        if (SourceRectangle.IsEmpty)
            SourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
        
        if (Origin == Vector2.Zero)
            Origin = new Vector2(SourceRectangle.Width / 2, SourceRectangle.Height / 2);

        return this;
    }
}
