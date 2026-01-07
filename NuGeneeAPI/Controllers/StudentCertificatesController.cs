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
    public class StudentCertificatesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public StudentCertificatesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentCertificateDto>>> GetCertificates()
        {
            var certs = await _context.StudentCertificates.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<StudentCertificateDto>>(certs));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentCertificateDto>> GetCertificate(int id)
        {
            var cert = await _context.StudentCertificates.FindAsync(id);
            if (cert == null) return NotFound();
            return Ok(_mapper.Map<StudentCertificateDto>(cert));
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<ActionResult<StudentCertificateDto>> CreateCertificate(StudentCertificateDto certDto)
        {
            var cert = _mapper.Map<StudentCertificate>(certDto);
            _context.StudentCertificates.Add(cert);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCertificate), new { id = cert.Id }, _mapper.Map<StudentCertificateDto>(cert));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> UpdateCertificate(int id, StudentCertificateDto certDto)
        {
            // Note: DTO for Certificate currently doesn't have Id, I should probably add it or handle it.
            // But looking at AllDtos.cs, StudentCertificateDto is minimal.
            // I'll assume the client sends the ID in the URL.
            
            var cert = await _context.StudentCertificates.FindAsync(id);
            if (cert == null) return NotFound();

            _mapper.Map(certDto, cert);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> DeleteCertificate(int id)
        {
            var cert = await _context.StudentCertificates.FindAsync(id);
            if (cert == null) return NotFound();

            _context.StudentCertificates.Remove(cert);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
