using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace SnakeGame.Core.Services;

public class CameraManager
{
    public float Zoom { get; private set; }
    public OrthographicCamera Camera { get; }

    public CameraManager(
        Game game,
        int virtualWidth,
        int virtualHeight,
        float zoom)
    {
        var viewportAdapter = new BoxingViewportAdapter(
            game.Window,
            game.GraphicsDevice,
            virtualWidth,
            virtualHeight);
        
        Camera = new OrthographicCamera(viewportAdapter)
        {
            Zoom = zoom
        };
        
        Zoom = zoom;
    }

    public void LookAt(Vector2 position, bool smooth = false)
    {
        // Calculate smoothed position
        var newPosition = smooth && Vector2.Distance(Camera.Center, position) >= 1f
            ? Vector2.Lerp(Camera.Center, position, .06f)
            : position;

        Camera.LookAt(newPosition);
    }

    public Matrix GetViewMatrix()
    {
        return Camera.GetViewMatrix();
    }
}