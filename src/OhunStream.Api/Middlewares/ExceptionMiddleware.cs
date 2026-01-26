using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OhunStream.Api.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate request,
        IHostEnvironment environment, ILogger<ExceptionMiddleware> logger)
    {

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await request(context);
            }
            catch (ValidationException exception)
            {
                logger.LogInformation("A validation exception has occurred while executing the request: {ErrorMessage}",
                exception.Message);
                var errors = exception.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(failureGroup => failureGroup.Key,
                    failureGroup => failureGroup.Select(f => f.ErrorMessage).ToArray());
                var problemDetail = new ValidationProblemDetails(errors)
                {
                    Title = "ValidationFailure",
                    Detail = "One or more validation errors occurred.",
                    Status = StatusCodes.Status400BadRequest,
                    Instance = context.Request.Path,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                };
                await Results.Json(problemDetail,
                    _jsonSerializerOptions,
                    statusCode: StatusCodes.Status400BadRequest
                    ).ExecuteAsync(context);

            }
            catch (DomainException exception)
            {
                logger.LogWarning(message: exception.Message);
                var (code, title) = exception switch
                {
                    //EntityNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                    //BusinessRuleViolationException => (StatusCodes.Status400BadRequest, "Business Rule Violation"),
                    DomainValidationException => (StatusCodes.Status400BadRequest, "Validation Error"),
                    //ConcurrencyException => (StatusCodes.Status409Conflict, "Conflict"),
                    // DomainAuthorizationException => (StatusCodes.Status403Forbidden, "Forbidden"),
                    _ => (StatusCodes.Status400BadRequest, "Domain Error")
                };
                var error = _errors[code];
                var problemDetail = new ProblemDetails
                {
                    Title = title,
                    Detail = exception.Message,
                    Status = code,
                    Instance = context.Request.Path,
                    Type = error.Type
                };
                await Results.Json(problemDetail,
                    options: _jsonSerializerOptions,
                    statusCode: code
                ).ExecuteAsync(context);
            }
            //catch (AppException exception)
            //{
            //    logger.LogWarning(message: exception.Message);
            //    var error = _errors[exception.StatusCode];
            //    var problemDetail = new ProblemDetails
            //    {
            //        Title = error.Title,
            //        Detail = exception.Message,
            //        Status = exception.StatusCode,
            //        Instance = context.Request.Path,
            //        Type = error.Type
            //    };
            //    await Results.Json(problemDetail,
            //        statusCode: exception.StatusCode,
            //        options: _jsonSerializerOptions
            //    ).ExecuteAsync(context);
            //}
            catch (Exception exception)
            {
                logger.LogError(exception: exception, "An unhandled exception has occurred while executing the request.");
                var error = _errors[500];
                var problem = new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = environment.IsDevelopment() ? exception.Message : error.Title,
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = context.Request.Path,
                    Type = error.Type
                };
                await Results.Json(
                    problem,
                    options: _jsonSerializerOptions,
                    statusCode: StatusCodes.Status500InternalServerError
                ).ExecuteAsync(context);
            }
        }
    private readonly Dictionary<int, (string Type, string Title)> _errors = new()
    {
        [400] =
        (
            "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            "Bad Request"
        ),

        [401] =
        (
            "https://tools.ietf.org/html/rfc7235#section-3.1",
            "Unauthorized"
        ),

        [403] =
        (
            "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            "Forbidden"
        ),

        [404] =
        (
            "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            "Not Found"
        ),

        [406] =
        (
            "https://tools.ietf.org/html/rfc7231#section-6.5.6",
            "Not Acceptable"
        ),

        [409] =
        (
            "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            "Conflict"
        ),

        [415] =
        (
            "https://tools.ietf.org/html/rfc7231#section-6.5.13",
            "Unsupported Media Type"
        ),

        [422] =
        (
            "https://tools.ietf.org/html/rfc4918#section-11.2",
            "Unprocessable Entity"
        ),

        [500] =
        (
            "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            "An error occurred while processing your request."
        ),
    };
    }
}
