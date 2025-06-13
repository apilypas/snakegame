using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.Entities;

public class Panel : Control
{
    public Texture2D Texture { get; set; }
    public Rectangle TextureRectangle { get; set; }
    
    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.DrawFromNinePatch(
            GlobalPosition,
            Size,
            Texture,
            TextureRectangle,
            Color.White);
    }
}