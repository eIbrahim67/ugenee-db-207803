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
    [Authorize(Roles = "Super Admin,Admin")]
    public class CourseContentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CourseContentController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // --- Modules ---

        [HttpGet("modules/{id}")]
        public async Task<ActionResult<CourseModuleDto>> GetModule(int id)
        {
            var module = await _context.CourseModules
                .Include(m => m.Lessons)
                    .ThenInclude(l => l.Resources)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (module == null) return NotFound();
            return Ok(_mapper.Map<CourseModuleDto>(module));
        }

        [HttpPost("modules")]
        public async Task<ActionResult<CourseModuleDto>> CreateModule(CourseModuleDto moduleDto)
        {
            var module = _mapper.Map<CourseModule>(moduleDto);
            _context.CourseModules.Add(module);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetModule), new { id = module.Id }, _mapper.Map<CourseModuleDto>(module));
        }

        [HttpPut("modules/{id}")]
        public async Task<IActionResult> UpdateModule(int id, CourseModuleDto moduleDto)
        {
            if (id != moduleDto.Id) return BadRequest();
            var module = await _context.CourseModules.FindAsync(id);
            if (module == null) return NotFound();
            _mapper.Map(moduleDto, module);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // --- Lessons ---

        [HttpGet("lessons/{id}")]
        public async Task<ActionResult<CourseLessonDto>> GetLesson(int id)
        {
            var lesson = await _context.CourseLessons
                .Include(l => l.Resources)
                .FirstOrDefaultAsync(l => l.Id == id);
            if (lesson == null) return NotFound();
            return Ok(_mapper.Map<CourseLessonDto>(lesson));
        }

        [HttpPost("lessons")]
        public async Task<ActionResult<CourseLessonDto>> CreateLesson(CourseLessonDto lessonDto)
        {
            var lesson = _mapper.Map<CourseLesson>(lessonDto);
            _context.CourseLessons.Add(lesson);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLesson), new { id = lesson.Id }, _mapper.Map<CourseLessonDto>(lesson));
        }
    }
}
