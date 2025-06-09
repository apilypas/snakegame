using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Utils;

namespace SnakeGame.Core.Renderers;

public class ScoreBoardRenderer(ScoreBoard scoreBoard) : RendererBase
{
    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        var displayTextPosition = new Vector2(GameWorld.GetRectangle().Width + 16f, 16f) + Globals.PlayFieldOffset;
        
        DrawLabel(spriteBatch, scoreBoard.DisplayLabel, displayTextPosition);
    }

    private void DrawLabel(SpriteBatch spriteBatch, Label label, Vector2 position)
    {
        spriteBatch.DrawStringWithShadow(
            label.Font,
            label.Text,
            position,
            Colors.DefaultTextColor,
            0f,
            Vector2.Zero);
    }
}
