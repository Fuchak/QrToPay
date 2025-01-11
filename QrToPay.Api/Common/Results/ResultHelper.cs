using Microsoft.AspNetCore.Mvc;
using QrToPay.Api.Common.Enums;

namespace QrToPay.Api.Common.Results;

public static class ResultHelper
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return result.IsSuccess
            ? new OkObjectResult(result.Value)
            : result.ErrorType switch
            {
                ErrorType.NotFound => new NotFoundObjectResult
                    (new { Message = result.Error }),
                ErrorType.NotVerified => new ObjectResult
                    (new { Message = result.Error }) 
                    { StatusCode = StatusCodes.Status403Forbidden },
                ErrorType.BadRequest => new BadRequestObjectResult
                    (new { Message = result.Error }),
                ErrorType.Unauthorized => new UnauthorizedObjectResult
                    (new { Message = result.Error }) 
                    { StatusCode = StatusCodes.Status401Unauthorized },
                _ => new ObjectResult
                    (new { Message = result.Error })
                    { StatusCode = StatusCodes.Status500InternalServerError }
            };
    }
}