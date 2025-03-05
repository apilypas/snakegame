using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.DesktopGL.Core.Renderers;

public static class RendererUtils
{
    private static Vector2 _playFieldOffset = Vector2.Zero;

    public static Vector2 GetPlayFieldOffset(GraphicsDevice graphicsDevice)
    {
        if (_playFieldOffset == Vector2.Zero)
        {
            var x = (graphicsDevice.Viewport.Width - Constants.WallWidth * Constants.SegmentSize) / 2f;
            var y = (graphicsDevice.Viewport.Height - Constants.WallHeight * Constants.SegmentSize) / 2f;
            _playFieldOffset = new Vector2(x, y);
        }
        
        return _playFieldOffset;
    }
}