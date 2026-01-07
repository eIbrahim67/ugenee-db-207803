using System.Collections.Generic;

namespace NuGeneeAPI.Models.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
    }

    public class InstructorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string RoleAr { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string BioAr { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public string Expertise { get; set; } = string.Empty;
        public string ExpertiseAr { get; set; } = string.Empty;
        public string LinkedIn { get; set; } = string.Empty;
        public string Twitter { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
    }

    public class CourseDto
    {
        public int Id { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string LevelAr { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string DurationAr { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public bool Active { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public List<InstructorDto> Instructors { get; set; } = new();
        public TableOfContentDto? TableOfContent { get; set; }
    }

    public class TableOfContentDto
    {
        public List<CourseModuleDto> Modules { get; set; } = new();
    }

    public class CourseModuleDto
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Title { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public List<CourseLessonDto> Lessons { get; set; } = new();
    }

    public class CourseLessonDto
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public List<LessonResourceDto> Resources { get; set; } = new();
    }

    public class LessonResourceDto
    {
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    public class AdminDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class LearningPathDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public List<PathCourseDto> PathCourses { get; set; } = new();
    }

    public class PathCourseDto
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public int OrderInPath { get; set; }
        public bool IsOptional { get; set; }
    }

    public class PathProjectDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
    }

    public class CourseProjectDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
    }

    public class StudentCertificateDto
    {
        public string StudentName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
    }
}
