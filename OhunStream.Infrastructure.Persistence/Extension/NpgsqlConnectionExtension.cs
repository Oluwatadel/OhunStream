using Microsoft.Extensions.Configuration;
using Npgsql;

namespace OhunStream.Infrastructure.Persistence.Extension
{
    public static class NpgsqlConnectionExtension
    {
        public static NpgsqlConnectionStringBuilder GetDbConnectionStringBuilder(this IConfiguration configuration)
        {
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = configuration.GetValue<string>("DB_HOST"),
                Database = configuration.GetValue<string>("DB_NAME"),
                Password = configuration.GetValue<string>("DB_PASSWORD"),
                Username = configuration.GetValue<string>("DB_USERNAME"),
                IncludeErrorDetail = true,
                Pooling = true,
                Port = configuration.GetValue<int?>("DB_PORT") ?? 5432
            };
            return builder;
        }
    }
}
