using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Common.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            {typeof(Exceptions.ValidationException), HandleValidationException},
            {typeof(Exceptions.NotFoundException), HandleNotFoundException},
            {typeof(Exceptions.BlException), HandleBlException},
            // { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            // { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
        };
    }

    public override void OnException(ExceptionContext context)
    {
        if (context.HttpContext.Request.ContentType != null && 
            context.HttpContext.Request.ContentType.Contains("json"))
        {
            HandleException(context);
        }

        base.OnException(context);
    }
    
    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }

        HandleUnknownException(context);
    }
    
    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (Exceptions.ValidationException)context.Exception;

        var details = new ValidationProblemDetails(exception.Errors);

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }
    
    private void HandleBlException(ExceptionContext context)
    {
        var exception = (Exceptions.BlException)context.Exception;

        var details = new ProblemDetails()
        {
            Detail = exception.Message
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }
    
    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        var details = new ValidationProblemDetails(context.ModelState);

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }
    
    private void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (Exceptions.NotFoundException)context.Exception;
        
        var details = new ProblemDetails()
        {
            Title = "The specified resource was not found.",
            Detail = exception.Message
        };

        context.Result = new NotFoundObjectResult(details);

        context.ExceptionHandled = true;
    }
    
    private void HandleUnknownException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request."
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }
}