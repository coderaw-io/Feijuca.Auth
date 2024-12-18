using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Infra.CrossCutting.Extensions;
using Feijuca.Auth.Infra.CrossCutting.Handlers;
using Feijuca.Auth.Infra.CrossCutting.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Configuration
    .AddJsonFile("appsettings.json", false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{enviroment}.json", true, reloadOnChange: true)
    .AddEnvironmentVariables();

var applicationSettings = builder.Configuration.GetApplicationSettings(builder.Environment);

builder.Services
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails()
    .AddMediator()
    .AddRepositories()
    .AddValidators()
    .AddServices()
    .AddMongo(applicationSettings)
    .AddApiAuthentication(out KeycloakSettings KeycloakSettings)
    .AddEndpointsApiExplorer()
    .AddSwagger(KeycloakSettings)
    .AddHttpClients(KeycloakSettings?.ServerSettings.Url)
    .ConfigureValidationErrorResponses()
    .AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins", policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    })
    .AddControllers();

var app = builder.Build();

app.UseCors("AllowAllOrigins")
   .UseExceptionHandler()
   .UseSwagger()
   .UseSwaggerUI(c =>
   {
       c.SwaggerEndpoint("/swagger/v1/swagger.json", "Feijuca.Auth.Api");
       c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
   });

if (KeycloakSettings?.Realms?.Any() ?? false)
{
    app.UseAuthorization()
       .UseMiddleware<TenantMiddleware>();
}

app.UseHttpsRedirection()
   .UseHttpsRedirection()
   .UseMiddleware<ConfigValidationMiddleware>();

app.MapControllers();

await app.RunAsync();
