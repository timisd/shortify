using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shortify.Common.Contracts.Response;
using Shortify.Common.Misc;

namespace Shortify.WebUI.Pages;

public class RegisterModel(ApiClient apiClient, JsonHelper jsonHelper) : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [RegularExpression(@"^(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-zA-Z]).{8,}$",
        ErrorMessage = "Password must contain at least one number and one special character.")]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Confirm Password is required.")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public string ValidationScript { get; private set; } = string.Empty;

    public void OnGet()
    {
        GenerateValidationScript();
    }

    public async Task<IActionResult> OnPostRegisterAsync()
    {
        if (!ModelState.IsValid) return Page();

        var loginData = new { Email, Password };
        var json = JsonSerializer.Serialize(loginData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var client = new HttpClient();
        var response = await apiClient.PostAsync("auth/register", content);
        var responseContent = await response.Content.ReadAsStringAsync();
        var baseResponse = jsonHelper.Deserialize<BaseResponse>(responseContent);

        if (response.IsSuccessStatusCode)
            return RedirectToPage("/Login");

        if (response.StatusCode == HttpStatusCode.Conflict)
            ModelState.AddModelError("Email", baseResponse!.Message ?? string.Empty);
        else
            ModelState.AddModelError(string.Empty, "Could not sign up!");

        return Page();
    }

    private void GenerateValidationScript()
    {
        ValidationScript = @"
            <script>
                function validateEmail() {
                    const email = document.getElementById('email').value;
                    const emailField = document.getElementById('email');
                    const validEmail = document.getElementById('valid-email');
                    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                    const isValid = emailPattern.test(email);

                    if (isValid) {
                        validEmail.style.display = 'none';
                        emailField.classList.remove('border-danger');
                        emailField.classList.add('border-success');
                    } else {
                        validEmail.style.display = 'block';
                        emailField.classList.remove('border-success');
                        emailField.classList.add('border-danger');
                    }

                    validateForm();
                }

                function validatePassword() {
    const password = document.getElementById('password').value;
    const confirmPassword = document.getElementById('confirmPassword').value;
    const minLength = document.getElementById('min-length');
    const number = document.getElementById('number');
    const specialChar = document.getElementById('special-char');
    const matchPassword = document.getElementById('match-password');
    const passwordMatchError = document.getElementById('password-match-error');
    const registerButton = document.querySelector('button[type=""submit""]');

    let isValid = true;

    if (password.length >= 8) {
        minLength.classList.remove('text-danger');
        minLength.classList.add('text-success');
    } else {
        minLength.classList.remove('text-success');
        minLength.classList.add('text-danger');
        isValid = false;
    }

    if (/\d/.test(password)) {
        number.classList.remove('text-danger');
        number.classList.add('text-success');
    } else {
        number.classList.remove('text-success');
        number.classList.add('text-danger');
        isValid = false;
    }

    if (/[!#$%^&*]/.test(password)) {
        specialChar.classList.remove('text-danger');
        specialChar.classList.add('text-success');
    } else {
        specialChar.classList.remove('text-success');
        specialChar.classList.add('text-danger');
        isValid = false;
    }

    if (password === confirmPassword) {
        matchPassword.classList.remove('text-danger');
        matchPassword.classList.add('text-success');
        passwordMatchError.style.display = 'none';
    } else {
        matchPassword.classList.remove('text-success');
        matchPassword.classList.add('text-danger');
        passwordMatchError.style.display = 'block';
        isValid = false;
    }

    validateForm();
}

                function validateForm() {
                    const emailValid = document.getElementById('valid-email').style.display === 'none';
                    const passwordValid = document.getElementById('min-length').classList.contains('text-success') &&
                        document.getElementById('number').classList.contains('text-success') &&
                        document.getElementById('special-char').classList.contains('text-success') &&
                        document.getElementById('match-password').classList.contains('text-success');
                    const registerButton = document.querySelector('button[type=""submit""]');

                    registerButton.disabled = !(emailValid && passwordValid);
                }

                document.addEventListener('DOMContentLoaded', () => {
                    validateEmail();
                    validatePassword();
                });
            </script>";
    }
}