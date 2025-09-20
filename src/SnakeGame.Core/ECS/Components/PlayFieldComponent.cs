using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Core.ECS.Components;

public class PlayFieldTile
{
    public Vector2 Position { get; set; }
    public Rectangle TileRectangle { get; set; }
    public bool IsPlayerFar { get; set; }
}

public class PlayFieldComponent
{
    public bool IsInitialized { get; set; }
    public HashSet<PlayFieldTile> Tiles { get; set; } = [];
    public Texture2D TilesTexture { get; set; }
}