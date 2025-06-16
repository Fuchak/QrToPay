using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QrToPay.Api.Common.Filters;
public class ValidationFilter : IActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument == null) continue;

            Type validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            
            if (_serviceProvider.GetService(validatorType) is not IValidator validator) continue;

            ValidationResult validationResult = validator.Validate(new ValidationContext<object>(argument));

            if (!validationResult.IsValid)
            {
                context.Result = new BadRequestObjectResult(new { Message = validationResult.Errors });
                return;
            }
        }
    }
    public void OnActionExecuted(ActionExecutedContext context) { }
}