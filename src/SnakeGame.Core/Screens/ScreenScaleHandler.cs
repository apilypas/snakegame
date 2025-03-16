using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using SnakeGame.Core.Renderers;

namespace SnakeGame.Core.Screens;

public class ScreenScaleHandler(GameScreen screen)
{
    private int _screenWidth;
    private int _screenHeight;
    
    public bool UpdateScreenScaling()
    {
        if (_screenWidth == screen.GraphicsDevice.Viewport.Width && _screenHeight == screen.GraphicsDevice.Viewport.Height)
            return false;

        _screenWidth = screen.GraphicsDevice.Viewport.Width;
        _screenHeight = screen.GraphicsDevice.Viewport.Height;
        
        var screenRatio = _screenWidth / (float)_screenHeight;
        var width = (int) (Constants.ScreenHeight * screenRatio);
        var scale = MathF.Min(_screenWidth / (float)width, _screenHeight / (float)Constants.ScreenHeight);
        
        Globals.ScreenScale = new Vector2(scale, scale);
        Globals.VirtualScreenWidth = width;
        Globals.VirtualScreenHeight = Constants.ScreenHeight;
        
        Globals.PlayFieldOffset = new Vector2(
            (Globals.VirtualScreenWidth - Constants.WallWidth * Constants.SegmentSize) / 2f,
            (Globals.VirtualScreenHeight - Constants.WallHeight * Constants.SegmentSize) / 2f);

        return true;
    }
}