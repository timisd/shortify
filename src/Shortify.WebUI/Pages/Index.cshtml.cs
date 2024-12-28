using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shortify.Common.Contracts.Requests;
using Shortify.Common.Misc;

namespace Shortify.WebUI.Pages;

public class IndexModel(ApiClient apiClient) : PageModel
{
    [BindProperty] public string OriginalUrl { get; set; } = string.Empty;

    [BindProperty] public string CustomShort { get; set; } = string.Empty;

    public async Task OnPostShortAsync()
    {
        var token = HttpContext.Request.Cookies["AuthToken"];
        if (string.IsNullOrEmpty(token)) return;

        var urlData = CustomShort != string.Empty
            ? new AddUrlRequest(OriginalUrl, CustomShort)
            : new AddUrlRequest(OriginalUrl);

        var json = JsonSerializer.Serialize(urlData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await apiClient.PostAsync("urls", content, token);
    }
}