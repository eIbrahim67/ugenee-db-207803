using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGeneeAPI.Data;
using NuGeneeAPI.Models.DTOs;
using NuGeneeAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace NuGeneeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthController(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(x => x.Email == loginDto.Email);

            if (admin == null) return Unauthorized("Invalid email");

            var result = BCrypt.Net.BCrypt.Verify(loginDto.Password, admin.PasswordHash);

            if (!result) return Unauthorized("Invalid password");

            return Ok(new LoginResponseDto
            {
                Token = _tokenService.CreateToken(admin),
                Email = admin.Email,
                Name = admin.Name,
                NameAr = admin.NameAr,
                Role = admin.Role
            });
        }
    }

    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
