using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Portfolio.Api.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<ApiExceptionFilterAttribute> _logger;
    private readonly IHostEnvironment _env;

    public ApiExceptionFilterAttribute(
        ILogger<ApiExceptionFilterAttribute> logger,
        IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public override void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Unhandled exception occurred.");

        var response = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request.",
            Detail = _env.IsDevelopment() ? context.Exception.ToString() : null,
            Instance = context.HttpContext.Request.Path
        };

        if (context.Exception is InvalidOperationException invalidOpEx)
        {
            response.Status = StatusCodes.Status400BadRequest;
            response.Title = "Invalid Operation";
            response.Detail = invalidOpEx.Message;
        }
        else if (context.Exception is SecurityException securityEx)
        {
            response.Status = StatusCodes.Status401Unauthorized;
            response.Title = "Security Error";
            response.Detail = securityEx.Message;
        }
        else if (context.Exception is ValidationException validationEx)
        {
            response.Status = StatusCodes.Status422UnprocessableEntity;
            response.Title = "Validation Error";
            response.Errors = validationEx.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
        }

        context.Result = new ObjectResult(response)
        {
            StatusCode = response.Status
        };

        context.ExceptionHandled = true;
    }
}
