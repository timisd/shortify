using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Shortify.Common.Misc;
using Shortify.WebUI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => { options.LoginPath = "/Login"; });

builder.Configuration.AddEnvironmentVariables();
builder.Services.Configure<WebSettings>(builder.Configuration.GetSection("WebSettings"));

builder.Services.AddSingleton<JsonHelper>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<ApiClient>();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"/root/.aspnet/DataProtection-Keys"))
    .SetApplicationName("shortify-webui");

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.MapGet("{shortUrl}", async (string shortUrl, ApiClient apiClient) =>
{
    var response = await apiClient.RedirectAsync($"/{shortUrl}");

    var fullUrl = (await response.Content.ReadAsStringAsync()).Trim('"');

    return Results.Redirect(fullUrl);
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();