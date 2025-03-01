using Feijuca.Auth.Api.Tests.Extensions;
using Feijuca.Auth.Api.Tests.Models;

var builder = WebApplication.CreateBuilder(args);

var applicationSettings = builder.Configuration.GetSection("Settings").Get<Settings>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddApiAuthentication(applicationSettings!)
    .AddEndpointsApiExplorer()
    .AddSwagger(applicationSettings!.ServerSettings);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Feijuca.Tests.Api");
    c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
});

app.UseHttpsRedirection()
    .UseAuthorization();

app.MapControllers();

await app.RunAsync();
