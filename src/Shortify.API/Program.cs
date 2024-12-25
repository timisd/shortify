using System.Diagnostics;
using System.Text;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Shortify.API.Extensions;
using Shortify.Common.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSettings(builder.Configuration);
builder.Services.AddHelpers();
builder.Services.AddRepositories();

builder.Services.AddFastEndpoints();

var settings = builder.Configuration.GetSection("GeneralSettings").Get<GeneralSettings>();
if (settings == null)
{
    Debug.WriteLine("GeneralSettings not found");
    return;
}

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "Shortify",
            ValidAudience = "Shortify",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.EncryptionKey))
        };
    });
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