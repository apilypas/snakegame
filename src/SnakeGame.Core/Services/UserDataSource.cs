using System;
using System.Linq;
using ShareGameLib.Utils;
using SnakeGame.Core.Data;

namespace SnakeGame.Core.Services;

public class UserDataSource
{
    private const string ScoreBoardFile = "ScoreBoard.json";
    
    public ScoreBoardData LoadScoreBoard()
    {
        var scoreBoard = UserStoreUtils.LoadJson<ScoreBoardData>(ScoreBoardFile);
        
        return scoreBoard ?? new ScoreBoardData { Entries = [] };
    }

    public int SaveScore(int score, int timePlayed)
    {
        var scoreBoard = LoadScoreBoard();
        
        var id = scoreBoard.Entries.Count > 0 
            ? scoreBoard.Entries.Max(x => x.Id) + 1
            : 1;
        
        scoreBoard.Entries.Add(new ScoreBoardEntryData
        {
            Id = id,
            CreatedAt = DateTime.Now,
            Score = score,
            TimePlayed = timePlayed
        });
        
        scoreBoard.Entries = scoreBoard
            .Entries
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.CreatedAt)
            .Take(Constants.MaxScoreBoardEntries)
            .ToList();
        
        UserStoreUtils.SaveJson(ScoreBoardFile, scoreBoard);

        return id;
    }
}