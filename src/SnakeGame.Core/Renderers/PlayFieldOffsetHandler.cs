using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Core.Renderers;

public class PlayFieldOffsetHandler(GraphicsDevice graphicsDevice)
{
    private int _screenWidth = 0;
    private int _screenHeight = 0;
    private Vector2 _offset = Vector2.Zero;

    public void Update()
    {
        if (graphicsDevice.Viewport.Width == _screenWidth && graphicsDevice.Viewport.Height == _screenHeight)
        {
            return;
        }
        
        _screenWidth = graphicsDevice.Viewport.Width;
        _screenHeight = graphicsDevice.Viewport.Height;
        
        var screenRatio = _screenWidth / (float)_screenHeight;
        var width = (int)(Constants.ScreenHeight * screenRatio);
        
        _offset = new Vector2(
            (width - Constants.WallWidth * Constants.SegmentSize) / 2f,
            (Constants.ScreenHeight - Constants.WallHeight * Constants.SegmentSize) / 2f
            );

        _offset.Round();
    }
    
    public Vector2 Offset => _offset;
}