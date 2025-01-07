using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace Shortify.WebUI;

public class ApiClient(IHttpClientFactory clientFactory, IOptions<WebSettings> options)
{
    public async Task<HttpResponseMessage> GetAsync(string endpoint, string? token = null)
    {
        var client = clientFactory.CreateClient();
        client.BaseAddress = new Uri(GetBaseApiUrl() + "/api");

        if (token != null) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await client.GetAsync(endpoint);
    }

    public async Task<HttpResponseMessage> PostAsync(string endpoint, StringContent content, string? token = null)
    {
        var client = clientFactory.CreateClient();
        client.BaseAddress = new Uri(GetBaseApiUrl() + "/api");

        if (token != null) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await client.PostAsync(endpoint, content);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string endpoint, string? token = null)
    {
        var client = clientFactory.CreateClient();
        client.BaseAddress = new Uri(GetBaseApiUrl() + "/api");

        if (token != null) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await client.DeleteAsync(endpoint);
    }

    public async Task<HttpResponseMessage> RedirectAsync(string endpoint, string? token = null)
    {
        var client = clientFactory.CreateClient();
        client.BaseAddress = new Uri(GetBaseApiUrl());

        if (token != null) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await client.GetAsync(endpoint);
    }

    private string GetBaseApiUrl()
    {
        return Environment.GetEnvironmentVariable("WebSettings__ApiUrl") ?? options.Value.ApiUrl;
    }
}