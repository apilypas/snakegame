using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Entities;

namespace SnakeGame.Core.Systems;

public class ThemeManager
{
    private readonly SpriteFont _labelFont;
    private readonly SpriteFont _scoreFont;
    private readonly Texture2D _userInterfaceTexture;

    public ThemeManager(AssetManager assets)
    {
        _labelFont = assets.MainFont;
        _scoreFont = assets.BigFont;
        _userInterfaceTexture = assets.UserInterfaceTexture;
    }

    public void Apply(Entity entity)
    {
        if (entity.Theme == null)
        {
            if (entity is Label label)
                ApplyToLabel(label);
            
            if (entity is Panel panel)
                ApplyToPanel(panel);

            if (entity is Button button)
                ApplyToButton(button);
            
            if (entity is ScoreDisplay scoreDisplay)
                ApplyToScoreDisplay(scoreDisplay);
            
            entity.Theme = this;
        }

        foreach (var child in entity.GetChildren())
            Apply(child);
    }

    private void ApplyToScoreDisplay(ScoreDisplay scoreDisplay)
    {
        scoreDisplay.ScoreFont = _scoreFont;
    }

    private void ApplyToButton(Button button)
    {
        if (button.Texture == null)
        {
            button.Texture = _userInterfaceTexture;
            button.TextureNormalRectangle = new Rectangle(0, 48, 48, 48);
            button.TextureHoveredRectangle = new Rectangle(48, 48, 48, 48);
            button.TexturePressedRectangle = new Rectangle(96, 48, 48, 48);
        }
    }

    private void ApplyToPanel(Panel panel)
    {
        if (panel.Texture == null)
        {
            panel.Texture = _userInterfaceTexture;
            panel.TextureRectangle = new Rectangle(0, 0, 48, 48);
        }
    }

    private void ApplyToLabel(Label label)
    {
        if (label.Font == null)
            label.Font = _labelFont;
        
        if (label.Color == Color.White)
            label.Color = Colors.DefaultTextColor;
    }
}