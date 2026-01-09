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
    public class LearningPathsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public LearningPathsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<LearningPathDto>>> GetLearningPaths()
        {
            var paths = await _context.LearningPaths
                .Include(p => p.PathCourses)
                    .ThenInclude(pc => pc.Course)
                .Include(p => p.Projects)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<LearningPathDto>>(paths));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<LearningPathDto>> GetLearningPath(int id)
        {
            var path = await _context.LearningPaths
                .Include(p => p.PathCourses)
                    .ThenInclude(pc => pc.Course)
                .Include(p => p.Projects)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (path == null) return NotFound();

            return Ok(_mapper.Map<LearningPathDto>(path));
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<ActionResult<LearningPathDto>> CreateLearningPath(LearningPathDto pathDto)
        {
            var path = _mapper.Map<LearningPath>(pathDto);
            _context.LearningPaths.Add(path);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLearningPath), new { id = path.Id }, _mapper.Map<LearningPathDto>(path));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> UpdateLearningPath(int id, LearningPathDto pathDto)
        {
            if (id != pathDto.Id) return BadRequest();

            var path = await _context.LearningPaths
                .Include(p => p.PathCourses)
                .Include(p => p.Projects)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (path == null) return NotFound();

            _mapper.Map(pathDto, path);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> DeleteLearningPath(int id)
        {
            var path = await _context.LearningPaths.FindAsync(id);
            if (path == null) return NotFound();

            _context.LearningPaths.Remove(path);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
