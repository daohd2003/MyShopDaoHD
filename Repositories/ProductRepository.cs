using BussinessObjects;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopDbContext _context;

        public ProductRepository(ShopDbContext context) { _context = context; }

        public async Task<bool> CreateProduct(Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null) return false;
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}");
                return false;
            }
        }

        public IQueryable<Product> GetAllProductsAsQueryable()
        {
            return _context.Products;
        }

        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Products
                     .Include(c => c.Category)
                     .Include(c => c.OrderDetails)
                     .FirstOrDefaultAsync(c => c.ProductId == id);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products
                     .Include(c => c.Category)
                     .ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            try
            {
                _context.Entry<Product>(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product: {ex.Message}");
                return false;
            }
        }
    }
}
