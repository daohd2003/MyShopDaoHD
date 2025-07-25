using BussinessObjects.DTOs;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly ShopDbContext _context;
        private readonly JwtService _jwtService;

        public AuthService(ShopDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Password == dto.Password);

            if (user == null)
            {
                return new LoginResponseDTO
                {
                    IsSuccess = false,
                    Message = "Invalid email or password"
                };
            }

            var token = _jwtService.GenerateToken(user.UserId, user.Email);

            return new LoginResponseDTO
            {
                IsSuccess = true,
                Message = "Login successful",
                Email = user.Email,
                UserId = user.UserId,
                Token = token
            };
        }

    }
}
