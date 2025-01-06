using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shortify.Common.Contracts.Requests;
using Shortify.Common.Misc;

namespace Shortify.WebUI.Pages;

public class IndexModel(ApiClient apiClient) : PageModel
{
    [BindProperty]
    [RegularExpression(@"^(https?:\/\/)?(www\.)?([a-zA-Z0-9\-]+)(\.[a-zA-Z]{2,})(\/[^\s]*)?$",
        ErrorMessage = "Please enter a valid URL ending with a domain.")]
    public string OriginalUrl { get; set; } = string.Empty;

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

        var response = await apiClient.PostAsync("urls", content, token);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            OriginalUrl = string.Empty;
            CustomShort = string.Empty;
        }
    }
}