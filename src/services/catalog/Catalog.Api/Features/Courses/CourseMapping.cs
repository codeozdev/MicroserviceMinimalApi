using AutoMapper;
using Catalog.Api.Features.Courses.Create;
using Catalog.Api.Features.Courses.Response;

namespace Catalog.Api.Features.Courses
{
    public class CourseMapping : Profile
    {
        public CourseMapping()
        {
            CreateMap<CreateCourseCommand, Course>().ForMember(dest => dest.Name, // Create icin
                opt => opt.MapFrom(src => src.Name.ToLowerInvariant()));

            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Feature, FeatureDto>().ReverseMap();
        }
    }
}