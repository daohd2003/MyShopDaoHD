using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.DTOs
{
    public class ProductDetailDTO
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string CategoryName { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<OrderDetailDTO> OrderDetails { get; set; }
    }
}
