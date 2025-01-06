using System.Diagnostics;
using FastEndpoints;
using Scalar.AspNetCore;
using Shortify.API.Extensions;
using Shortify.Common.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSettings(builder.Configuration);
builder.Services.AddHelpers();
builder.Services.AddRepositories();

builder.Services.AddFastEndpoints();

builder.Configuration.AddEnvironmentVariables();

var settings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
if (settings == null)
{
    Debug.WriteLine("ApiSettings not found");
    return;
}

builder.Services.AddAuth(settings);
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Shortify API")
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

app.Run();