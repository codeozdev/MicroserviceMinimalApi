using AutoMapper;
using Catalog.Api.Features.Courses.Create;

namespace Catalog.Api.Features.Courses
{
    public class CourseMapping : Profile
    {
        public CourseMapping()
        {
            CreateMap<CreateCourseCommand, Course>().ForMember(dest => dest.Name,  // Create icin
                opt => opt.MapFrom(src => src.Name.ToLowerInvariant()));
        }

    }
}