using System;
using Microsoft.Xna.Framework;

namespace SnakeGame.Core.Screens;

public class ScreenScalingHandler(ScreenBase screen, int screenWidth, int screenHeight)
{
    private int _viewportWidth = 0;
    private int _viewportHeight = 0;
        
    public Point Position { get; private set; } = Point.Zero;
    public Point Size { get; private set; } = Point.Zero;
    public float Scale { get; private set; } = 1f;
        
    public void Update()
    {
        if (screen.GraphicsDevice.Viewport.Width == _viewportWidth
            && screen.GraphicsDevice.Viewport.Height == _viewportHeight)
        {
            return;
        }

        _viewportWidth = screen.GraphicsDevice.Viewport.Width;
        _viewportHeight = screen.GraphicsDevice.Viewport.Height;
        
        Scale = MathF.Min(
            _viewportWidth / (float)screenWidth,
            _viewportHeight / (float)screenHeight);

        Size = new Point((int)(Scale * screenWidth), (int)(Scale * screenHeight));
        
        Position = new Point(
            (_viewportWidth - Size.X) / 2,
            (_viewportHeight - Size.Y) / 2);
    }
}