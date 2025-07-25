using BussinessObjects.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace MyFE.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DetailsModel(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public ProductDetailDTO? Product { get; set; }

        /*public OrderDTO Orders { get; set; } = new();*/

        [TempData]
        public string? SuccessMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = User.FindFirst("Token")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                await HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToPage("/Auth/Login", new { returnUrl = Request.Path });
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var productsResponse = await _httpClient.GetAsync($"products/{Id}");
            if (productsResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                productsResponse.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                await HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToPage("/Auth/Login", new { returnUrl = Request.Path });
            }
            if (productsResponse.IsSuccessStatusCode)
            {
                Product = await productsResponse.Content.ReadFromJsonAsync<ProductDetailDTO>();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = User.FindFirst("Token")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                await HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToPage("/Auth/Login", new { returnUrl = Request.Path });
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                ModelState.AddModelError("", "You need to log in to order.");
                await OnGetAsync();
                return Page();
            }

            /*Orders.UserId = userId;
            Orders.ProductId = Id;
            Orders.OrderDate = DateTime.Now;

            var res = await _httpClient.PostAsJsonAsync("orders", Orders);
            if (res.IsSuccessStatusCode)
            {
                SuccessMessage = "Order added successfully!";
                return RedirectToPage(new { id = Id });
            }
            else if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                     res.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                await HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToPage("/Auth/Login", new { returnUrl = Request.Path });
            }

            ModelState.AddModelError("", "Failed to add orders");*/
            await OnGetAsync();
            return Page();
        }
    }
}
