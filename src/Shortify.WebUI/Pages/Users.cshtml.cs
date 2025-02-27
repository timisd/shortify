using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shortify.Common.Contracts.Response;
using Shortify.Common.Misc;

namespace Shortify.WebUI.Pages;

public class UsersModel(ApiClient apiClient, JsonHelper jsonHelper) : PageModel
{
    public PagedResult<GetUserResponse>? PagedResult { get; set; }

    public async Task OnGetAsync()
    {
        var token = GetToken();
        if (string.IsNullOrEmpty(token)) return;

        var response = await apiClient.GetAsync("users", token);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            PagedResult = jsonHelper.Deserialize<PagedResult<GetUserResponse>>(content);
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        var token = GetToken();
        if (!string.IsNullOrEmpty(token))
        {
            var response = await apiClient.DeleteAsync($"users/{id}", token);
        }

        return RedirectToPage();
    }

    private string GetToken()
    {
        return HttpContext.Request.Cookies["AuthToken"] ?? string.Empty;
    }
}