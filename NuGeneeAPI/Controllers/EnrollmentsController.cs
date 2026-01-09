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
    public class EnrollmentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public EnrollmentsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // 1. Create Course Enrollment Request
        [HttpPost("course")]
        [AllowAnonymous]
        public async Task<ActionResult<CourseEnrollmentRequestDto>> CreateCourseEnrollment(CourseEnrollmentRequestDto requestDto)
        {
            // Validation: Ensure course exists
            if (!await _context.Courses.AnyAsync(c => c.Id == requestDto.CourseId))
            {
                return BadRequest($"Course with ID {requestDto.CourseId} does not exist.");
            }

            var enrollment = _mapper.Map<CourseEnrollmentRequest>(requestDto);
            enrollment.CreatedAt = DateTime.UtcNow;
            enrollment.IsContacted = false;

            try 
            {
                _context.CourseEnrollmentRequests.Add(enrollment);
                await _context.SaveChangesAsync();
                return Ok(_mapper.Map<CourseEnrollmentRequestDto>(enrollment));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal error: {ex.Message} {(ex.InnerException != null ? " | Inner: " + ex.InnerException.Message : "")}");
            }
        }

        // Get Single Course Enrollment (Admin only)
        [HttpGet("course/{id}")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<ActionResult<CourseEnrollmentRequestDto>> GetCourseEnrollment(int id)
        {
            var request = await _context.CourseEnrollmentRequests
                .Include(r => r.Course)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null) return NotFound();

            return Ok(_mapper.Map<CourseEnrollmentRequestDto>(request));
        }

        // 2. Get All Course Enrollment Requests
        [HttpGet("course")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<ActionResult<IEnumerable<CourseEnrollmentRequestDto>>> GetCourseEnrollments()
        {
            var requests = await _context.CourseEnrollmentRequests
                .Include(r => r.Course)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<CourseEnrollmentRequestDto>>(requests));
        }

        // 3. Create Learning Path Enrollment Request
        [HttpPost("path")]
        [AllowAnonymous]
        public async Task<ActionResult<LearningPathEnrollmentRequestDto>> CreatePathEnrollment(LearningPathEnrollmentRequestDto requestDto)
        {
            // Validation: Ensure path exists
            if (!await _context.LearningPaths.AnyAsync(p => p.Id == requestDto.PathId))
            {
                return BadRequest($"Learning Path with ID {requestDto.PathId} does not exist.");
            }

            var enrollment = _mapper.Map<LearningPathEnrollmentRequest>(requestDto);
            enrollment.CreatedAt = DateTime.UtcNow;
            enrollment.IsContacted = false;

            try 
            {
                _context.LearningPathEnrollmentRequests.Add(enrollment);
                await _context.SaveChangesAsync();
                return Ok(_mapper.Map<LearningPathEnrollmentRequestDto>(enrollment));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal error: {ex.Message} {(ex.InnerException != null ? " | Inner: " + ex.InnerException.Message : "")}");
            }
        }

        // Get Single Path Enrollment (Admin only)
        [HttpGet("path/{id}")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<ActionResult<LearningPathEnrollmentRequestDto>> GetPathEnrollment(int id)
        {
            var request = await _context.LearningPathEnrollmentRequests
                .Include(r => r.LearningPath)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null) return NotFound();

            return Ok(_mapper.Map<LearningPathEnrollmentRequestDto>(request));
        }

        // 4. Get All Learning Path Enrollment Requests
        [HttpGet("path")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<ActionResult<IEnumerable<LearningPathEnrollmentRequestDto>>> GetPathEnrollments()
        {
            var requests = await _context.LearningPathEnrollmentRequests
                .Include(r => r.LearningPath)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<LearningPathEnrollmentRequestDto>>(requests));
        }

        // 5. Mark Enrollment Request as Contacted
        [HttpPatch("{type}/{id}/contacted")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> MarkAsContacted(string type, int id)
        {
            if (type.ToLower() == "course")
            {
                var enrollment = await _context.CourseEnrollmentRequests.FindAsync(id);
                if (enrollment == null) return NotFound();
                enrollment.IsContacted = true;
            }
            else if (type.ToLower() == "path")
            {
                var enrollment = await _context.LearningPathEnrollmentRequests.FindAsync(id);
                if (enrollment == null) return NotFound();
                enrollment.IsContacted = true;
            }
            else
            {
                return BadRequest("Invalid type. Must be 'course' or 'path'.");
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
