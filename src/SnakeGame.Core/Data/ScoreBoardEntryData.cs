using System;

namespace SnakeGame.Core.Data;

public class ScoreBoardEntryData
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Score { get; set; }
    public int TimePlayed { get; set; }
}