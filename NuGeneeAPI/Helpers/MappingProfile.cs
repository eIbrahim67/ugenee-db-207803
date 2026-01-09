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

            // Course - Entity to DTO (int to string)
            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId.ToString()))
                .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => src.InstructorId.HasValue ? src.InstructorId.Value.ToString() : string.Empty));
            
            // Course - DTO to Entity (string to int)
            CreateMap<CourseDto, Course>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // ID is auto-generated
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => int.Parse(src.CategoryId)))
                .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => 
                    string.IsNullOrEmpty(src.InstructorId) ? (int?)null : int.Parse(src.InstructorId)))
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.MainInstructor, opt => opt.Ignore())
                .ForMember(dest => dest.Instructors, opt => opt.Ignore())
                .ForMember(dest => dest.PathCourses, opt => opt.Ignore())
                .ForMember(dest => dest.Projects, opt => opt.Ignore())
                .ForMember(dest => dest.TableOfContent, opt => opt.Ignore());

            // CourseDetailDto - includes TableOfContent
            CreateMap<Course, CourseDetailDto>()
                .IncludeBase<Course, CourseDto>();

            // TableOfContent
            CreateMap<TableOfContent, TableOfContentDto>().ReverseMap();
            // Course Content with explicit ID ignoring for creation
            CreateMap<CourseModule, CourseModuleDto>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Lessons, opt => opt.MapFrom(src => src.Lessons));
            
            CreateMap<CourseLesson, CourseLessonDto>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ModuleId, opt => opt.Ignore()) // Inferred from parent
                .ForMember(dest => dest.Resources, opt => opt.MapFrom(src => src.Resources));
            
            CreateMap<LessonResource, LessonResourceDto>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.LessonId, opt => opt.Ignore()); // Inferred from parent

            // CourseProject
            CreateMap<CourseProject, CourseProjectDto>().ReverseMap();

            // LearningPath
            CreateMap<LearningPath, LearningPathDto>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PathCourses, opt => opt.MapFrom(src => src.PathCourses));

            CreateMap<PathCourse, PathCourseDto>()
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : string.Empty));
            
            CreateMap<PathCourseDto, PathCourse>()
                .ForMember(dest => dest.PathId, opt => opt.Ignore())
                .ForMember(dest => dest.LearningPath, opt => opt.Ignore())
                .ForMember(dest => dest.Course, opt => opt.Ignore());

            CreateMap<PathProject, PathProjectDto>().ReverseMap();

            // StudentCertificate
            CreateMap<StudentCertificate, StudentCertificateDto>().ReverseMap();

            // Admin
            CreateMap<Admin, AdminDto>().ReverseMap();

            // Enrollments
            CreateMap<CourseEnrollmentRequest, CourseEnrollmentRequestDto>()
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Course, opt => opt.Ignore());

            CreateMap<LearningPathEnrollmentRequest, LearningPathEnrollmentRequestDto>()
                .ForMember(dest => dest.PathName, opt => opt.MapFrom(src => src.LearningPath != null ? src.LearningPath.Name : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LearningPath, opt => opt.Ignore());
        }
    }
}
