using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shortify.WebUI.Pages;

public class RegisterModel : PageModel
{
    [BindProperty] public string Email { get; set; } = string.Empty;

    [BindProperty] public string Password { get; set; } = string.Empty;
    [BindProperty] public string ConfirmPassword { get; set; } = string.Empty;

    public async Task<IActionResult> OnPostRegisterAsync()
    {
        var loginData = new { Email, Password };
        var json = JsonSerializer.Serialize(loginData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var client = new HttpClient();
        var response = await client.PostAsync("http://localhost:5134/api/auth/register", content);

        if (response.IsSuccessStatusCode)
            return RedirectToPage("/Login");

        ModelState.AddModelError(string.Empty, "Could not sign up!");
        return Page();
    }
}