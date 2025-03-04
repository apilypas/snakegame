using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.DesktopGL.Core.Sprites;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class ScoreBoardRenderer : RendererBase
{
    private TextSprite _scoreSprite;

    private readonly ScoreBoard _scoreBoard;

    public ScoreBoardRenderer(ScoreBoard scoreBoard)
    {
        _scoreBoard = scoreBoard;
    }

    public override void LoadContent(ContentManager content)
    {
        _scoreSprite = TextSprite.Create().Load(content, "font1");
    }

    public override void Render(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, float deltaTime)
    {
        Offset = PlayFieldRenderer.GetPlayFieldOffset(graphicsDevice);

        RenderScores(spriteBatch);
    }
    
    private void RenderScores(SpriteBatch spriteBatch)
    {
        _scoreSprite.Text = _scoreBoard.ScoreText;

        Draw(spriteBatch,
            new Vector2(GameWorld.GetRectangle().Width + 60f, 20f),
            _scoreSprite);
    }
}
