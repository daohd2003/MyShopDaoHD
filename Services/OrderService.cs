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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public Task<bool> Order(OrderDTO dto)
        {
            var comment = _mapper.Map<Order>(dto);
            return _orderRepository.Order(comment);
        }
    }
}
