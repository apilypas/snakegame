using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.ECS.Components;
using SnakeGame.Core.Services;

namespace SnakeGame.Core.ECS.Systems;

public class SoundEffectSystem : EntityProcessingSystem
{
    private readonly GameContentManager _contents;
    private ComponentMapper<SoundEffectComponent> _soundEffectMapper;

    public SoundEffectSystem(GameContentManager contents) 
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
        
        var instance = _contents.GetSoundEffect(audioEffect.Type).CreateInstance();
        instance.Pitch = Random.Shared.Next(-2, 2) / 10f;
        instance.Volume = .7f;
        instance.Play();

        _soundEffectMapper.Delete(entityId);
    }
}