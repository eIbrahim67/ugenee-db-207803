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
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProjectsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // --- Course Projects ---

        [HttpGet("course")]
        public async Task<ActionResult<IEnumerable<CourseProjectDto>>> GetCourseProjects()
        {
            var projects = await _context.CourseProjects.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<CourseProjectDto>>(projects));
        }

        [HttpPost("course")]
        public async Task<ActionResult<CourseProjectDto>> CreateCourseProject(CourseProjectDto projectDto)
        {
            var project = _mapper.Map<CourseProject>(projectDto);
            _context.CourseProjects.Add(project);
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<CourseProjectDto>(project));
        }

        // --- Learning Path Projects ---

        [HttpGet("path")]
        public async Task<ActionResult<IEnumerable<PathProjectDto>>> GetPathProjects()
        {
            var projects = await _context.PathProjects.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<PathProjectDto>>(projects));
        }

        [HttpPost("path")]
        public async Task<ActionResult<PathProjectDto>> CreatePathProject(PathProjectDto projectDto)
        {
            var project = _mapper.Map<PathProject>(projectDto);
            _context.PathProjects.Add(project);
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<PathProjectDto>(project));
        }
    }
}
