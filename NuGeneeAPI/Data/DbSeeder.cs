using NuGeneeAPI.Models.Entities;
using BCrypt.Net;

namespace NuGeneeAPI.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (context.Admins.Any()) return;

            // 1. Seed Admin
            var admin = new Admin
            {
                Email = "admin@nugenee.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Name = "Super Admin",
                NameAr = "المشرف العام",
                Role = "Super Admin"
            };
            context.Admins.Add(admin);

            // 2. Seed Category
            var category = new Category
            {
                Title = "Web Development",
                TitleAr = "تطوير الويب",
                Description = "Learn modern web development",
                DescriptionAr = "تعلم تطوير الويب الحديث"
            };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            // 3. Seed Instructor
            var instructor = new Instructor
            {
                Name = "John Doe",
                NameAr = "جون دو",
                Role = "Senior Web Developer",
                RoleAr = "مطور ويب أول",
                Bio = "Experienced developer with 10 years in the field.",
                BioAr = "مطور ذو خبرة 10 سنوات في المجال.",
                Expertise = "React, .NET, SQL",
                ExpertiseAr = "رياكت، دوت نت، سيكوال",
                LinkedIn = "https://linkedin.com/in/johndoe"
            };
            context.Instructors.Add(instructor);
            await context.SaveChangesAsync();

            // 4. Seed Course
            var course = new Course
            {
                Title = "Complete ASP.NET Core API",
                TitleAr = "دورة ASP.NET Core API المتكاملة",
                Description = "Master backend dev with .NET 8",
                DescriptionAr = "احترف تطوير الخلفية البرمجية مع دوت نت 8",
                CategoryId = category.Id,
                Active = true,
                Level = "Intermediate",
                LevelAr = "متوسط",
                Duration = "20 Hours",
                DurationAr = "20 ساعة",
                Color = "#336699",
                Icon = "code"
            };
            course.Instructors.Add(instructor);
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            // 5. Seed Table of Content
            var toc = new TableOfContent { CourseId = course.Id };
            var module = new CourseModule
            {
                Title = "Introduction",
                TitleAr = "مقدمة",
                Order = 1,
                TableOfContent = toc
            };
            module.Lessons.Add(new CourseLesson
            {
                Title = "Getting Started",
                Order = 1,
                Type = "video",
                Duration = "10m",
                IsPreview = true
            });
            context.TableOfContents.Add(toc);
            await context.SaveChangesAsync();

            // 6. Seed Learning Path
            var path = new LearningPath
            {
                Name = "Full Stack Developer",
                NameAr = "مطوّر ويب شامل",
                Featured = true,
                Difficulty = "Begginer to Advanced"
            };
            path.PathCourses.Add(new PathCourse { CourseId = course.Id, OrderInPath = 1 });
            context.LearningPaths.Add(path);

            await context.SaveChangesAsync();
        }
    }
}
