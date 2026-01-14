using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OhunStream.Application.Repositories;
using OhunStream.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhunStream.Infrastructure.Persistence.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ISesionRepository, SessionRepository>();
            return services;
        }

        public static AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetDb
        }
    }
}
