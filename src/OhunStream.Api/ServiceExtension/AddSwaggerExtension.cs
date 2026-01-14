using Microsoft.OpenApi;
using System.Security.Cryptography.Xml;

namespace OhunStream.Api.ServiceExtension
{
    public static class AddSwaggerExtension
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new()
                    {
                        Title = "OhunStream Service",
                        Version = "v1",
                        Contact = new()
                        {
                            Name = "Adelesi Oluwatobi",
                            Email = "oluwatobiadelesi@gmail.com"
                        }
                    }
                );
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter Bearer follow by Token(e.g Bearer ey...)"
                });
            });

            return services;
        }
    }
}
