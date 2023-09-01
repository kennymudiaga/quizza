using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quizza.Common.Results;

namespace Quizza.Common.Web;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return result switch
        {
            { IsSuccess: true } => new OkObjectResult(result.Value),
            _ => ToActionResult(result as Result),
        };
    }

    public static IActionResult ToActionResult(this Result result)
    {
        return result switch
        {
            Unauthorized => new UnauthorizedObjectResult(result),
            Forbidden => new ObjectResult(result) { StatusCode = StatusCodes.Status403Forbidden },
            { IsSuccess: true } => new NoContentResult(),
            { IsSuccess: false} => new BadRequestObjectResult(result),
            _ => new ConflictObjectResult(result),
        };
    }
}