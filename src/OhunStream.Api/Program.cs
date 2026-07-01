using OhunStream.Api.ServiceExtension;
using OhunStream.Infrastructure.Persistence.Extension;
using System.Reflection;

try
{
    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;

    // Add services to the container.

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    });

    builder.Services.AddControllers();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddSwagger();
    builder.Services.AddRequestHandlers();
    builder.Services.AddValidators();
    builder.Services.AddDatabase(configuration);
    builder.Services.AddRepositories();
    builder.Services.ConfigureApiVersioning();
    builder.Services.ConfigureMvc();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseHttpsRedirection();
    app.UseSwaggerAndSwaggerUI();

    app.UseCors();
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
