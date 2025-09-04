using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Core.ECS.Components;

public record struct PlayFieldTile
{
    public Vector2 Position;
    public Rectangle TileRectangle;
}

public class PlayFieldComponent
{
    public bool IsInitialized { get; set; }
    public HashSet<PlayFieldTile> Tiles { get; set; } = [];
    public Texture2D TilesTexture { get; set; }
}