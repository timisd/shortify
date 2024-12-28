using System.Net.Http.Headers;

namespace Shortify.Common.Misc;

public class ApiClient(IHttpClientFactory clientFactory)
{
    private const string BaseApiUrl = "http://localhost:5134/api/";

    public async Task<HttpResponseMessage> GetAsync(string endpoint, string? token = null)
    {
        var client = clientFactory.CreateClient();
        client.BaseAddress = new Uri(BaseApiUrl);

        if (token != null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        
        return await client.GetAsync(endpoint);
    }
    
    public async Task<HttpResponseMessage> PostAsync(string endpoint, StringContent content, string? token = null)
    {
        var client = clientFactory.CreateClient();
        client.BaseAddress = new Uri(BaseApiUrl);

        if (token != null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        
        return await client.PostAsync(endpoint, content);
    }
}