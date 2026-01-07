using Microsoft.EntityFrameworkCore;
using NuGeneeAPI.Models.Entities;

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
        }
    }
}
