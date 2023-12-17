using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Maasgroep.Exceptions;

namespace Maasgroep.Filters;

public class MaasgroepExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is MaasgroepException httpResponseException)
        {
            context.Result = new ObjectResult(new {
                error = httpResponseException.StatusCode,
                message = httpResponseException.Message,
            })
            {
                StatusCode = httpResponseException.StatusCode
            };

            context.ExceptionHandled = true;
        }
    }
}