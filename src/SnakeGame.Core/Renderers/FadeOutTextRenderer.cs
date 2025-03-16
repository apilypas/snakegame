using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.Renderers;

public class FadeOutTextRenderer(IList<FadeOutText> fadeOutTexts) : RendererBase
{
    private SpriteFont _font;
    
    public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _font = content.Load<SpriteFont>("MainFont");
    }

    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        foreach (var fadeOutText in fadeOutTexts)
        {
            spriteBatch.DrawStringWithShadow(
                _font,
                fadeOutText.Text,
                fadeOutText.Location + Globals.PlayFieldOffset,
                Colors.DefaultTextColor);
        }
    }

    public override void Update(GameTime gameTime)
    {
    }
}