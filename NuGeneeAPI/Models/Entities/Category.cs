using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NuGeneeAPI.Models.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string TitleAr { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;

        // Navigation property
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
