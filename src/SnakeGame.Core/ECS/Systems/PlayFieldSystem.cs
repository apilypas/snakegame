using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using SnakeGame.Core.ECS.Components;

namespace SnakeGame.Core.ECS.Systems;

public class PlayFieldSystem : EntityUpdateSystem
{
    private ComponentMapper<PlayFieldComponent> _playFieldMapper;
    private ComponentMapper<PlayerComponent> _playerMapper;
    private ComponentMapper<SnakeComponent> _snakeMapper;

    public PlayFieldSystem() 
        : base(Aspect.One(typeof(PlayFieldComponent), typeof(PlayerComponent)))
    {
    }

    public override void Initialize(IComponentMapperService mapperService)
    {
        _playFieldMapper = mapperService.GetMapper<PlayFieldComponent>();
        _playerMapper = mapperService.GetMapper<PlayerComponent>();
        _snakeMapper = mapperService.GetMapper<SnakeComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        SnakeComponent snake = null;

        foreach (var entityId in ActiveEntities)
        {
            if (_playerMapper.Has(entityId))
            {
                snake = _snakeMapper.Get(entityId);
            }
        }
        
        foreach (var entityId in ActiveEntities)
        {
            var playField = _playFieldMapper.Get(entityId);

            if (playField is { IsInitialized: false })
            {
                RandomizeTiles(playField);
                playField.IsInitialized = true;
            }
        }

        foreach (var entityId in ActiveEntities)
        {
            var playField = _playFieldMapper.Get(entityId);

            if (playField != null)
            {
                foreach (var tile in playField.Tiles)
                {
                    tile.IsPlayerFar = true;
                    if (snake != null)
                    {
                        foreach (var segment in snake.Segments)
                        {
                            if (Vector2.Distance(tile.Position, segment.Position) < 30f)
                            {
                                tile.IsPlayerFar = false;
                                break;
                            }
                        }
                    }
                }
            }
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