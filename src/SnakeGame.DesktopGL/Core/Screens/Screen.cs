using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.DesktopGL.Core.Screens;

public abstract class ScreenBase
{
    public abstract void Initialize();
    public abstract void LoadContent(GraphicsDevice graphicsDevice, ContentManager content);
    public abstract void Update(float deltaTime);
    public abstract void Draw(GraphicsDevice graphicsDevice, float deltaTime, SpriteBatch spriteBatch);
}