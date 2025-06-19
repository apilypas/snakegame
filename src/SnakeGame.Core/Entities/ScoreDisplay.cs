using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SnakeGame.Core.Events;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Entities;

public class ScoreDisplay : Entity
{
    private readonly Label _scoreLabel;
    private readonly Label _multiplicatorLabel;
    private readonly Label _timeLabel;
    
    private int _score;
    private int _timer = (int)Constants.InitialTimer;
    private int _scoreMultiplicator = 1;
    
    public SpriteFont ScoreFont { get; set; } 

    public ScoreDisplay(EventBus eventBus, Texture2D collectablesTexture)
    {
        eventBus.Subscribe<TimerChangedEvent>(OnTimerChanged);
        eventBus.Subscribe<ScoreChangedEvent>(OnScoreChanged);
        eventBus.Subscribe<ScoreMultiplicatorChangedEvent>(OnScoreMultiplayerChanged);
        
        _timeLabel = new Label
        {
            Position = new Vector2(22f, 0f),
            Color = Colors.ScoreTimeColor
        };

        _scoreLabel = new Label
        {
            Position = new Vector2(0f, 10f)
        };
        
        _multiplicatorLabel = new Label
        {
            Position = new Vector2(0f, 37f),
            Size = new SizeF(180f, 0f),
            HorizontalAlignment = Label.HorizontalLabelAlignment.Right,
            Color = Colors.ScoreMultiplicatorColor
        };
        
        var clockSprite = new Sprite
        {
            Texture = collectablesTexture,
            SourceRectangle = new Rectangle(16, 0, 16, 16),
            Position = new Vector2(2f, 3f)
        };

        AddChild(_scoreLabel);
        AddChild(_multiplicatorLabel);
        AddChild(_timeLabel);
        AddChild(clockSprite);
        
        UpdateTexts();
    }

    public override void Update(GameTime gameTime)
    {
        _scoreLabel.Font = ScoreFont;
    }

    private void OnTimerChanged(TimerChangedEvent e)
    {
        _timer = e.Timer;
        UpdateTexts();
    }

    private void OnScoreChanged(ScoreChangedEvent e)
    {
        _score = e.Score;
        UpdateTexts();
    }
    
    private void OnScoreMultiplayerChanged(ScoreMultiplicatorChangedEvent e)
    {
        _scoreMultiplicator = e.ScoreMultiplicator;
        UpdateTexts();
    }

    private void UpdateTexts()
    {
        _scoreLabel.Text = _score.ToString(Constants.ScoreFormat);
        _multiplicatorLabel.Text = $"x{_scoreMultiplicator}";
        _timeLabel.Text = $"{_timer / 60:00}:{_timer % 60:00}";
    }
}