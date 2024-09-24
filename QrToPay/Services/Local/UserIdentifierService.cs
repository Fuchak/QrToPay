namespace QrToPay.Services.Local;

public static class UserIdentifierService
{
    public static async Task<string> GetOrCreateUserUUIDAsync()
    {
        // Sprawdź, czy UUID już istnieje
        string? userUuid = await SecureStorage.GetAsync(SecureStorageConst.UserUuid);
        if (string.IsNullOrEmpty(userUuid))
        {
            // Jeśli nie istnieje, wygeneruj nowy UUID
            userUuid = Guid.NewGuid().ToString();
            await SecureStorage.SetAsync(SecureStorageConst.UserUuid, userUuid);
        }
        return userUuid;
    }

    public static void ClearUserUUIDAsync()
    {
        // Usuwanie UUID przy wylogowaniu lub zmianie konta
        SecureStorage.Remove(SecureStorageConst.UserUuid);
    }
}