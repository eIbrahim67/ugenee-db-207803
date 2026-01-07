using System.ComponentModel.DataAnnotations;

namespace NuGeneeAPI.Models.Entities
{
    public class CourseProject
    {
        [Key]
        public int Id { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        [Required]
        public string Title { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty; // URL
        public int Order { get; set; }
    }
}
