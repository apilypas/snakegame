using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.DesktopGL.Core.Sprites;

namespace SnakeGame.DesktopGL.Core.Renderers;

public class PlayFieldRenderer : RendererBase
{
    private GameWorld _gameWorld;

    private TextureSprite _backgroundSprite1;
    private TextureSprite _backgroundSprite2;
    private TextureSprite _frameSprite1;
    private TextureSprite _frameSprite2;
    private TextureSprite _frameSprite3;
    private TextureSprite _frameSprite4;
    
    private readonly IList<Vector2> _backgroundTiles;
    private readonly IList<Vector2> _backgroundGrassTiles;
    private readonly IList<Vector2> _frameTiles;

    public PlayFieldRenderer(GameWorld gameWorld)
    {
        _gameWorld = gameWorld;
        _backgroundTiles = [];
        _backgroundGrassTiles = [];
        _frameTiles = [];
    }

    public override void LoadContent(ContentManager content)
    {
        _backgroundSprite1 = TextureSprite
            .Create(new Rectangle(40, 40, 20, 20))
            .Load(content, "snake");

        _backgroundSprite2 = TextureSprite
            .Create(new Rectangle(60, 40, 20, 20))
            .Load(content, "snake");

        _frameSprite1 = TextureSprite
            .Create(new Rectangle(0, 40, 20, 20))
            .Load(content, "snake");
        _frameSprite1.Rotation = 0f;

        _frameSprite2 = TextureSprite
            .Create(new Rectangle(0, 40, 20, 20))
            .Load(content, "snake");
        _frameSprite2.Rotation = MathF.PI / 2f;

        _frameSprite3 = TextureSprite
            .Create(new Rectangle(0, 40, 20, 20))
            .Load(content, "snake");
        _frameSprite3.Rotation = MathF.PI;

        _frameSprite4 = TextureSprite
            .Create(new Rectangle(0, 40, 20, 20))
            .Load(content, "snake");
        _frameSprite4.Rotation = -MathF.PI / 2f;

        InitializeBackground();
    }

    public override void Render(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, float deltaTime)
    {
        Offset = GetPlayFieldOffset(graphicsDevice);
        
        DrawBackground(spriteBatch);
        DrawFrame(spriteBatch);
    }

    public static Vector2 GetPlayFieldOffset(GraphicsDevice graphicsDevice)
    {
        return new Vector2(
            (graphicsDevice.Viewport.Width - Constants.WallWidth * Constants.SegmentSize) / 2f,
            (graphicsDevice.Viewport.Height - Constants.WallHeight * Constants.SegmentSize) / 2f);
    }

    private void DrawBackground(SpriteBatch spriteBatch)
    {
        foreach (var location in _backgroundTiles)
            Draw(spriteBatch, location, _backgroundSprite1);

        foreach (var location in _backgroundGrassTiles)
            Draw(spriteBatch, location, _backgroundSprite2);
    }

    private void DrawFrame(SpriteBatch spriteBatch)
    {
        const float tolerance = 0.001f;
        
        foreach (var location in _frameTiles)
        {
            if (location.Y == 0f)
                Draw(spriteBatch, location, _frameSprite1);
            
            if (Math.Abs(location.Y - (Constants.WallHeight - 1) * Constants.SegmentSize) < tolerance)
                Draw(spriteBatch, location, _frameSprite3);
            
            if (location.X == 0f)
                Draw(spriteBatch, location, _frameSprite4);
            
            if (Math.Abs(location.X - (Constants.WallWidth - 1) * Constants.SegmentSize) < tolerance)
                Draw(spriteBatch, location, _frameSprite2);
        }
    }

    private void InitializeBackground()
    {
        var random = new Random();

        for (var i = 0; i < Constants.WallHeight; i++)
        {
            var y = i * Constants.SegmentSize;
            for (var j = 0; j < Constants.WallWidth; j++)
            {
                var x = j * Constants.SegmentSize;
                var location = new Vector2(x, y);
                var hasGrass = random.Next() % 12 == 0;
                if (hasGrass)
                    _backgroundGrassTiles.Add(location);
                else
                    _backgroundTiles.Add(location);
                if (i == 0 || i == (Constants.WallHeight - 1) || j == 0 || j == (Constants.WallWidth - 1))
                    _frameTiles.Add(location);
            }
        }
    }
}