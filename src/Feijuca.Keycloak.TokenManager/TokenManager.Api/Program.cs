using TokenManager.Infra.CrossCutting.Config;
using TokenManager.Infra.CrossCutting.Extensions;
using TokenManager.Infra.CrossCutting.Handlers;

var builder = WebApplication.CreateBuilder(args);
var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Logging
    .ClearProviders()
    .AddFilter("Microsoft", LogLevel.Warning)
    .AddFilter("Microsoft", LogLevel.Critical);

builder.Configuration
    .AddJsonFile("appsettings.json", false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{enviroment}.json", true, reloadOnChange: true)
    .AddEnvironmentVariables();

var applicationSettings = builder.Configuration.GetApplicationSettings(builder.Environment);

builder.Services
    .AddSingleton<ISettings>(applicationSettings)
    .AddControllers();

builder.Services.AddSwaggerGen();
builder.Services
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails()
    .AddApiAuthentication(applicationSettings.AuthSettings)
    .AddLoggingDependency()
    .AddMediator()
    .AddRepositories()
    .AddHttpClients(applicationSettings.AuthSettings)
    .AddEndpointsApiExplorer()    
    .AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins", policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });

var app = builder.Build();

app.UseCors("AllowAllOrigins")
   .UseExceptionHandler()
   .UseSwagger()
   .UseSwaggerUI();

app.UseHttpsRedirection()
   .UseAuthorization();

app.MapControllers();

await app.RunAsync();
