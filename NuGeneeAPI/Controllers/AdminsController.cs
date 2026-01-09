using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGeneeAPI.Data;
using NuGeneeAPI.Models.DTOs;
using NuGeneeAPI.Models.Entities;

namespace NuGeneeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Super Admin")]
    public class AdminsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AdminsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminDto>>> GetAdmins()
        {
            var admins = await _context.Admins.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<AdminDto>>(admins));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdminDto>> GetAdmin(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null) return NotFound();
            return Ok(_mapper.Map<AdminDto>(admin));
        }

        [HttpPost]
        public async Task<ActionResult<AdminDto>> CreateAdmin(AdminDto adminDto)
        {
            // Note: In real scenarios, you'd need the password here.
            // For now, we'll assume a default password or handle it via a specialized DTO.
            var admin = _mapper.Map<Admin>(adminDto);
            
            string passwordToHash = !string.IsNullOrEmpty(adminDto.Password) 
                ? adminDto.Password 
                : "DefaultPassword123!";

            admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordToHash);
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAdmin), new { id = admin.Id }, _mapper.Map<AdminDto>(admin));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdmin(int id, AdminDto adminDto)
        {
            if (id != adminDto.Id) return BadRequest();

            var admin = await _context.Admins.FindAsync(id);
            if (admin == null) return NotFound();

            _mapper.Map(adminDto, admin);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null) return NotFound();

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
