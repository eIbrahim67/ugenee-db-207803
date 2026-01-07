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
    public class InstructorsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public InstructorsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<InstructorDto>>> GetInstructors()
        {
            var instructors = await _context.Instructors.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<InstructorDto>>(instructors));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<InstructorDto>> GetInstructor(int id)
        {
            var instructor = await _context.Instructors
                .Include(i => i.Courses)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instructor == null) return NotFound();

            return Ok(_mapper.Map<InstructorDto>(instructor));
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<ActionResult<InstructorDto>> CreateInstructor(InstructorDto instructorDto)
        {
            var instructor = _mapper.Map<Instructor>(instructorDto);
            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInstructor), new { id = instructor.Id }, _mapper.Map<InstructorDto>(instructor));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> UpdateInstructor(int id, InstructorDto instructorDto)
        {
            if (id != instructorDto.Id) return BadRequest();

            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null) return NotFound();

            _mapper.Map(instructorDto, instructor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> DeleteInstructor(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null) return NotFound();

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
