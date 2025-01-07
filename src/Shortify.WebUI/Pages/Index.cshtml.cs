using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Shortify.Common.Contracts.Requests;
using Shortify.Common.Contracts.Response;
using Shortify.Common.Misc;

namespace Shortify.WebUI.Pages;

public class IndexModel(ApiClient apiClient, JsonHelper jsonHelper, IOptions<WebSettings> options) : PageModel
{
    [BindProperty]
    [RegularExpression(@"^(https?:\/\/)?(www\.)?([a-zA-Z0-9\-]+)(\.[a-zA-Z]{2,})(\/[^\s]*)?$",
        ErrorMessage = "Please enter a valid URL ending with a domain.")]
    public string OriginalUrl { get; set; } = string.Empty;

    [BindProperty] public string CustomShort { get; set; } = string.Empty;

    [BindProperty] public string ShortenedUrl { get; set; } = string.Empty;

    [BindProperty] public string ErrorMessage { get; set; } = string.Empty;

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
        var responseObj = jsonHelper.Deserialize<BaseResponse>(responseContent);

        if (responseObj is { Success: true })
        {
            var shortUrl = jsonHelper.Deserialize<AddUrlResponse>(responseContent);
            if (shortUrl is not null)
            {
                var request = HttpContext.Request;
                var currentUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
                ShortenedUrl = $"{currentUrl}/{shortUrl.ShortLink}";
            }
        }
        else
        {
            ErrorMessage = responseObj?.Message ?? "An error occurred while shortening the URL.";
        }
    }
}