using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;

namespace QrToPay.Services.Local;
public class CacheService
{
    private readonly string _cacheDirectory;

    public CacheService()
    {
        _cacheDirectory = FileSystem.CacheDirectory;
    }

    // Zapisuje dane do pliku w pamięci podręcznej
    public async Task SaveToCacheAsync<T>(string fileName, T data)
    {
        var filePath = GetCacheFilePath(fileName);
        var serializedData = JsonSerializer.Serialize(data);
        await File.WriteAllTextAsync(filePath, serializedData);
    }

    // Odczytuje dane z pliku w pamięci podręcznej
    public async Task<T?> LoadFromCacheAsync<T>(string fileName) where T : class
    {
        var filePath = GetCacheFilePath(fileName);
        if (File.Exists(filePath))
        {
            var content = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<T>(content);
        }

        return null;
    }

    // Usuwa plik pamięci podręcznej
    public void RemoveFromCache(string fileName)
    {
        var filePath = GetCacheFilePath(fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    // Czyści całą pamięć podręczną
    public void ClearAllCache()
    {
        var files = Directory.GetFiles(_cacheDirectory);
        foreach (var file in files)
        {
            File.Delete(file);
        }
    }

    // Metoda pomocnicza do porównania dwóch list w formie JSON
    public static bool AreDataEqual<T>(IEnumerable<T> currentData, IEnumerable<T> newData)
    {
        var currentDataSerialized = JsonSerializer.Serialize(currentData);
        var newDataSerialized = JsonSerializer.Serialize(newData);

        return currentDataSerialized == newDataSerialized;

    }

    // Metoda pomocnicza do aktualizacji listy
    public static void UpdateCollection<T>(ObservableCollection<T> collection, IEnumerable<T> newItems)
    {
        collection.Clear();
        collection.AddRange(newItems);
    }

    // Pomocnicza metoda do uzyskania ścieżki do pliku pamięci podręcznej
    private string GetCacheFilePath(string fileName)
    {
        return Path.Combine(_cacheDirectory, $"{fileName}.json");
    }
}

// Rozszerzenie dla ObservableCollection do obsługi AddRange
public static class ObservableCollectionExtensions
{
    public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            collection.Add(item);
        }
    }
}