using BookStoreApp.API.Models;
using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Services;

public interface IAuthorService //cip...44
{
    //Task<Response<List<AuthorReadOnlyDto>>> GetAsync();
    Task<Response<AuthorReadOnlyDtoVirtualiseResponse>> GetAsync(QueryParameters queryParams); //cip...66
    //Task<Response<AuthorReadOnlyDto>> GetAuthorAsync(int id); //cip...46
    Task<Response<AuthorDetailsDto>> GetAsync(int id); //cip...47
    Task<Response<int>> CreateAsync(AuthorCreateDto author); //cip...45
    Task<Response<int>> EditAsync(int id, AuthorUpdateDto author); //cip...46
    Task<Response<int>> DeleteAsync(int id); //cip...48
}
