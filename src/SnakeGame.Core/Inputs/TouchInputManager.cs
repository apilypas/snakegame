using System.Linq;
using Microsoft.Xna.Framework.Input.Touch;
using SnakeGame.Core.Commands;

namespace SnakeGame.Core.Inputs;

public class TouchInputManager
{
    private TouchCollection _touches;
    
    private ICommand _touchedBinding;

    public void Update()
    {
        _touches = TouchPanel.GetState();

        if (_touchedBinding != null && IsTouched)
        {
            _touchedBinding.Execute();
        }
    }
    
    public void BindTouched(ICommand command)
    {
        _touchedBinding = command;
    }

    public bool IsConnected => _touches.IsConnected;
    public bool IsTouched => _touches.Any(x => x.State is TouchLocationState.Pressed or TouchLocationState.Moved);
    
    public TouchCollection Touches => _touches;
}