using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.Data;
using SnakeGame.Core.ECS.Components;

namespace SnakeGame.Core.ECS.Systems;

public class InvincibleSystem : EntityProcessingSystem
{
    private readonly GameState _gameState;
    private ComponentMapper<InvincibleComponent> _invincibleMapper;

    public InvincibleSystem(GameState gameState) 
        : base(Aspect.All(typeof(InvincibleComponent)))
    {
        _gameState = gameState;
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _invincibleMapper = mapperService.GetMapper<InvincibleComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        if (_gameState.IsPaused) return;
        
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var invincible = _invincibleMapper.Get(entityId);

        invincible.Timer -= deltaTime;

        if (invincible.Timer <= 0)
        {
            _invincibleMapper.Delete(entityId);
        }
    }
}