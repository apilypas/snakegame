using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Core.Utils;

public static class ViewportExtensions
{
    public static float GetRenderTargetScale(this Viewport viewport)
    {
        var scaleX = (float)viewport.Width / Constants.VirtualScreenWidth;
        var scaleY = (float)viewport.Height / Constants.VirtualScreenHeight;
        
        var scale = MathF.Max(1f, MathF.Min(scaleX, scaleY));
        
        return scale - MathF.Floor(scale) >= .75f 
            ? MathF.Ceiling(scale) 
            : MathF.Floor(scale);
    }

    public static Rectangle GetRenderTargetRectangle(this Viewport viewport, float scale)
    {
        var renderWidth = (int) (Constants.VirtualScreenWidth * scale);
        var renderHeight = (int) (Constants.VirtualScreenHeight * scale);
        var renderX = (int) ((viewport.Width - renderWidth) / 2f);
        var renderY = (int) ((viewport.Height - renderHeight) / 2f);

        return new Rectangle(renderX, renderY, renderWidth, renderHeight);
    }
}