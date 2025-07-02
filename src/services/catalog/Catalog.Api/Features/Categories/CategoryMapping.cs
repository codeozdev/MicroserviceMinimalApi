using AutoMapper;
using Catalog.Api.Features.Categories.Create;
using Catalog.Api.Features.Categories.Response;

namespace Catalog.Api.Features.Categories;

public class CategoryMapping : Profile
{
    public CategoryMapping()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();  // Get icin
        CreateMap<CreateCategoryCommand, Category>().ForMember(dest => dest.Name,  // Create icin
            opt => opt.MapFrom(src => src.Name.ToLowerInvariant()));
    }

}



// CommonService.cs extensions methodu icerisinde tanimli bu sayede tum projelerde kullanabiliyoruz