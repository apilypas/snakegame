using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace SnakeGame.Core.Inputs;

public class TouchInputHandler
{
    private TouchCollection _touches = TouchPanel.GetState();
    
    public bool IsConnected => _touches.IsConnected;
    public bool IsTouchedAnywhere => GetIsPressedAnywhere();

    public void Update()
    {
        _touches = TouchPanel.GetState();
    }

    public IEnumerable<Vector2> GetTouchedPoints()
    {
        foreach (var touch in _touches)
        {
            if (touch.State is TouchLocationState.Pressed or TouchLocationState.Moved)
                yield return touch.Position / Globals.ScreenScale;
        }
    }
    
    public IEnumerable<Vector2> GetReleasedPoints()
    {
        foreach (var touch in _touches)
        {
            if (touch.State is TouchLocationState.Released)
                yield return touch.Position / Globals.ScreenScale;
        }
    }

    private bool GetIsPressedAnywhere()
    {
        foreach (var touch in _touches)
        {
            if (touch.State is TouchLocationState.Pressed)
                return true;
        }
        
        return false;
    }
}