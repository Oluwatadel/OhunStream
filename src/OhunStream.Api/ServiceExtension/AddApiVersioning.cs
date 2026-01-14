using Microsoft.AspNetCore.Mvc.Versioning;

namespace OhunStream.Api.ServiceExtension
{
    public static class AddApiVersioning
    {
        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
        }


    }
}
