using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.DesktopGL.Core.Sprites;

public class TextureSprite
{
    private bool _isLoaded = false;

    public Texture2D Texture { get; private set; }
    public Vector2 Location { get; private set; } = Vector2.Zero;
    public Vector2 Origin { get; private set; } = Vector2.Zero;
    public Rectangle SourceRectangle { get; private set; } = Rectangle.Empty;
    public float Rotation { get; private set; } = 0f;
    public SpriteEffects Effects { get; private set; } = SpriteEffects.None;

    private TextureSprite() {}
    
    public TextureSprite(Rectangle? sourceRectangle = null)
    {
        if (sourceRectangle != null)
            SourceRectangle = sourceRectangle.Value;
    }

    public TextureSprite WithEffects(SpriteEffects effects)
    {
        ThrowIfLoaded();
        Effects = effects;
        return this;
    }

    public TextureSprite WithRotation(float rotation)
    {
        ThrowIfLoaded();
        Rotation = rotation;
        return this;
    }

    public TextureSprite Load(ContentManager content, string assetName)
    {
        Texture = content.Load<Texture2D>(assetName);
        
        if (SourceRectangle.IsEmpty)
            SourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
        
        if (Origin == Vector2.Zero)
            Origin = new Vector2(SourceRectangle.Width / 2, SourceRectangle.Height / 2);

        _isLoaded = true;
        
        return this;
    }

    private void ThrowIfLoaded()
    {
        if (_isLoaded)
            throw new Exception("Sprite is already loaded");
    }
}
