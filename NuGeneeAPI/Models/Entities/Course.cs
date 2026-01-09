using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuGeneeAPI.Models.Entities
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        public string Icon { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string TitleAr { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;

        public string Overview { get; set; } = string.Empty;
        public string OverviewAr { get; set; } = string.Empty;

        public string Level { get; set; } = string.Empty;
        public string LevelAr { get; set; } = string.Empty;

        public string Duration { get; set; } = string.Empty;
        public string DurationAr { get; set; } = string.Empty;

        public string Schedule { get; set; } = string.Empty;
        public string ScheduleAr { get; set; } = string.Empty;

        public List<string> Skills { get; set; } = new();
        public List<string> SkillsAr { get; set; } = new();

        public List<string> Prerequisites { get; set; } = new();
        public List<string> PrerequisitesAr { get; set; } = new();

        public List<string> Tools { get; set; } = new();

        public List<string> Objectives { get; set; } = new();
        public List<string> ObjectivesAr { get; set; } = new();

        public string Certification { get; set; } = string.Empty;
        public string CertificationAr { get; set; } = string.Empty;

        public List<string> TargetAudience { get; set; } = new();
        public List<string> TargetAudienceAr { get; set; } = new();

        public string Color { get; set; } = string.Empty;

        public bool Active { get; set; } = true;

        // Foreign Keys
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public int? InstructorId { get; set; }
        public Instructor? MainInstructor { get; set; }

        // Relationships
        public ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
        public ICollection<PathCourse> PathCourses { get; set; } = new List<PathCourse>();
        public ICollection<CourseProject> Projects { get; set; } = new List<CourseProject>();

        // Navigation to Content
        public TableOfContent TableOfContent { get; set; } = null!;
    }
}
