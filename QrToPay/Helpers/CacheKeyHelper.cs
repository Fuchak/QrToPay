namespace QrToPay.Helpers;
public static class CacheKeyHelper
{
    public static string GetCacheKey(string baseKey)
    {
        return $"{baseKey}".ToLower();
    }

    // Przeciążenie z dwoma argumentami
    public static string GetCacheKey(string baseKey, string cityName)
    {
        return $"{baseKey}_{cityName}".ToLower();
    }

    // Przeciążenie z trzema argumentami
    public static string GetCacheKey(string baseKey, string cityName, string resortName)
    {
        return $"{baseKey}_{cityName}_{resortName}".ToLower();
    }
}