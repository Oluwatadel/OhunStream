using OhunStream.Api.ServiceExtension;
using OhunStream.Infrastructure.Persistence.Extension;
using System.Reflection;

try
{
    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();
    builder.Services.AddSwaggerGen();
    builder.Services.AddRequestHandlers();
    builder.Services.AddValidators();
    builder.Services.AddDatabase(configuration);
    builder.Services.AddRepositories();
    builder.Services.ConfigureApiVersioning();
    builder.Services.ConfigureMvc();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (ReflectionTypeLoadException ex)
{
    Console.WriteLine("ReflectionTypeLoadException: " + ex.Message);
    foreach (var loaderException in ex.LoaderExceptions)
    {
        Console.WriteLine("LoaderException: " + loaderException.Message);
    }
}
catch (Exception ex)
{
    Console.WriteLine("An exception occurred: " + ex.Message);
}
