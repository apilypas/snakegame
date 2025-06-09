using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.Renderers;

public class FadeOutTextRenderer(IList<FadeOutText> fadeOutTexts) : RendererBase
{
    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        foreach (var fadeOutText in fadeOutTexts)
        {
            spriteBatch.DrawStringWithShadow(
                fadeOutText.Font,
                fadeOutText.Text,
                fadeOutText.Location + Globals.PlayFieldOffset,
                Colors.DefaultTextColor);
        }
    }
}