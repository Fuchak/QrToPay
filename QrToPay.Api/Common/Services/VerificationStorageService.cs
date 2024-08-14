using System.Collections.Concurrent;

namespace QrToPay.Api.Common.Services;

public class VerificationStorageService
{
    private readonly ConcurrentDictionary<int, string> _emailVerificationStorage = new();
    private readonly ConcurrentDictionary<int, string> _phoneVerificationStorage = new();

    public void StoreEmailVerification(int userId, string email)
    {
        _emailVerificationStorage[userId] = email;
    }

    public void StorePhoneVerification(int userId, string phoneNumber)
    {
        _phoneVerificationStorage[userId] = phoneNumber;
    }

    public bool TryGetEmailVerification(int userId, out string? email)
    {
        return _emailVerificationStorage.TryGetValue(userId, out email);
    }

    public bool TryGetPhoneVerification(int userId, out string? phoneNumber)
    {
        return _phoneVerificationStorage.TryGetValue(userId, out phoneNumber);
    }

    public void RemoveEmailVerification(int userId)
    {
        _emailVerificationStorage.TryRemove(userId, out _);
    }

    public void RemovePhoneVerification(int userId)
    {
        _phoneVerificationStorage.TryRemove(userId, out _);
    }
}