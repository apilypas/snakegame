using SnakeGame.Core.Dialogs;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Screens;

namespace SnakeGame.Core.Systems;

public class DialogManager(PlayScreen playScreen, Entity world)
{
    public PauseDialog Pause { get; } = new(playScreen, world);
    public GameOverDialog GameOver { get; } = new(playScreen, world);
}