using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Entities;

public class InputBindingDisplay : Entity
{
    private readonly Sprite _sprite;
    private readonly Label _keyText;
    private readonly Label _keyName;

    public InputBindingDisplay(AssetManager assets, string name, string key)
    {
        _keyName = new Label
        {
            Text = name,
            Position = new Vector2(0f, 0f),
            Size = new SizeF(100f, 32f),
            VerticalAlignment = Label.VerticalLabelAlignment.Center,
            HorizontalAlignment = Label.HorizontalLabelAlignment.Right
        };
        
        AddChild(_keyName);
        
        _sprite = new Sprite
        {
            Texture = assets.CollectableTexture,
            SourceRectangle = new Rectangle(32, 0, 32, 32),
            Position = new Vector2(0, 0)
        };

        AddChild(_sprite);
        
        _keyText = new Label
        {
            Position = new Vector2(0, 0),
            Text = key,
            Font = assets.SmallFont,
            Size = new Vector2(32, 32),
            HorizontalAlignment = Label.HorizontalLabelAlignment.Center,
            VerticalAlignment = Label.VerticalLabelAlignment.Center
        };

        AddChild(_keyText);
    }

    public override void Update(GameTime gameTime)
    {
        _sprite.Position = new Vector2(
            _keyName.Position.X + _keyName.Size.Width + 10f,
            _keyName.Position.Y);

        _keyText.Position = new Vector2(
            _keyName.Position.X + _keyName.Size.Width + 10f,
            _keyName.Position.Y);
    }
}