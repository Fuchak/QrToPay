namespace QrToPay.Services.Local;

public static class UserIdentifierService
{
    private const string UserUuidKey = "UserUUID";

    public static async Task<string> GetOrCreateUserUUIDAsync()
    {
        // Sprawdź, czy UUID już istnieje
        string? userUuid = await SecureStorage.GetAsync(UserUuidKey);
        if (string.IsNullOrEmpty(userUuid))
        {
            // Jeśli nie istnieje, wygeneruj nowy UUID
            userUuid = Guid.NewGuid().ToString();
            await SecureStorage.SetAsync(UserUuidKey, userUuid);
        }
        return userUuid;
    }

    public static void ClearUserUUIDAsync()
    {
        // Usuwanie UUID przy wylogowaniu lub zmianie konta
        SecureStorage.Remove(UserUuidKey);
    }
}