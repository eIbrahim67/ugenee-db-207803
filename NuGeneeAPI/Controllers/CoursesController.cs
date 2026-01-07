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
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CoursesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses(
            [FromQuery] string? search, [FromQuery] int? categoryId, 
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructors)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Title.Contains(search) || c.TitleAr.Contains(search));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(c => c.CategoryId == categoryId.Value);
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<CourseDto>>(items);
            
            Response.Headers.Add("X-Total-Count", totalItems.ToString());
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructors)
                .Include(c => c.TableOfContent)
                    .ThenInclude(t => t.Modules)
                        .ThenInclude(m => m.Lessons)
                            .ThenInclude(l => l.Resources)
                .Include(c => c.Projects)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) return NotFound();

            return _mapper.Map<CourseDto>(course);
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<ActionResult<CourseDto>> CreateCourse(CourseDto courseDto)
        {
            var course = _mapper.Map<Course>(courseDto);
            
            // Handle unique logic for TOC if needed
            if (course.TableOfContent == null) {
                course.TableOfContent = new TableOfContent();
            }

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            var result = await _context.Courses
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == course.Id);

            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, _mapper.Map<CourseDto>(result));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> UpdateCourse(int id, CourseDto courseDto)
        {
            if (id != courseDto.Id) return BadRequest();

            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            _mapper.Map(courseDto, course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
