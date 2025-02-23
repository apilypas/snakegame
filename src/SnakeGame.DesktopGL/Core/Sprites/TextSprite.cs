using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.DesktopGL.Core.Sprites;

public class TextSprite
{
    private bool _isLoaded = false;

    public SpriteFont Font { get; private set; }
    public Vector2 Location { get; private set; } = Vector2.Zero;
    public string Text { get; private set; } = string.Empty;

    private TextSprite()
    {
    }

    public static TextSprite Create()
    {
        return new TextSprite();
    }

    public static TextSprite Create(string text)
    {
        return new TextSprite().WithText(text);
    }

    public TextSprite WithText(string text)
    {
        Text = text;
        return this;
    }

    public TextSprite Load(ContentManager content, string fontName)
    {
        Font = content.Load<SpriteFont>(fontName);
        _isLoaded = true;
        return this;
    }
    
    private void ThrowIfLoaded()
    {
        if (_isLoaded)
            throw new Exception("Sprite is already loaded");
    }
}