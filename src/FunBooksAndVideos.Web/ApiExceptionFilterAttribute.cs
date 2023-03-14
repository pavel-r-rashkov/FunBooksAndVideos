using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FunBooksAndVideos.Web.CollectionUtils;
using FunBooksAndVideos.Application.Common;

namespace FunBooksAndVideos.Web;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<ApiExceptionFilterAttribute> _logger;
    private readonly Action<ILogger, Exception?> _errorLogger;
    private readonly Action<ILogger, Exception?> _notFoundLogger;
    private readonly Action<ILogger, Exception?> _validationErrorLogger;

    public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
    {
        _logger = logger;
        _errorLogger = LoggerMessage.Define(LogLevel.Error, new EventId(), "Error processing request");
        _notFoundLogger = LoggerMessage.Define(LogLevel.Error, new EventId(), "Resource not found");
        _validationErrorLogger = LoggerMessage.Define(LogLevel.Warning, new EventId(), "Validation error");
    }

    public override void OnException(ExceptionContext context)
    {
        context.ExceptionHandled = true;

        switch (context.Exception)
        {
            case ValidationException ex:
                HandleValidationError(ex, context);
                return;
            case CollectionBindingException ex:
                HandleCollectionBindingException(ex, context);
                return;
            case ResourceNotFoundException ex:
                HandleResourceNotFoundException(ex, context);
                return;
            default:
                HandleUnknownError(context);
                return;
        };
    }

    private void HandleValidationError(ValidationException ex, ExceptionContext context)
    {
        _validationErrorLogger(_logger, ex);

        var errors = ex.Errors
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(g => g.Key, g => g.ToArray());

        context.Result = new UnprocessableEntityObjectResult(new ValidationProblemDetails(errors));
    }

    private void HandleCollectionBindingException(CollectionBindingException ex, ExceptionContext context)
    {
        _validationErrorLogger(_logger, ex);

        var details = new ProblemDetails()
        {
            Detail = ex.Message,
        };

        context.Result = new BadRequestObjectResult(details);
    }

    private void HandleResourceNotFoundException(ResourceNotFoundException ex, ExceptionContext context)
    {
        _notFoundLogger(_logger, ex);

        var details = new ProblemDetails()
        {
            Status = StatusCodes.Status404NotFound,
            Detail = "The resource was not found.",
        };

        context.Result = new NotFoundObjectResult(details);
    }

    private void HandleUnknownError(ExceptionContext context)
    {
        _errorLogger(_logger, context.Exception);

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request.",
        };
        context.Result = new ObjectResult(details);
    }
}
