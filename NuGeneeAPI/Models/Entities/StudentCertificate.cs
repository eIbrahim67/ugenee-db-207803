using System;
using System.ComponentModel.DataAnnotations;

namespace NuGeneeAPI.Models.Entities
{
    public class StudentCertificate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string StudentName { get; set; } = string.Empty;
        public string StudentNameAr { get; set; } = string.Empty;

        [Required]
        public string CourseName { get; set; } = string.Empty;
        public string CourseNameAr { get; set; } = string.Empty;

        public string CertificateImage { get; set; } = string.Empty;

        public string Achievement { get; set; } = string.Empty;
        public string AchievementAr { get; set; } = string.Empty;

        public DateTime Date { get; set; }
        public int Order { get; set; }

        // Relationships
        public int? CourseId { get; set; }
        public Course? Course { get; set; }

        public int? LearningPathId { get; set; }
        public LearningPath? LearningPath { get; set; }
    }
}
