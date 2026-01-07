using AutoMapper;
using NuGeneeAPI.Models.DTOs;
using NuGeneeAPI.Models.Entities;

namespace NuGeneeAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category
            CreateMap<Category, CategoryDto>().ReverseMap();

            // Instructor
            CreateMap<Instructor, InstructorDto>().ReverseMap();

            // Course
            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Title))
                .ForMember(dest => dest.Instructors, opt => opt.MapFrom(src => src.Instructors));
            CreateMap<CourseDto, Course>();

            // TableOfContent
            CreateMap<TableOfContent, TableOfContentDto>().ReverseMap();
            CreateMap<CourseModule, CourseModuleDto>().ReverseMap();
            CreateMap<CourseLesson, CourseLessonDto>().ReverseMap();
            CreateMap<LessonResource, LessonResourceDto>().ReverseMap();

            // CourseProject
            CreateMap<CourseProject, CourseProjectDto>().ReverseMap();

            // LearningPath
            CreateMap<LearningPath, LearningPathDto>().ReverseMap();
            CreateMap<PathCourse, PathCourseDto>().ReverseMap();
            CreateMap<PathProject, PathProjectDto>().ReverseMap();

            // StudentCertificate
            CreateMap<StudentCertificate, StudentCertificateDto>().ReverseMap();

            // Admin
            CreateMap<Admin, AdminDto>().ReverseMap();
        }
    }
}
