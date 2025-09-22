using System.IO;
using System.IO.IsolatedStorage;
using System.Text.Json;

namespace SnakeGame.Core.Utils;

public static class UserStoreUtils
{
    public static T LoadJson<T>(string fileName)
    {
        var storage = IsolatedStorageFile.GetUserStoreForApplication();
        
        if (!storage.FileExists(fileName))
            return default;
        
        using var stream = new IsolatedStorageFileStream(fileName, FileMode.Open, storage);
        using var reader = new StreamReader(stream);
        
        var json = reader.ReadToEnd();
        
        reader.Close();
        stream.Close();
        
        return JsonSerializer.Deserialize<T>(json);
    }

    public static void SaveJson<T>(string fileName, T data)
    {
        var storage = IsolatedStorageFile.GetUserStoreForApplication();
        using var stream = new IsolatedStorageFileStream(fileName, FileMode.Create, storage);
        using var writer = new StreamWriter(stream);
        
        writer.Write(JsonSerializer.Serialize(data));
        writer.Flush();
        writer.Close();
    }
}