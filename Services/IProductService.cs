using BussinessObjects;
using BussinessObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProductsForList();
        Task<ProductDetailDTO?> GetProductDetails(int id);
        Task<bool> CreateProduct(CreateProductDTO dto);
        Task<bool> UpdateProduct(UpdateProductDTO dto);
        Task<bool> DeleteProduct(int id);
        IQueryable<Product> GetProductsAsQueryable();
    }
}
