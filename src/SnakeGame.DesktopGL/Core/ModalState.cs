namespace SnakeGame.DesktopGL.Core;

public class ModalState
{
    public enum ModalStateType
    {
        None,
        Paused
    }

    public ModalStateType Type { get; private set; } = ModalStateType.None;

    public void TogglePausedModal()
    {
        Type = Type == ModalStateType.Paused ? ModalStateType.None : ModalStateType.Paused;
    }
}