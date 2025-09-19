using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SnakeGame.Core.ECS.Components;

namespace SnakeGame.Core.Services;

public class GameContentManager
{
    private readonly Dictionary<SoundEffectTypes, SoundEffect> _soundEffects = [];
    
    public Texture2D CollectableTexture { get; private set; }
    public Texture2D SnakeTexture { get; private set; }
    public Texture2D UserInterfaceTexture { get; private set; }
    public Texture2D TilesTexture { get; private set; }
    public SpriteFont SmallFont { get; private set; }
    public SpriteFont MainFont { get; private set; }
    public SpriteFont BigFont { get; private set; }
    public SpriteFont LogoFont { get; private set; }
    public Song MainTrackSong { get; private set; }
    
    public void LoadContent(ContentManager content)
    {
        CollectableTexture = content.Load<Texture2D>("Collectables");
        SnakeTexture = content.Load<Texture2D>("Snake");
        UserInterfaceTexture = content.Load<Texture2D>("UserInterface");
        TilesTexture = content.Load<Texture2D>("Tiles");
        SmallFont = content.Load<SpriteFont>("SmallFont");
        MainFont = content.Load<SpriteFont>("MainFont");
        BigFont = content.Load<SpriteFont>("BigFont");
        LogoFont = content.Load<SpriteFont>("LogoFont");
        MainTrackSong = content.Load<Song>("Tracks/Main");

        foreach (var effect in Enum.GetValues(typeof(SoundEffectTypes)))
        {
            _soundEffects[(SoundEffectTypes)effect] = content.Load<SoundEffect>($"Sounds/{effect}");
        }
    }

    public SoundEffect GetSoundEffect(SoundEffectTypes effect)
    {
        return _soundEffects[effect];
    }
}