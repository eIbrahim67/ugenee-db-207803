using System.Collections.Generic;

namespace NuGeneeAPI.Models.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
    }

    /// <summary>
    /// Data Transfer Object for creating or updating an instructor.
    /// </summary>
    public class InstructorDto
    {
        public int Id { get; set; }

        /// <example>John Doe</example>
        public string Name { get; set; } = string.Empty;

        /// <example>جون دو</example>
        public string NameAr { get; set; } = string.Empty;

        /// <example>Senior Software Engineer</example>
        public string Role { get; set; } = string.Empty;

        /// <example>مهندس برمجيات أول</example>
        public string RoleAr { get; set; } = string.Empty;

        /// <example>Experienced developer with 10+ years in the industry.</example>
        public string Bio { get; set; } = string.Empty;

        /// <example>مطور ذو خبرة تزيد عن 10 سنوات في الصناعة.</example>
        public string BioAr { get; set; } = string.Empty;

        /// <example>https://example.com/photo.jpg</example>
        public string Photo { get; set; } = string.Empty;

        /// <example>Web Development, Cloud Computing</example>
        public string Expertise { get; set; } = string.Empty;

        /// <example>تطوير الويب، الحوسبة السحابية</example>
        public string ExpertiseAr { get; set; } = string.Empty;

        /// <example>https://linkedin.com/in/johndoe</example>
        public string LinkedIn { get; set; } = string.Empty;

        /// <example>https://twitter.com/johndoe</example>
        public string Twitter { get; set; } = string.Empty;

        /// <example>https://johndoe.com</example>
        public string Website { get; set; } = string.Empty;
    }

    /// <summary>
    /// Data Transfer Object for creating or updating a course.
    /// </summary>
    public class CourseDto
    {
        public string Id { get; set; } = string.Empty;

        /// <example>code</example>
        public string Icon { get; set; } = string.Empty;

        /// <example>Frontend Web Development</example>
        public string Title { get; set; } = string.Empty;

        /// <example>تطوير الواجهات الأمامية للويب</example>
        public string TitleAr { get; set; } = string.Empty;

        /// <example>Learn how to build modern, responsive web applications using HTML, CSS, and JavaScript.</example>
        public string Description { get; set; } = string.Empty;

        /// <example>تعلم كيفية بناء تطبيقات ويب حديثة ومتجاوبة باستخدام HTML و CSS و JavaScript.</example>
        public string DescriptionAr { get; set; } = string.Empty;

        /// <example>Beginner</example>
        public string Level { get; set; } = string.Empty;

        /// <example>مبتدئ</example>
        public string LevelAr { get; set; } = string.Empty;

        /// <example>12 Weeks</example>
        public string Duration { get; set; } = string.Empty;

        /// <example>12 أسبوع</example>
        public string DurationAr { get; set; } = string.Empty;

        /// <example>#4F46E5</example>
        public string Color { get; set; } = string.Empty;

        /// <example>true</example>
        public bool Active { get; set; }

        /// <example>1</example>
        public string CategoryId { get; set; } = string.Empty;

        /// <example>1</example>
        public string InstructorId { get; set; } = string.Empty;

        /// <example>This course introduces the core concepts of frontend development.</example>
        public string Overview { get; set; } = string.Empty;

        /// <example>تقدم هذه الدورة المفاهيم الأساسية لتطوير الواجهات الأمامية.</example>
        public string OverviewAr { get; set; } = string.Empty;

        /// <example>3 sessions per week</example>
        public string Schedule { get; set; } = string.Empty;

        /// <example>3 حصص أسبوعيًا</example>
        public string ScheduleAr { get; set; } = string.Empty;

        /// <example>["HTML5", "CSS3", "JavaScript"]</example>
        public List<string> Skills { get; set; } = new();

        /// <example>["HTML5", "CSS3", "جافاسكريبت"]</example>
        public List<string> SkillsAr { get; set; } = new();

        /// <example>["Basic computer skills"]</example>
        public List<string> Prerequisites { get; set; } = new();

        /// <example>["مهارات أساسية في استخدام الحاسوب"]</example>
        public List<string> PrerequisitesAr { get; set; } = new();

        /// <example>["VS Code", "Chrome"]</example>
        public List<string> Tools { get; set; } = new();

        /// <example>["Build responsive websites"]</example>
        public List<string> Objectives { get; set; } = new();

        /// <example>["بناء مواقع ويب متجاوبة"]</example>
        public List<string> ObjectivesAr { get; set; } = new();

        /// <example>Certificate of Completion</example>
        public string Certification { get; set; } = string.Empty;

        /// <example>شهادة إتمام</example>
        public string CertificationAr { get; set; } = string.Empty;

        /// <example>["Beginners"]</example>
        public List<string> TargetAudience { get; set; } = new();

        /// <example>["المبتدئين"]</example>
        public List<string> TargetAudienceAr { get; set; } = new();
    }

    public class CourseDetailDto : CourseDto
    {
        public TableOfContentDto? TableOfContent { get; set; }
    }

    public class TableOfContentDto
    {
        public List<CourseModuleDto> Modules { get; set; } = new();
    }

    /// <summary>
    /// Data Transfer Object for creating or updating a course module.
    /// </summary>
    public class CourseModuleDto
    {
        public int Id { get; set; }

        /// <example>1</example>
        public int Order { get; set; }

        /// <example>Introduction to Web Development</example>
        public string Title { get; set; } = string.Empty;

        /// <example>مقدمة في تطوير الويب</example>
        public string TitleAr { get; set; } = string.Empty;

        /// <example>2 hours</example>
        public string Duration { get; set; } = string.Empty;

        /// <example>1</example>
        public int TableOfContentId { get; set; }
        
        public List<CourseLessonDto> Lessons { get; set; } = new();
    }

    /// <summary>
    /// Data Transfer Object for creating or updating a course lesson.
    /// </summary>
    public class CourseLessonDto
    {
        public int Id { get; set; }

        /// <example>1</example>
        public int Order { get; set; }

        /// <example>What is HTML?</example>
        public string Title { get; set; } = string.Empty;

        /// <example>ما هو HTML؟</example>
        public string TitleAr { get; set; } = string.Empty;

        /// <example>video</example>
        public string Type { get; set; } = string.Empty;

        /// <example>1</example>
        public int ModuleId { get; set; }

        public List<LessonResourceDto> Resources { get; set; } = new();
    }

    public class LessonResourceDto
    {
        /// <example>pdf</example>
        public string Type { get; set; } = string.Empty;

        /// <example>Lecture Slides</example>
        public string Title { get; set; } = string.Empty;

        /// <example>https://example.com/slides.pdf</example>
        public string Url { get; set; } = string.Empty;
    }

    public class AdminDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LearningPathDto
    {
        public int Id { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string SubtitleAr { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string Goal { get; set; } = string.Empty;
        public string GoalAr { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string DurationAr { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public string DifficultyAr { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public List<string> Outcomes { get; set; } = new();
        public List<string> OutcomesAr { get; set; } = new();
        public bool IsCustomizable { get; set; }
        public bool Featured { get; set; }
        public List<PathCourseDto> PathCourses { get; set; } = new();
    }

    public class PathCourseDto
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public int OrderInPath { get; set; }
        public bool IsOptional { get; set; }
    }

    public class PathProjectDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
    }

    public class CourseProjectDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
    }

    public class StudentCertificateDto
    {
        public string StudentName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
    }
}
