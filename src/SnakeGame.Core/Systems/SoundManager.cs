using System;
using SnakeGame.Core.Events;

namespace SnakeGame.Core.Systems;

public class SoundManager
{
    private readonly Random _random;
    private readonly AssetManager _assets;

    public SoundManager(AssetManager assets, EventBus eventBus)
    {
        _assets = assets;
        
        _random = new Random();

        eventBus.Subscribe<ScoreChangedEvent>(OnScoreChanged);
        eventBus.Subscribe<PlayerDiedEvent>(OnPlayerDied);
        eventBus.Subscribe<GameEndedEvent>(OnGameEnded);
        eventBus.Subscribe<TimerChangedEvent>(OnTimerChanged);
    }

    private void OnTimerChanged(TimerChangedEvent e)
    {
        if (e.Timer <= 10)
        {
            var instance = _assets.Sound4.CreateInstance();
            instance.Volume = .7f;
            instance.Play();
        }
    }

    private void OnGameEnded(GameEndedEvent e)
    {
        var instance = _assets.Sound3.CreateInstance();
        instance.Volume = 1f;
        instance.Play();
    }

    private void OnPlayerDied(PlayerDiedEvent e)
    {
        var instance = _assets.Sound2.CreateInstance();
        instance.Volume = .7f;
        instance.Play();
    }

    private void OnScoreChanged(ScoreChangedEvent e)
    {
        var instance = _assets.Sound1.CreateInstance();
        instance.Pitch = _random.Next(-5, 5) / 10f;
        instance.Volume = .5f;
        instance.Play();
    }
}