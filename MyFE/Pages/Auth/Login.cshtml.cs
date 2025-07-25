using BussinessObjects.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace MyFE.Pages.Auth
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginRequestDTO LoginRequest { get; set; } = new();

        public string? ErrorMessage { get; set; }

        private readonly HttpClient _httpClient;

        public LoginModel(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", LoginRequest);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

                if (result != null && result.IsSuccess && !string.IsNullOrEmpty(result.Token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(result.Token);

                    var claims = jwtToken.Claims.ToList();
                    claims.Add(new Claim("Token", result.Token));
                    var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("MyCookieAuth", principal, new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = jwtToken.ValidTo
                    });

                    return RedirectToPage("/Index");
                }

                ErrorMessage = result?.Message ?? "Đăng nhập thất bại: Không nhận được token hoặc token không hợp lệ.";
                return Page();
            }

            ErrorMessage = "Thông tin đăng nhập không hợp lệ hoặc lỗi máy chủ.";
            return Page();
        }
    }
}