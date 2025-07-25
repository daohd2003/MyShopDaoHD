using AutoMapper;
using BussinessObjects;
using BussinessObjects.DTOs;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateProduct(CreateProductDTO dto)
        {
            var product = _mapper.Map<Product>(dto);
            return await _productRepository.CreateProduct(product);
        }

        public async Task<bool> DeleteProduct(int id)
        {
            return await _productRepository.DeleteProduct(id);
        }

        public async Task<ProductDetailDTO?> GetProductDetails(int id)
        {
            var product = await _productRepository.GetProductById(id);
            var dto = _mapper.Map<ProductDetailDTO?>(product);
            return dto;
        }

        public IQueryable<Product> GetProductsAsQueryable()
        {
            return _productRepository.GetAllProductsAsQueryable();
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsForList()
        {
            var products = await _productRepository.GetProducts();
            var dtos = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return dtos;
        }

        public async Task<bool> UpdateProduct(UpdateProductDTO dto)
        {
            var product = _mapper.Map<Product>(dto);
            return await _productRepository.UpdateProduct(product);
        }
    }
}
