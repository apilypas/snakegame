using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.DesktopGL.Core;

public class StateManager
{
    private IState _currentState;

    private ContentManager _content;
    private GraphicsDevice _graphics;

    public StateManager(GraphicsDevice graphics, ContentManager content)
    {
        _graphics = graphics;
        _content = content;
    }

    public IState GetCurrentState()
    {
        return _currentState;
    }

    public void SwitchToPlayState()
    {
        _content.Unload();
        
        _currentState = new PlayState(this);
        _currentState.Initialize();
        _currentState.LoadContent(_graphics, _content);
    }

    public void Initialize()
    {
        _currentState = new StartState(this);
        _currentState.Initialize();
    }
}