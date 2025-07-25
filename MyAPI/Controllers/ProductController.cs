using BussinessObjects;
using BussinessObjects.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Services;

namespace MyAPI.Controllers
{
    [Route("api/products")]
    [Authorize]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("/odata/Products")]
        [EnableQuery]
        public ActionResult<IQueryable<Product>> GetODataProducts()
        {
            var products = _productService.GetProductsAsQueryable();
            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetProductsForList();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllProducts(int id)
        {
            var products = await _productService.GetProductDetails(id);
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO dto)
        {
            var products = await _productService.CreateProduct(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDTO dto)
        {
            var products = await _productService.UpdateProduct(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var products = await _productService.DeleteProduct(id);
            return Ok();
        }
    }
}
