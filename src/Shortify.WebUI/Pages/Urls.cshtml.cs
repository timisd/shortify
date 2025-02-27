using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Shortify.Common.Contracts.Response;
using Shortify.Common.Misc;

namespace Shortify.WebUI.Pages;

public class UrlsModel(ApiClient apiClient, JsonHelper jsonHelper, IOptions<WebSettings> options) : PageModel
{
    public string ApiUrl = Environment.GetEnvironmentVariable("WebSettings__ApiUrl") ?? options.Value.ApiUrl;
    public PagedResult<GetUrlResponse>? PagedResult { get; set; }

    public async Task OnGetAsync()
    {
        var token = GetToken();
        if (string.IsNullOrEmpty(token)) return;

        var response = await apiClient.GetAsync("urls", token);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            PagedResult = jsonHelper.Deserialize<PagedResult<GetUrlResponse>>(content);
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        var token = GetToken();
        if (!string.IsNullOrEmpty(token))
        {
            var response = await apiClient.DeleteAsync($"urls/{id}", token);
        }

        return RedirectToPage();
    }

    private string GetToken()
    {
        return HttpContext.Request.Cookies["AuthToken"] ?? string.Empty;
    }
}