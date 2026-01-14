using CurriculumService.Api.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using OhunStream.Application.Repositories;
using OhunStream.Infrastructure.Persistence.Repositories;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OhunStream.Api.ServiceExtension
{
    public static class ServiceCollectionsExtension
    {
        public static void ConfigureMvc(this IServiceCollection serviceCollection)
        {
            serviceCollection
               .AddControllers(options =>
               {
                   options.OutputFormatters.RemoveType<StringOutputFormatter>();
                   options.Filters.Add<ValidationFilter>();
                   options.ModelValidatorProviders.Clear();
               })
               .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
               .AddJsonOptions(options =>
               {
                   // Serialize enums as strings in api responses
                   options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                   options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                   options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
               });
        }

    }
}
