using MonoGame.Extended.Tiled;
using SnakeGame.Core.Core.Systems;

namespace SnakeGame.Core.Entities;

public class PlayField : Entity
{
    public TiledMap TiledMap { get; private set; }

    public PlayField(AssetManager assets)
    {
        TiledMap = assets.TiledMap;
    }
}