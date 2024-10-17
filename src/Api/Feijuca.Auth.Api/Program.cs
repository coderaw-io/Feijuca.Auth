using Feijuca.Auth.Infra.CrossCutting.Config;
using Feijuca.Auth.Infra.CrossCutting.Extensions;
using Feijuca.Auth.Infra.CrossCutting.Handlers;
using Feijuca.Auth.Infra.CrossCutting.Middlewares;
using Feijuca.Auth.Models;

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

var applicationSettings = builder.Configuration.GetSection("Settings").Get<MongoSettings>()!;

builder.Services.AddSwaggerGen();
builder.Services
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails()
    .AddLoggingDependency()
    .AddMediator()
    .AddRepositories()
    .AddServices()
    .AddMongo(applicationSettings)
    .AddApiAuthentication(out AuthSettings authSettings)
    .AddSwagger(authSettings)
    .AddHttpClients(authSettings.AuthServerUrl)
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
   .UseSwaggerUI(c =>
   {
       c.SwaggerEndpoint("/swagger/v1/swagger.json", "Feijuca.Auth.Api");
       c.OAuthClientId(null); //passar valores que vao vir do banco
       c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
   });

app.UseHttpsRedirection()
   .UseAuthorization()
   .UseMiddleware<TenantMiddleware>();

app.MapControllers();

await app.RunAsync();
