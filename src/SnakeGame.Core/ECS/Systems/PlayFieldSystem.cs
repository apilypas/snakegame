using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.ECS.Components;

namespace SnakeGame.Core.ECS.Systems;

public class PlayFieldSystem : EntityProcessingSystem
{
    private ComponentMapper<PlayFieldComponent> _playFieldMapper;

    public PlayFieldSystem() 
        : base(Aspect.All(typeof(PlayFieldComponent)))
    {
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _playFieldMapper = mapperService.GetMapper<PlayFieldComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var playField = _playFieldMapper.Get(entityId);

        if (!playField.IsInitialized)
        {
            RandomizeTiles(playField);
            playField.IsInitialized = true;
        }
    }
    
    private static void RandomizeTiles(PlayFieldComponent playField)
    {
        Rectangle[] tilesRectangles =
        [
            new(16, 0, 16, 16),
            new(32, 0, 16, 16),
            new(48, 0, 16, 16),
            new(64, 0, 16, 16)
        ];
        
        for (var x = 0; x < Constants.WallWidth; x++)
        {
            for (var y = 0; y < Constants.WallHeight; y++)
            {
                var at = new Vector2(x * Constants.SegmentSize, y * Constants.SegmentSize);
                var r = tilesRectangles[Random.Shared.Next(tilesRectangles.Length)];
                
                playField.Tiles.Add(new PlayFieldTile
                {
                    Position = at,
                    TileRectangle = r
                });
            }
        }
    }
}