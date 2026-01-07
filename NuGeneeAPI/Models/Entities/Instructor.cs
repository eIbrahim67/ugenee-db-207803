using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NuGeneeAPI.Models.Entities
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string NameAr { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Role { get; set; } = string.Empty;

        [MaxLength(200)]
        public string RoleAr { get; set; } = string.Empty;

        public string Bio { get; set; } = string.Empty;
        public string BioAr { get; set; } = string.Empty;

        public string Photo { get; set; } = string.Empty; // URL

        public string Expertise { get; set; } = string.Empty;
        public string ExpertiseAr { get; set; } = string.Empty;

        // Social Links (stored as JSON or separate fields, keeping it simple as requested)
        public string LinkedIn { get; set; } = string.Empty;
        public string Twitter { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;

        // Many-to-Many Relationship with Course
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
