using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeGame.Core.Entities;

namespace SnakeGame.Core.Systems;

public class ThemeManager
{
    private readonly SpriteFont _labelFont;
    private readonly Texture2D _userInterfaceTexture;

    public ThemeManager(AssetManager assets)
    {
        _labelFont = assets.MainFont;
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
            
            entity.Theme = this;
        }

        foreach (var child in entity.GetChildren())
            Apply(child);
    }

    private void ApplyToButton(Button button)
    {
        button.Texture = _userInterfaceTexture;
        button.TextureNormalRectangle = new Rectangle(0, 48, 48, 48);
        button.TextureHoveredRectangle = new Rectangle(48, 48, 48, 48);
        button.TexturePressedRectangle = new Rectangle(96, 48, 48, 48);
    }

    private void ApplyToPanel(Panel panel)
    {
        panel.Texture = _userInterfaceTexture;
        panel.TextureRectangle = new Rectangle(0, 0, 48, 48);
    }

    private void ApplyToLabel(Label label)
    {
        label.Font = _labelFont;
        label.Color = Colors.DefaultTextColor;
    }
}