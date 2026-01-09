using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuGeneeAPI.Models.Entities
{
    public class EnrollmentRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string Phone { get; set; } = string.Empty;

        public string? DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Required]
        public string EducationLevel { get; set; } = string.Empty;

        [Required]
        public bool TermsAccepted { get; set; }

        public bool IsContacted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class CourseEnrollmentRequest : EnrollmentRequest
    {
        [Required]
        public int CourseId { get; set; }
        
        [ForeignKey("CourseId")]
        public Course Course { get; set; } = null!;
    }

    public class LearningPathEnrollmentRequest : EnrollmentRequest
    {
        [Required]
        public int PathId { get; set; }
        
        [ForeignKey("PathId")]
        public LearningPath LearningPath { get; set; } = null!;
    }
}
