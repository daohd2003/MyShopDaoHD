using BussinessObjects.DTOs;
using DataAccessObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;

namespace MyAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ShopDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(ShopDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login(LoginRequestDTO dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || user.Password != dto.Password)
            {
                return Unauthorized(new LoginResponseDTO
                {
                    IsSuccess = false,
                    Message = "Invalid email or password"
                });
            }

            var token = _jwtService.GenerateToken(user.UserId, user.Email);

            return Ok(new LoginResponseDTO
            {
                IsSuccess = true,
                Message = "Login successful",
                Email = user.Email,
                UserId = user.UserId,
                Token = token
            });
        }
    }
}
