using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace SnakeGame.Core.Entities;

public class PlayField : Entity
{
    private record struct PlayFieldTile
    {
        public Vector2 Position;
        public Rectangle TileRectangle;
    }
    
    private readonly Rectangle _backgroundRectangle = new(0, 0, 16, 16);
    private readonly HashSet<PlayFieldTile> _tiles = [];

    private readonly Rectangle[] _tilesRectangles =
    [
        new(16, 0, 16, 16),
        new(32, 0, 16, 16),
        new(48, 0, 16, 16),
        new(64, 0, 16, 16)
    ];

    public Texture2D TilesTexture { get; set; }

    public void Initialize()
    {
        RandomizeTiles();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var tile in _tiles)
        {
            spriteBatch.Draw(TilesTexture, GlobalPosition + tile.Position, _backgroundRectangle, Color.White);
            spriteBatch.Draw(TilesTexture, GlobalPosition + tile.Position, tile.TileRectangle, Color.White);
        }
        
        spriteBatch.DrawRectangle(
            GlobalPosition,
            Globals.PlayFieldRectangle.Size,
            Color.Black);
    }

    private void RandomizeTiles()
    {
        var random = new Random();

        for (var x = 0; x < Constants.WallWidth; x++)
        {
            for (var y = 0; y < Constants.WallHeight; y++)
            {
                var at = new Vector2(x * Constants.SegmentSize, y * Constants.SegmentSize);
                var r = _tilesRectangles[random.Next(_tilesRectangles.Length)];
                
                _tiles.Add(new PlayFieldTile
                {
                    Position = at,
                    TileRectangle = r
                });
            }
        }
    }
}