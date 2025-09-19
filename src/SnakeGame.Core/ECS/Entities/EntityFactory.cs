using MonoGame.Extended.ECS;
using SnakeGame.Core.Services;

namespace SnakeGame.Core.ECS.Entities;

public class EntityFactory
{
    public WorldEntityFactory World { get; private set; }
    public DialogEntityFactory Dialog { get; private set; }
    public HudEntityFactory Hud { get; private set; }

    public void Initialize(World world, GameContentManager contents)
    {
        World = new WorldEntityFactory(world, contents);
        Dialog = new DialogEntityFactory(world, contents);
        Hud = new HudEntityFactory(world, contents);
    }
}