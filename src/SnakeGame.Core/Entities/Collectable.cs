using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Entities;

public class Collectable : Entity
{
    private readonly AssetManager _assets;

    public CollectableType Type { get; private set; }
    public Sprite Sprite { get; private set; }

    public Collectable(AssetManager asset, CollectableType type)
    {
        _assets = asset;
        Type = type;
        Sprite = CreateSprite(Type);
    }

    private Sprite CreateSprite(CollectableType type)
    {
        return type switch
        {
            CollectableType.Diamond => new Sprite(
                new Texture2DRegion(_assets.CollectableTexture,
                    new Rectangle(0, 32, 16, 16))),
            CollectableType.SnakePart => new Sprite(
                new Texture2DRegion(_assets.CollectableTexture,
                    new Rectangle(0, 16, 16, 16))),
            CollectableType.SpeedBoost => new Sprite(
                new Texture2DRegion(_assets.CollectableTexture,
                    new Rectangle(0, 0, 16, 16))),
            CollectableType.Clock => new Sprite(
                new Texture2DRegion(_assets.CollectableTexture,
                    new Rectangle(16, 0, 16, 16))),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}
