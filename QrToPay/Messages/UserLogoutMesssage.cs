namespace QrToPay.Messages;

public class UserLogoutRequestMessage
{
    public string Reason { get; }

    public UserLogoutRequestMessage(string reason)
    {
        Reason = reason;
    }
}
public class UserLogoutMessage
{
}