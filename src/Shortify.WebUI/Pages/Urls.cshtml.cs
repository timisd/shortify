using Microsoft.AspNetCore.Mvc.RazorPages;
using Shortify.Common.Contracts.Response;
using Shortify.Common.Misc;

namespace Shortify.WebUI.Pages;

public class UrlsModel(ApiClient apiClient, JsonHelper jsonHelper) : PageModel
{
    public PagedResult<GetUrlResponse>? PagedResult { get; set; }

    public async Task OnGetAsync()
    {
        var token = HttpContext.Request.Cookies["AuthToken"];
        if (string.IsNullOrEmpty(token)) return;

        var response = await apiClient.GetAsync("urls", token);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            PagedResult = jsonHelper.Deserialize<PagedResult<GetUrlResponse>>(content);
        }
    }
}