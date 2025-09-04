using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Services;

namespace SnakeGame.Core.ECS.Systems;

public class SoundEffectSystem : EntityProcessingSystem
{
    private readonly ContentManager _contents;
    private ComponentMapper<SoundEffectComponent> _soundEffectMapper;

    public SoundEffectSystem(ContentManager contents) 
        : base(Aspect.All(typeof(SoundEffectComponent)))
    {
        _contents = contents;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _soundEffectMapper = mapperService.GetMapper<SoundEffectComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var audioEffect = _soundEffectMapper.Get(entityId);

        if (audioEffect.Type == SoundEffectTypes.TimerChanged)
        {
            var instance = _contents.Sound4.CreateInstance();
            instance.Volume = .7f;
            instance.Play();
        }

        if (audioEffect.Type == SoundEffectTypes.GameEnded)
        {
            var instance = _contents.Sound3.CreateInstance();
            instance.Volume = 1f;
            instance.Play();
        }

        if (audioEffect.Type == SoundEffectTypes.PlayerDied)
        {
            var instance = _contents.Sound2.CreateInstance();
            instance.Volume = .7f;
            instance.Play();
        }

        if (audioEffect.Type == SoundEffectTypes.ScoreChanged)
        {
            var instance = _contents.Sound1.CreateInstance();
            instance.Pitch = Random.Shared.Next(-5, 5) / 10f;
            instance.Volume = .5f;
            instance.Play();
        }
        
        _soundEffectMapper.Delete(entityId);
    }
}