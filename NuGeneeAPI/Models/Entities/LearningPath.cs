using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NuGeneeAPI.Models.Entities
{
    public class LearningPath
    {
        [Key]
        public int Id { get; set; }

        public string Icon { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;

        public string Subtitle { get; set; } = string.Empty;
        public string SubtitleAr { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;

        public string Goal { get; set; } = string.Empty;
        public string GoalAr { get; set; } = string.Empty;

        public string Duration { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        public string Outcomes { get; set; } = string.Empty;
        public string OutcomesAr { get; set; } = string.Empty;

        public bool IsCustomizable { get; set; }
        public bool Featured { get; set; }

        // Relationships
        public ICollection<PathCourse> PathCourses { get; set; } = new List<PathCourse>();
        public ICollection<PathProject> Projects { get; set; } = new List<PathProject>();
    }

    public class PathCourse
    {
        public int PathId { get; set; }
        public LearningPath LearningPath { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public int OrderInPath { get; set; }
        public bool IsOptional { get; set; }
    }

    public class PathProject
    {
        [Key]
        public int Id { get; set; }

        public int PathId { get; set; }
        public LearningPath LearningPath { get; set; } = null!;

        [Required]
        public string Title { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}
