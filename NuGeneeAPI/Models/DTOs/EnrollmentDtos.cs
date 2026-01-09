using System;
using System.ComponentModel.DataAnnotations;

namespace NuGeneeAPI.Models.DTOs
{
    public class EnrollmentRequestDto
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

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

        public bool IsContacted { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class CourseEnrollmentRequestDto : EnrollmentRequestDto
    {
        [Required]
        public int CourseId { get; set; }
        public string? CourseTitle { get; set; }
    }

    public class LearningPathEnrollmentRequestDto : EnrollmentRequestDto
    {
        [Required]
        public int PathId { get; set; }
        public string? PathName { get; set; }
    }
}
