using AutoMapper;
using BookStoreApp.Blazor.WebAssembly.UI.Services.Base;

namespace BookStoreApp.Blazor.WebAssembly.UI.Configurations;

public class MapperConfig : Profile //cip...46
{
    public MapperConfig()
    {
        CreateMap<AuthorReadOnlyDto, AuthorUpdateDto>().ReverseMap();
        CreateMap<AuthorDetailsDto, AuthorUpdateDto>().ReverseMap();
        CreateMap<BookDetailsDto, BookUpdateDto>().ReverseMap(); //cip...57
    }
}
