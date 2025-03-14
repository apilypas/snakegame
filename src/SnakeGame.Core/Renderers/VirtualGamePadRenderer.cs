using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace SnakeGame.Core.Renderers;

public class VirtualGamePadRenderer(VirtualGamePad gamePad) : RendererBase
{
    private Texture2D _texture;

    private Sprite _buttonSprite;
    private Sprite _pressedButtonSprite;
    private Sprite _upArrowSprite;
    private Sprite _downArrowSprite;
    private Sprite _leftArrowSprite;
    private Sprite _rightArrowSprite;
    private Sprite _actionSprite;
    private Sprite _pauseSprite;
    
    public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _texture = content.Load<Texture2D>("GamePad");
        _buttonSprite = new Sprite(new Texture2DRegion(_texture, new Rectangle(0, 0, 64, 64)));
        _pressedButtonSprite = new Sprite(new Texture2DRegion(_texture, new Rectangle(64, 0, 64, 64)));
        _upArrowSprite = new Sprite(new Texture2DRegion(_texture, new Rectangle(0, 64, 32, 32)));
        _downArrowSprite = new Sprite(new Texture2DRegion(_texture, new Rectangle(32, 64, 32, 32)));
        _rightArrowSprite = new Sprite(new Texture2DRegion(_texture, new Rectangle(64, 64, 32, 32)));
        _leftArrowSprite = new Sprite(new Texture2DRegion(_texture, new Rectangle(96, 64, 32, 32)));
        _actionSprite = new Sprite(new Texture2DRegion(_texture, new Rectangle(128, 64, 32, 32)));
        _pauseSprite = new Sprite(new Texture2DRegion(_texture, new Rectangle(160, 64, 32, 32)));
    }

    public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
    {
        if (gamePad.IsConnected && gamePad.IsVisible)
        {
            RenderButton(spriteBatch, gamePad.LeftButton, _leftArrowSprite);
            RenderButton(spriteBatch, gamePad.RightButton, _rightArrowSprite);
            RenderButton(spriteBatch, gamePad.UpButton, _upArrowSprite);
            RenderButton(spriteBatch, gamePad.DownButton, _downArrowSprite);
            RenderButton(spriteBatch, gamePad.ActionButton, _actionSprite);
            RenderButton(spriteBatch, gamePad.StartButton, _pauseSprite);
        }
    }

    private void RenderButton(SpriteBatch spriteBatch, VirtualGamePad.VirtualGamePadButton button, Sprite signSprite)
    {
        if (button.IsPressed)
            _pressedButtonSprite.Draw(spriteBatch, button.Position, 0f, Vector2.One);
        else
            _buttonSprite.Draw(spriteBatch, button.Position, 0f, Vector2.One);
        
        signSprite.Draw(spriteBatch, button.Position + new Vector2(16f, 16f), 0f, Vector2.One);
    }

    public override void Update(GameTime gameTime)
    {
    }
}