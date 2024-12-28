using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shortify.Common.Contracts.Response;

namespace Shortify.WebUI.Pages;

public class UrlsModel : PageModel
{
    public PagedResult<GetUrlResponse>? PagedResult { get; set; }

    public async Task OnGetAsync()
    {
        using var httpClient = new HttpClient();
        var token =
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zaWQiOiI2NDA0ZGExMy1hNDBkLTQ0Y2ItODIzYy1lMzk3NmVkZjBjM2IiLCJlbWFpbCI6ImFkbWluQHVzZXIuZGUiLCJyb2xlIjoiYWRtaW4iLCJuYmYiOjE3MzUzMTYyMDQsImV4cCI6MTczNTMxOTgwNCwiaWF0IjoxNzM1MzE2MjA0LCJpc3MiOiJTaG9ydGlmeSIsImF1ZCI6IlNob3J0aWZ5In0.We5aEiYwVMLKc_DOYmn9ull-P-Hof02SSdwRAIv4OY8";
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.GetAsync("http://localhost:5134/api/urls");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            PagedResult = JsonSerializer.Deserialize<PagedResult<GetUrlResponse>>(content);
        }
    }
}