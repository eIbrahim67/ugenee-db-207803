using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGeneeAPI.Data;
using NuGeneeAPI.Models.DTOs;

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
    }
}
