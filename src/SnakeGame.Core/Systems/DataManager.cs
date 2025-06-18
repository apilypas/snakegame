using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text.Json;
using SnakeGame.Core.Data;

namespace SnakeGame.Core.Systems;

public class DataManager
{
    private const string ScoreBoardFile = "ScoreBoard.json";
    
    public ScoreBoardData LoadScoreBoard()
    {
        var scoreBoard = Load<ScoreBoardData>(ScoreBoardFile);
        
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
        
        Save(ScoreBoardFile, scoreBoard);

        return id;
    }

    private static T Load<T>(string fileName)
    {
        var storage = IsolatedStorageFile.GetUserStoreForApplication();
        
        if (!storage.FileExists(fileName))
            return default;
        
        using var stream = new IsolatedStorageFileStream(ScoreBoardFile, FileMode.Open, storage);
        using var reader = new StreamReader(stream);
        
        var json = reader.ReadToEnd();
        
        reader.Close();
        stream.Close();
        
        return JsonSerializer.Deserialize<T>(json);
    }

    private static void Save<T>(string fileName, T data)
    {
        var storage = IsolatedStorageFile.GetUserStoreForApplication();
        using var stream = new IsolatedStorageFileStream(fileName, FileMode.Create, storage);
        using var writer = new StreamWriter(stream);
        
        writer.Write(JsonSerializer.Serialize(data));
        writer.Flush();
        writer.Close();
    }
}