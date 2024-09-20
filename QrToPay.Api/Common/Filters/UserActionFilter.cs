using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QrToPay.Api.Common.Filters;
public class UserActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.Items.TryGetValue("UserId", out var userIdObj) || userIdObj is not int userId)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var email = context.HttpContext.Items["UserEmail"] as string;
        var phoneNumber = context.HttpContext.Items["UserPhoneNumber"] as string;

        Console.WriteLine($"UserId: {userId}, Email: {email}, PhoneNumber: {phoneNumber}");

        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is IUserRequest userRequest)
            {
                userRequest.UserId = userId;
            }

            if (argument is IUserContactRequest contactRequest)
            {
                contactRequest.Email = email;
                contactRequest.PhoneNumber = phoneNumber;
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}

public interface IUserRequest
{
    int UserId { get; set; }
}
public interface IUserContactRequest : IUserRequest
{
    string? Email { get; set; }
    string? PhoneNumber { get; set; }
}