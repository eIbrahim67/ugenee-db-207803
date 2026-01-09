using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace NuGeneeAPI.Models.Entities
{
    public class TableOfContent
    {
        [Key, ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public ICollection<CourseModule> Modules { get; set; } = new List<CourseModule>();
    }

    public class CourseModule
    {
        [Key]
        public int Id { get; set; }
        public int Order { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;

        public string Duration { get; set; } = string.Empty;
        public string DurationAr { get; set; } = string.Empty;

        // Parent Reference
        public int TableOfContentId { get; set; }
        public TableOfContent TableOfContent { get; set; } = null!;

        public ICollection<CourseLesson> Lessons { get; set; } = new List<CourseLesson>();
    }

    public class CourseLesson
    {
        [Key]
        public int Id { get; set; }
        public int Order { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;

        public string Type { get; set; } = "video"; // video, article, quiz, etc.
        public string Duration { get; set; } = string.Empty;
        public string DurationAr { get; set; } = string.Empty;

        public string Objectives { get; set; } = string.Empty;
        public string ObjectivesAr { get; set; } = string.Empty;

        public bool IsPreview { get; set; }

        // Parent Reference
        public int ModuleId { get; set; }
        public CourseModule Module { get; set; } = null!;

        public ICollection<LessonResource> Resources { get; set; } = new List<LessonResource>();
    }

    public class LessonResource
    {
        [Key]
        public int Id { get; set; }

        public string Type { get; set; } = "pdf"; // pdf, repo, link, slides
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;

        // Parent Reference
        public int LessonId { get; set; }
        public CourseLesson Lesson { get; set; } = null!;
    }
}
