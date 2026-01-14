using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CurriculumService.Api.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    private const string JsonExceptionName = "JsonException";
    private const string JsonExceptionMessage = "An exception occurred parsing the supplied JSON";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ModelState.IsValid)
        {
            await next();
            return;
        }

        var problemDetail = new ValidationProblemDetails(context.ModelState)
        {
            Title = JsonExceptionName,
            Detail = JsonExceptionMessage,
            Status = StatusCodes.Status400BadRequest,
            Instance = context.HttpContext.Request.Path,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };
        var serializerSettings = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };
        context.Result = new JsonResult(problemDetail, serializerSettings)
        {
            StatusCode = 400
        };
    }
}