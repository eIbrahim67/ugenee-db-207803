using Microsoft.EntityFrameworkCore;
using NuGeneeAPI.Models.Entities;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NuGeneeAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<LearningPath> LearningPaths { get; set; }
        public DbSet<PathCourse> PathCourses { get; set; }
        public DbSet<CourseProject> CourseProjects { get; set; }
        public DbSet<PathProject> PathProjects { get; set; }
        public DbSet<StudentCertificate> StudentCertificates { get; set; }
        public DbSet<TableOfContent> TableOfContents { get; set; }
        public DbSet<CourseModule> CourseModules { get; set; }
        public DbSet<CourseLesson> CourseLessons { get; set; }
        public DbSet<LessonResource> LessonResources { get; set; }
        public DbSet<CourseEnrollmentRequest> CourseEnrollmentRequests { get; set; }
        public DbSet<LearningPathEnrollmentRequest> LearningPathEnrollmentRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Admin configuration
            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.Email)
                .IsUnique();

            // Course ↔ Category (1:N)
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Category)
                .WithMany(cat => cat.Courses)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Value Converter for List<string> with fallback for non-JSON data
            var listConverter = new ValueConverter<List<string>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => HandleListDeserialization(v));

            modelBuilder.Entity<Course>()
                .Property(c => c.Skills).HasConversion(listConverter);
            modelBuilder.Entity<Course>()
                .Property(c => c.SkillsAr).HasConversion(listConverter);
            modelBuilder.Entity<Course>()
                .Property(c => c.Prerequisites).HasConversion(listConverter);
            modelBuilder.Entity<Course>()
                .Property(c => c.PrerequisitesAr).HasConversion(listConverter);
            modelBuilder.Entity<Course>()
                .Property(c => c.Tools).HasConversion(listConverter);
            modelBuilder.Entity<Course>()
                .Property(c => c.Objectives).HasConversion(listConverter);
            modelBuilder.Entity<Course>()
                .Property(c => c.ObjectivesAr).HasConversion(listConverter);
            modelBuilder.Entity<Course>()
                .Property(c => c.TargetAudience).HasConversion(listConverter);
            modelBuilder.Entity<Course>()
                .Property(c => c.TargetAudienceAr).HasConversion(listConverter);

            // LearningPath List<string> Converters
            modelBuilder.Entity<LearningPath>()
                .Property(lp => lp.Outcomes).HasConversion(listConverter);
            modelBuilder.Entity<LearningPath>()
                .Property(lp => lp.OutcomesAr).HasConversion(listConverter);

            // Course ↔ Instructor (M:N)
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Instructors)
                .WithMany(i => i.Courses)
                .UsingEntity(j => j.ToTable("CourseInstructors"));

            // Course ↔ Main Instructor (1:N optional)
            modelBuilder.Entity<Course>()
                .HasOne(c => c.MainInstructor)
                .WithMany()
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.SetNull);

            // LearningPath ↔ Course (M:N via PathCourse)
            modelBuilder.Entity<PathCourse>()
                .HasKey(pc => new { pc.PathId, pc.CourseId });

            modelBuilder.Entity<PathCourse>()
                .HasOne(pc => pc.LearningPath)
                .WithMany(lp => lp.PathCourses)
                .HasForeignKey(pc => pc.PathId);

            modelBuilder.Entity<PathCourse>()
                .HasOne(pc => pc.Course)
                .WithMany(c => c.PathCourses)
                .HasForeignKey(pc => pc.CourseId);

            // Course ↔ TableOfContent (1:1)
            modelBuilder.Entity<TableOfContent>()
                .HasKey(t => t.CourseId);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.TableOfContent)
                .WithOne(t => t.Course)
                .HasForeignKey<TableOfContent>(t => t.CourseId);

            // TableOfContent ↔ Modules (1:N)
            modelBuilder.Entity<CourseModule>()
                .HasOne(m => m.TableOfContent)
                .WithMany(t => t.Modules)
                .HasForeignKey(m => m.TableOfContentId);

            // Modules ↔ Lessons (1:N)
            modelBuilder.Entity<CourseLesson>()
                .HasOne(l => l.Module)
                .WithMany(m => m.Lessons)
                .HasForeignKey(l => l.ModuleId);

            // Lessons ↔ Resources (1:N)
            modelBuilder.Entity<LessonResource>()
                .HasOne(r => r.Lesson)
                .WithMany(l => l.Resources)
                .HasForeignKey(r => r.LessonId);
            
            // LearningPath ↔ PathProjects (1:N)
            modelBuilder.Entity<PathProject>()
                .HasOne(p => p.LearningPath)
                .WithMany(lp => lp.Projects)
                .HasForeignKey(p => p.PathId);

            // Course ↔ CourseProjects (1:N)
            modelBuilder.Entity<CourseProject>()
                .HasOne(p => p.Course)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.CourseId);

            // StudentCertificate Relationships
            modelBuilder.Entity<StudentCertificate>()
                .HasOne(sc => sc.Course)
                .WithMany()
                .HasForeignKey(sc => sc.CourseId);

            modelBuilder.Entity<StudentCertificate>()
                .HasOne(sc => sc.LearningPath)
                .WithMany()
                .HasForeignKey(sc => sc.LearningPathId);

            // Enrollment Tables (Explicit)
            modelBuilder.Entity<CourseEnrollmentRequest>().ToTable("CourseEnrollmentRequests");
            modelBuilder.Entity<LearningPathEnrollmentRequest>().ToTable("LearningPathEnrollmentRequests");
        }
        private static List<string> HandleListDeserialization(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return new List<string>();
            if (!value.TrimStart().StartsWith("[") && !value.TrimStart().StartsWith("{")) 
                return new List<string> { value }; // Treat as single item if not JSON
            
            try 
            {
                return JsonSerializer.Deserialize<List<string>>(value, (JsonSerializerOptions)null) ?? new List<string>();
            }
            catch 
            {
                return new List<string> { value }; // Fallback to raw string as single item
            }
        }
    }
}
