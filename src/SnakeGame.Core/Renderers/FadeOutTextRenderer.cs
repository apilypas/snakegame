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
    private PlayFieldOffsetHandler _playFieldOffsetHandler;
    
    public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _font = content.Load<SpriteFont>("MainFont");
        
        _playFieldOffsetHandler = new PlayFieldOffsetHandler(graphicsDevice);
    }

    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        var offset = _playFieldOffsetHandler.Offset;
        
        foreach (var fadeOutText in fadeOutTexts)
        {
            spriteBatch.DrawStringWithShadow(
                _font,
                fadeOutText.Text,
                fadeOutText.Location + offset,
                Colors.DefaultTextColor);
        }
    }

    public override void Update(GameTime gameTime)
    {
        _playFieldOffsetHandler.Update();
    }
}