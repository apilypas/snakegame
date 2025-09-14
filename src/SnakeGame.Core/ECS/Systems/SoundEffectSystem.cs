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
            var instance = _contents.TimerSoundEffect.CreateInstance();
            instance.Volume = .7f;
            instance.Play();
        }
        else if (audioEffect.Type == SoundEffectTypes.GameEnded)
        {
            var instance = _contents.GameEndSoundEffect.CreateInstance();
            instance.Volume = .7f;
            instance.Play();
        }
        else if (audioEffect.Type == SoundEffectTypes.PlayerDied)
        {
            var instance = _contents.HitSoundEffect.CreateInstance();
            instance.Volume = .7f;
            instance.Play();
        }
        else if (audioEffect.Type == SoundEffectTypes.Pickup)
        {
            var instance = _contents.PickupSoundEffect.CreateInstance();
            instance.Pitch = Random.Shared.Next(-1, 1) / 10f;
            instance.Volume = .7f;
            instance.Play();
        }
        else if (audioEffect.Type == SoundEffectTypes.SpeedUp)
        {
            var instance = _contents.SpeedUpSoundEffect.CreateInstance();
            instance.Volume = .7f;
            instance.Play();
        }
        else if (audioEffect.Type == SoundEffectTypes.Click)
        {
            var instance = _contents.ClickSoundEffect.CreateInstance();
            instance.Volume = .5f;
            instance.Play();
        }
        else if (audioEffect.Type == SoundEffectTypes.Turn)
        {
            var instance = _contents.TurnSoundEffect.CreateInstance();
            instance.Pitch = Random.Shared.Next(-1, 1) / 10f;
            instance.Volume = .7f;
            instance.Play();
        }

        _soundEffectMapper.Delete(entityId);
    }
}