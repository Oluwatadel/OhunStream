using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OhunStream.Application.Repositories;
using OhunStream.Infrastructure.Persistence.Persistence;
using OhunStream.Infrastructure.Persistence.Repositories;

namespace OhunStream.Infrastructure.Persistence.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ISesionRepository, SessionRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetDbConnectionStringBuilder().ToString();
            return services
                .AddDbContext<OhunStreamDbContext>(options =>
                    options.UseNpgsql(connectionString,
                        action => action.MigrationsAssembly(typeof(OhunStreamDbContext).Assembly.FullName)
                        .EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null)));
        }

    }
}
